using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sail
{
    public class MoveAction : ActionBase
    {
        public static new ActionID ID { get; set; } = ActionID.MOVE;


        public override int SelectionCount => 1; //{ get { return 1; } }

        public override IEnumerator run()
        {
            Debug.Log("Start run action.");

            var character = BattleFSM.Instance.SelectedEnemy;

            //TODO tmp
            var target = BattleFSM.Instance.TileSelectionController.SelectedTiles[BattleFSM.Instance.TileSelectionController.SelectedTiles.Count - 1];

            yield return ActionFunctionLibrary.moveTo(character.gameObject, target);

            character.Coord = target;

            Debug.Log("End run action.");
        }
    }

    public class AttackAction : ActionBase
    {
        public static new ActionID ID { get; set; } = ActionID.ATTACK;
    }
}