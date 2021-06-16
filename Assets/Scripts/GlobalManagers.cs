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
        public static EventManager eventManager = null;
        public static CharacterInfoManager characterInfoManager = null;

        // UI
        public static BattleHUD hud = null;
    }
}
