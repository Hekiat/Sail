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

        public static MotionState RangedAttack = new MotionState("RangedAttack");

        public static MotionState Fire = new MotionState("Fire");

        public static MotionState Jump = new MotionState("Jump");
        public static MotionState JumpAttack = new MotionState("JumpAttack");

        public static MotionState DashAttack = new MotionState("DashAttack");

        public static MotionState Shield = new MotionState("Shield");
        
        //public static MotionState Walk = new MotionState("Walk");
    }
}