﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapManager : MonoBehaviour
{
    public int Width = 10;
    public int Height = 10;
    public float Spacing = 0.05f;

    public float BlendDuration = 2f;

    public Vector3 MapCenterPosition = Vector3.zero;

    public TileType TileType { get; private set; } = TileType.Cube;
    private List<Tile> Tiles = new List<Tile>();

    // Start is called before the first frame update
    void Start()
    {
        for (int j = 0; j < Height; ++j)
        {
            for (int i = 0; i < Width; ++i)
            {
                var coord = new TileCoord(i, j);
                var pos = getTilePosition(coord);

                Tile ti = new Tile(pos, Quaternion.identity, transform, coord);
                Tiles.Add(ti);

                //var meshc = ti.GameObject.AddComponent<MeshCollider>();
                //meshc.sharedMesh = ti.GameObject.GetComponent<MeshFilter>().mesh;

                //ti.Button.onClick.AddListener(() => { onTileClicked(ti); });
            }
        }

        updateMapCenterPosition();

        GlobalManagers.mapManager = this;
    }

    void OnDestroy()
    {
        GlobalManagers.mapManager = null;
    }

    void updateMapCenterPosition()
    {
        //var bottomLeft = getTilePosition(new TileCoord(0, 0));
        //var topRight = getTilePosition(new TileCoord(Width - 1, Height - 1));

        var bottomLeft = Tiles[0].GameObject.transform.position;
        var topRight = Tiles[Tiles.Count-1].GameObject.transform.position;

        MapCenterPosition = (topRight - bottomLeft) / 2f;
        MapCenterPosition += Vector3.up * 1f;
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
        float duration = 0f;

        List<Vector3> initialPositions = new List<Vector3>();
        foreach (var tile in Tiles)
        {
            initialPositions.Add(tile.GameObject.transform.position);
        }

        while (duration < BlendDuration)
        {
            duration = Mathf.Min(duration + Time.deltaTime, BlendDuration);
            var ratio = duration / BlendDuration;

            for (int i = 0; i < Tiles.Count; ++i)
            {
                var tile = Tiles[i];
                var targetPos = getTilePosition(tile.Coord) + tile.HeightOffset;

                tile.GameObject.transform.position = Vector3.Lerp(initialPositions[i], targetPos, ratio);
            }

            yield return null;
        }
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
        updateMapCenterPosition();
    }

    void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        if(EditorApplication.isPlaying == false)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(MapCenterPosition, 0.5f);

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

            return  x + y;
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

            var x = Vector3.right   * ((float)coord.Square.x + xOffset + spacingOffsetX);
            var y = Vector3.forward * ((float)coord.Square.y + yOffset + spacingOffsetY);

            return x + y;
        }

        return Vector3.zero;
    }

    public void TileClicked(GameObject tileGO)
    {
        var tile = Tiles.Find(t => t.GameObject == tileGO);

        if (tile == null)
        {
            Debug.Log("Tile clicked is not found");
            return;
        }

        if (tile.Coord.Square.x == 0 && tile.Coord.Square.y == 0)
        {
            clearTilesSelection();
            return;
        }

        AreaMapSelectionController controller = new AreaMapSelectionController(tile.Coord);
        controller.update();

        //setTileSelected(tile);
    }

    private void setTileSelected(Tile tile)
    {
        // tmp code
        tile.GameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    public void setTilesSelected(List<TileCoord> coords)
    {
        foreach (var coord in coords)
        {
            setTileSelected(coord);
        }
    }

    public void setTileSelected(TileCoord coord)
    {
        var tile = getTile(coord);
        if (tile != null)
        {
            setTileSelected(tile);
        }
    }

    public void clearTilesSelection()
    {
        foreach (var tile in Tiles)
        {
            tile.GameObject.GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

    private Tile getTile(TileCoord coord)
    {
        if (coord.isValid() == false)
        {
            return null;
        }

        return Tiles[coord.Square.y * Width + coord.Square.x];
    }
}

[CustomEditor(typeof(MapManager))]
public class MapManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapManager map = (MapManager)target;
        if (GUILayout.Button("Switch"))
        {
            map.Switch();
        }
    }
}