using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public static class GlobalManagers
    {
        // Gameplay
        public static GameFlowManager gameManager = null;
        public static Board board = null;
        public static ActionManager actionManager = null;
        public static DialogueManager dialogueManager = null;

        // UI
        public static BattleHUD hud = null;
    }
}
