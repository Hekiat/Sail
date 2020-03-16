using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class BattleFSM : StateMachine
    {
        static public BattleFSM Instance = null;

        public GameObject HUDPrefab = null;
        public List<GameObject> UnitsPrefab = new List<GameObject>();

        public Board board;
        public BattleHUD hud;
        public TimelineController timelineController;
        //public LevelData levelData;
        //public Transform tileSelectionIndicator;
        //public Point pos;

        public List<EnemyCore> enemies = new List<EnemyCore>();
        public EnemyCore SelectedEnemy = null;

        private void Awake()
        {
            Instance = this;

            timelineController = GetComponent<TimelineController>();
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