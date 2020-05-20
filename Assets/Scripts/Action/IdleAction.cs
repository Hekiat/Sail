using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class IdleAction : ActionBase
    {
        public static new ActionID ID { get; set; } = ActionID.IDLE;
        public override ActionID id() { return ID; }

        public override void start()
        {
            base.start();
            Unit.MotionController.requestMotion(EmMotionStates.Idle, 0.5f);
        }
    }
}