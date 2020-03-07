using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public static class GlobalManagers
    {
        // System
        public static MouseInputManager mouseInputManager = null;

        // Gameplay
        public static GameFlowManager gameManager = null;
        public static BoardManager boardManager = null;
        public static ActionManager actionManager = null;

        // UI
        public static BattleHUD hud = null;
    }
}
