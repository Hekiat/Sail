using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class ActionSelectionModel
    {
        public TileSelectionModelBase SelectionModel { get; set; } = null;
        public TileSelectionModelBase TargetModel { get; set; } = null;

        public ActionSelectionModel(TileSelectionModelBase selection, TileSelectionModelBase target)
        {
            SelectionModel = selection;
            TargetModel = target;
        }
    }
}
