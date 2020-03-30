using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class TileSelectionController : MonoBehaviour
    {
        public TileSelectionBase currentSelection { get; set; } = null;

        public List<TileCoord> SelectedTiles = new List<TileCoord>();

        protected virtual void select()
        {
            clear();
            GlobalManagers.board.clearTilesSelection();
        }

        public virtual void clear()
        {
            SelectedTiles.Clear();
            GlobalManagers.board.clearTilesSelection();
        }
    }
}