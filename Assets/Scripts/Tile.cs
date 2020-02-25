using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public enum TileType
    {
        Cube,
        Hex,
        Count
    };

    public class Tile
    {
        public GameObject GameObject { get; set; } = null;
        public TileMesh Mesh { get; set; } = null;
        public UnityEngine.UI.Text Text { get; set; } = null;

        public TileCoord Coord;

        public Vector3 HeightOffset = Vector3.zero;

        private static GameObject TilePrefab = null;

        public Tile(Vector3 pos, Quaternion rot, Transform trans, TileCoord coord)
        {
            if (TilePrefab == null)
            {
                TilePrefab = Resources.Load<GameObject>("Prefab/Tile");

                if (TilePrefab == null)
                {
                    Debug.Log("Can't load Tile prefab.");
                    return;
                }
            }

            HeightOffset = Vector3.up * Random.value * 0.2f;
            GameObject = GameObject.Instantiate(TilePrefab, pos + HeightOffset, rot, trans);
            Mesh = GameObject.GetComponent<TileMesh>();
            Text = GameObject.GetComponentInChildren<UnityEngine.UI.Text>();
            Coord = coord;

            var text = $"({Coord.Square.x}, {Coord.Square.y})\n";
            text += $"({Coord.Hex.q}, {Coord.Hex.r}, {Coord.Hex.s})";
            Text.text = text;

            //var mc = GameObject.GetComponent<MeshCollider>();
            //mc.sharedMesh = null;
            //mc.sharedMesh = GameObject.GetComponent<MeshFilter>().mesh;
        }

        public override string ToString()
        {
            return $"({Coord.Square.x}, {Coord.Square.y})";
        }
    }
}