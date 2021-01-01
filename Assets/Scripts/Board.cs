using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using sail.animation;

namespace sail
{
    public class BoardGraph : GraphBase<TileCoord>
    {
        public Board board = null;

        public override List<Node<TileCoord>> neighbors(Node<TileCoord> node)
        {
            var neighbors = board.neighbors(node.pos);
            List<Node<TileCoord>> tilesNodes = new List<Node<TileCoord>>();

            foreach(var n in neighbors)
            {
                var tileNode = nodes.Find((item) => item.pos == n);
                tilesNodes.Add(tileNode);
            }

            return tilesNodes;
        }
    }

    public class Board : MonoBehaviour
    {
        public GameObject TilePrefab = null;

        public int Width = 10;
        public int Height = 10;
        public float Spacing = 0.05f;

        public float BlendDuration = 2f;

        public Vector3 CenterPosition = Vector3.zero;

        public TileType TileType { get; private set; } = TileType.Cube;
        public AreaTileSelection.AreaType SelectionType = AreaTileSelection.AreaType.Circle;

        private List<Tile> Tiles = new List<Tile>();
        private BoardGraph Graph = new BoardGraph();
        private MapWidget.AStarSearch<TileCoord> AStar = null;

        // Start is called before the first frame update
        void Start()
        {
            GlobalManagers.board = this;
        }

        void OnDestroy()
        {
            GlobalManagers.board = null;
        }

        public void Generate()
        {
            for (int j = 0; j < Height; ++j)
            {
                for (int i = 0; i < Width; ++i)
                {
                    var coord = new TileCoord(i, j);
                    var pos = getTilePosition(coord);

                    var heightOffset = Vector3.up * Random.value * 0.2f;
                    var tileGO = Instantiate(TilePrefab, pos + heightOffset, Quaternion.identity, transform);

                    Tile tile = tileGO.GetComponent<Tile>();
                    tile.Coord = coord;
                    tile.HeightOffset = heightOffset;
                    Tiles.Add(tile);
                }
            }

            updateBoardCenterPosition();

            updateNavigation();
        }

        void updateNavigation()
        {
            Graph.board = this;

            // Update Graph
            Graph.clear();

            foreach(var tile in Tiles)
            {
                Graph.addNode(tile.Coord);
            }

            // Update AStar
            AStar = new MapWidget.AStarSearch<TileCoord>(BattleFSM.Instance.board.Graph);
            AStar.heuristic = (TileCoord start, TileCoord current, TileCoord goal) =>
            {
                var h = Mathf.Abs(current.Square.x - goal.Square.x) + Mathf.Abs(current.Square.y - goal.Square.y);
                var dx1 = current.Square.x - goal.Square.x;
                var dy1 = current.Square.y - goal.Square.y;
                var dx2 = start.Square.x - goal.Square.x;
                var dy2 = start.Square.y - goal.Square.y;
                var cross = Mathf.Abs(dx1 * dy2 - dx2 * dy1);
                return h + cross * 0.001f;
            };
        }

        public List<TileCoord> getPath(TileCoord from, TileCoord to)
        {
            return AStar.getPath(from, to);
        }

        void updateBoardCenterPosition()
        {
            if (Tiles == null || Tiles.Count == 0)
            {
                return;
            }

            //var bottomLeft = getTilePosition(new TileCoord(0, 0));
            //var topRight = getTilePosition(new TileCoord(Width - 1, Height - 1));

            var bottomLeft = Tiles[0].transform.position;
            var topRight = Tiles[Tiles.Count - 1].transform.position;

            CenterPosition = (topRight - bottomLeft) / 2f;
            CenterPosition += Vector3.up * 1f;
        }

        void onTileClicked(Tile tile)
        {
            Debug.Log(tile.ToString());
        }

        IEnumerator startMorph()
        {
            foreach (var tile in Tiles)
            {
                tile.Mesh.morphTo(BlendDuration, TileType);
            }

            yield return new WaitForSeconds(BlendDuration);
        }

        IEnumerator startRootBlend()
        {
            List<sail.animation.Tweener> tweenerList = new List<sail.animation.Tweener>();

            for (int i = 0; i < Tiles.Count; ++i)
            {
                var tile = Tiles[i];
                var targetPos = getTilePosition(tile.Coord) + tile.HeightOffset;

                var t = tile.transform.MoveTo(targetPos);
                
                tweenerList.Add(t);
            }

            bool allTweenerEnded = false;

            while (allTweenerEnded == false)
            {
                var foundTweener = tweenerList.Find(a => a != null);
                allTweenerEnded = foundTweener == null;

                if (allTweenerEnded == false)
                {
                    yield return null;
                }
            }


            //float duration = 0f;
            //
            //List<Vector3> initialPositions = new List<Vector3>();
            //foreach (var tile in Tiles)
            //{
            //    initialPositions.Add(tile.transform.position);
            //}
            //
            //while (duration < BlendDuration)
            //{
            //    duration = Mathf.Min(duration + Time.deltaTime, BlendDuration);
            //    var ratio = duration / BlendDuration;
            //
            //    for (int i = 0; i < Tiles.Count; ++i)
            //    {
            //        var tile = Tiles[i];
            //        var targetPos = getTilePosition(tile.Coord) + tile.HeightOffset;
            //
            //        tile.transform.position = Vector3.Lerp(initialPositions[i], targetPos, ratio);
            //    }
            //
            //    yield return null;
            //}
        }

        private IEnumerator startTransitionToHex()
        {
            TileType = TileType.Hex;

            yield return startMorph();
            yield return startRootBlend();
        }

        private IEnumerator startTransitionToCube()
        {
            TileType = TileType.Cube;

            yield return startRootBlend();
            yield return startMorph();
        }

        // Update is called once per frame
        void Update()
        {
            updateBoardCenterPosition();
        }

        void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying == false)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(CenterPosition, 0.5f);

                for (int i = 0; i < Width; ++i)
                {
                    for (int j = 0; j < Height; ++j)
                    {
                        var coord = new TileCoord(i, j);
                        Gizmos.DrawWireCube(getTilePosition(coord), Vector3.one);
                    }
                }
            }
#endif
        }

        public void Switch()
        {
            if (TileType == TileType.Hex)
            {
                StartCoroutine(startTransitionToCube());
            }
            else if (TileType == TileType.Cube)
            {
                StartCoroutine(startTransitionToHex());
            }
        }

        Vector3 getTilePosition(TileCoord coord)
        {
            float spacingOffsetX = coord.Square.x * Spacing;
            float spacingOffsetY = coord.Square.y * Spacing;

            if (TileType == TileType.Cube)
            {
                var x = Vector3.right * ((float)coord.Square.x + spacingOffsetX);
                var y = Vector3.forward * ((float)coord.Square.y + spacingOffsetY);

                return x + y;
            }
            if (TileType == TileType.Hex)
            {
                float tileRadius = 0.5f;
                float tileWidth = Mathf.Sqrt(3) * tileRadius / 2;

                float oddRowOffset = coord.Square.y % 2 == 0 ? 0f : tileWidth;
                float oddRowSpacingOffset = coord.Square.y % 2 == 0 ? 0f : Spacing / 2f;
                spacingOffsetX += oddRowSpacingOffset;
                float sizeDeltaOffset = coord.Square.x * (tileRadius - tileWidth) * 2f;

                var xOffset = oddRowOffset - sizeDeltaOffset;
                var yOffset = -coord.Square.y * tileRadius / 2f;

                var x = Vector3.right * ((float)coord.Square.x + xOffset + spacingOffsetX);
                var y = Vector3.forward * ((float)coord.Square.y + yOffset + spacingOffsetY);

                return x + y;
            }

            return Vector3.zero;
        }

        public Tile getTile(TileCoord coord)
        {
            if (coord.isValid() == false)
            {
                return null;
            }

            return Tiles[coord.Square.y * Width + coord.Square.x];
        }

        static readonly TileCoord[] CubeNeighborsOffset = {
        new TileCoord(-1, 0),
        new TileCoord(0, -1),
        new TileCoord(1, 0),
        new TileCoord(0, 1) };

        static readonly TileCoord[] HexNeighborsOffset = {
        new TileCoord(-1, 1, 0),
        new TileCoord(0, 1, -1),
        new TileCoord(1, 0, -1),
        new TileCoord(1, -1, 0),
        new TileCoord(0, -1, 1),
        new TileCoord(-1, 0, 1),
        };

        public List<TileCoord> neighbors(TileCoord coord)
        {
            List<TileCoord> neighborList = new List<TileCoord>();

            if (TileType == TileType.Cube)
            {
                foreach (var n in CubeNeighborsOffset)
                {
                    var current = new TileCoord(coord.Square.x + n.Square.x, coord.Square.y + n.Square.y);
                    if (current.isValid())
                    {
                        neighborList.Add(current);
                    }
                }
            }
            else if (TileType == TileType.Hex)
            {
                foreach (var n in CubeNeighborsOffset)
                {
                    TileCoord current = new TileCoord(coord.Hex.q + n.Hex.q, coord.Hex.r + n.Hex.r, coord.Hex.s + n.Hex.s);
                    if (current.isValid())
                    {
                        neighborList.Add(current);
                    }
                }
            }

            return neighborList;
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Board))]
    public class BoardManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Board board = (Board)target;
            if (GUILayout.Button("Switch"))
            {
                board.Switch();
            }
        }
    }
    #endif
}