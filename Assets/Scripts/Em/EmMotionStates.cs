using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public static class EmMotionStates
    {
        public static MotionState Idle = new MotionState("Idle");
        public static MotionState Walk = new MotionState("Walk");

        public static MotionState BasicAttack = new MotionState("BasicAttack");
        public static MotionState Fire = new MotionState("Fire");


        //public static MotionState Walk = new MotionState("Walk");
    }
}