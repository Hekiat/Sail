using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class TileSelectionController : MonoBehaviour
    {
        public TileSelectionBase currentSelection { get; set; } = null;

        public List<TileCoord> SelectedTiles = new List<TileCoord>();
        public List<TileCoord> HighlightedTiles = new List<TileCoord>();

        ///
        /// SELECTION
        ///
        public void setSelection(TileCoord tile)
        {
            clearSelection();
            addToSelection(tile);
        }

        public void setSelection(List<TileCoord> tiles)
        {
            clearSelection();
            addToSelection(tiles);
        }

        public void addToSelection(TileCoord tile)
        {
            SelectedTiles.Add(tile);
            BattleFSM.Instance.board.setTileColor(tile, Color.blue);
        }

        public void addToSelection(List<TileCoord> tiles)
        {
            foreach (var tile in tiles)
            {
                addToSelection(tile);
            }
        }

        public void clearSelection()
        {
            var board = GlobalManagers.board;
            foreach (var tile in SelectedTiles)
            {
                board.getTile(tile).GetComponent<MeshRenderer>().material.color = Color.white;
            }

            SelectedTiles.Clear();
        }

        ///
        /// HIGHLIGHT
        ///

        public void setHighlighted(TileCoord tile)
        {
            clear();
            addToHighlighted(tile);
        }

        public void setHighlighted(List<TileCoord> tiles)
        {
            clear();
            addToHighlighted(tiles);
        }

        public void addToHighlighted(TileCoord tile)
        {
            HighlightedTiles.Add(tile);
            BattleFSM.Instance.board.setTileColor(tile, Color.cyan);
        }

        public void addToHighlighted(List<TileCoord> tiles)
        {
            foreach (var tile in tiles)
            {
                addToHighlighted(tile);
            }
        }

        public void clearHighlighted()
        {
            var board = GlobalManagers.board;
            foreach (var tile in HighlightedTiles)
            {
                board.getTile(tile).GetComponent<MeshRenderer>().material.color = Color.white;
            }

            HighlightedTiles.Clear();
        }

        public void clear()
        {
            clearSelection();
            clearHighlighted();
        }
    }
}