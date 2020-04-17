using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class TileSelectionController : MonoBehaviour
    {
        public TileCoord? HoveredTile { get; set; } = null;

        int CurrentStackID = 0;
        List<TileSelectionElement> SelectionStack = new List<TileSelectionElement>();

        public void next()
        {
            CurrentStackID++;
        }

        public List<TileCoord> selectedTiles()
        {
            return SelectionStack[CurrentStackID].get(TileLayerID.SELECTED);
        }

        public void enable()
        {
            var models = BattleFSM.Instance.ActionController.Action.selectionModels();

            if (models == null || models.Count == 0)
            {
                return;
            }

            var character = BattleFSM.Instance.SelectedEnemy;

            foreach (var model in models)
            {
                SelectionStack.Add(new TileSelectionElement(model, character));
            }

            CurrentStackID = 0;
            SelectionStack[0].enable();
        }

        public void disable()
        {
            if (SelectionStack.Count == 0)
            {
                return;
            }

            SelectionStack[CurrentStackID].disable();
            SelectionStack.Clear();
        }

        public void updateHoveredTile()
        {
            SelectionStack[CurrentStackID].clear(TileLayerID.HIGHLIGHTED);

            if (HoveredTile == null)
            {
                return;
            }

            SelectionStack[CurrentStackID].set(HoveredTile.Value, TileLayerID.HIGHLIGHTED);

            // TMP
            var fsm = BattleFSM.Instance;
            if (fsm.ActionController.Action.Name == "Move")
            {
                var path = AStarSearch.search(fsm.SelectedEnemy.Coord, HoveredTile.Value);
                SelectionStack[CurrentStackID].set(path, TileLayerID.HIGHLIGHTED);
            }
        }

        // TMP
        public void highlight(List<TileCoord> list)
        {
            SelectionStack[CurrentStackID].add(list, TileLayerID.HIGHLIGHTED);
        }

        public void select(TileCoord tile)
        {
            SelectionStack[CurrentStackID].add(tile, TileLayerID.SELECTED);
        }

        private void Update()
        {
            if(SelectionStack.Count == 0)
            {
                return;
            }

            updateHoveredTile();
            SelectionStack[CurrentStackID].update();
        }
    }
}