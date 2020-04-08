using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class TileSelectionController : MonoBehaviour
    {
        public TileSelectionBase currentSelection { get; set; } = null;

        public List<TileCoord> SelectedTiles = new List<TileCoord>();

        public void select(List<TileCoord> tiles)
        {
            clear();
            addToSelection(tiles);
        }

        public void select(TileCoord tile)
        {
            clear();
            addToSelection(tile);
        }

        public void addToSelection(List<TileCoord> tiles)
        {
            SelectedTiles = tiles;
            BattleFSM.Instance.board.setTilesColor(SelectedTiles, Color.blue);
        }

        public void addToSelection(TileCoord tile)
        {
            SelectedTiles.Add(tile);
            BattleFSM.Instance.board.setTileColor(tile, Color.blue);
        }

        public void addToHighlight(List<TileCoord> tiles)
        {
            BattleFSM.Instance.board.setTilesColor(tiles, Color.cyan);
        }

        public void clear()
        {
            SelectedTiles.Clear();
            GlobalManagers.board.clearTilesSelection();
        }
    }
}