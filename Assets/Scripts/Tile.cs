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

    public class Tile : MonoBehaviour
    {
        public TileMesh Mesh { get; set; } = null;
        public UnityEngine.UI.Text Text { get; set; } = null;
        private MeshRenderer MeshRenderer { get; set; } = null;

        TileCoord _Coord;
        public TileCoord Coord {
            get { return _Coord; }
            set { _Coord = value; updateText(); }
        }

        public Vector3 HeightOffset = Vector3.zero;

        private void Awake()
        {
            Mesh = gameObject.GetComponent<TileMesh>();
            Text = gameObject.GetComponentInChildren<UnityEngine.UI.Text>();
            MeshRenderer = gameObject.GetComponent<MeshRenderer>();
        }

        void updateText()
        {
            var text = $"({Coord.Square.x}, {Coord.Square.y})\n";
            text += $"({Coord.Hex.q}, {Coord.Hex.r}, {Coord.Hex.s})";
            Text.text = text;
        }

        public override string ToString()
        {
            return $"({Coord.Square.x}, {Coord.Square.y})";
        }

        public void setColor(Color color)
        {
            MeshRenderer.material.color = color;
        }
    }
}