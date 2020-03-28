using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sail
{
    public class MoveAction : ActionBase
    {
        public static new ActionID ID { get; set; } = ActionID.MOVE;
    }

    public class AttackAction : ActionBase
    {
        public static new ActionID ID { get; set; } = ActionID.ATTACK;
    }
}