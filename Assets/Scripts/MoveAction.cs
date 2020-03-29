using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sail
{
    public class MoveAction : ActionBase
    {
        public static new ActionID ID { get; set; } = ActionID.MOVE;

        public override IEnumerator run()
        {
            Debug.Log("Start run action.");

            var character = BattleFSM.Instance.enemies[0].gameObject;
            yield return ActionFunctionLibrary.moveTo(character, new TileCoord(5, 5));
            Debug.Log("End run action.");
        }
    }

    public class AttackAction : ActionBase
    {
        public static new ActionID ID { get; set; } = ActionID.ATTACK;
    }
}