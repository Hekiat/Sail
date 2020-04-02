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

        public void addToSelection(List<TileCoord> tiles)
        {
            SelectedTiles = tiles;
            BattleFSM.Instance.board.setTilesSelected(SelectedTiles);
        }

        public void addToSelection(TileCoord tile)
        {
            SelectedTiles.Add(tile);
            BattleFSM.Instance.board.setTileSelected(tile);
        }

        public void clear()
        {
            SelectedTiles.Clear();
            GlobalManagers.board.clearTilesSelection();
        }
    }
}