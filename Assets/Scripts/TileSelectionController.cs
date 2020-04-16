using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class TileSelectionController : MonoBehaviour
    {
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
            SelectionStack[CurrentStackID].disable();
            SelectionStack.Clear();
        }


        // TMP
        public void highlight(List<TileCoord> list)
        {
            SelectionStack[CurrentStackID].add(TileLayerID.HIGHLIGHTED, list);
        }

        public void select(TileCoord tile)
        {
            SelectionStack[CurrentStackID].add(TileLayerID.SELECTED, tile);
        }
    }
}