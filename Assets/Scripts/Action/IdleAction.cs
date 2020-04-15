using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class IdleAction : ActionBase
    {
        public static new ActionID ID { get; set; } = ActionID.IDLE;

        public override void start()
        {
            var character = BattleFSM.Instance.SelectedEnemy;
            character.Animator.CrossFade("Idle", 0.5f);
        }
    }
}