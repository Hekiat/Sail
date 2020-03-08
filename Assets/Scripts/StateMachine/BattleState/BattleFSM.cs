using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class BattleFSM : StateMachine
    {
        static public BattleFSM Instance = null;

        public GameObject HUDPrefab = null;

        public Board board;
        public BattleHUD hud;
        //public LevelData levelData;
        //public Transform tileSelectionIndicator;
        //public Point pos;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            ChangeToState<BattleInitState>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}