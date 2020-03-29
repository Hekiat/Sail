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
        public TimelineController TimelineController;
        //public LevelData levelData;
        //public Transform tileSelectionIndicator;
        //public Point pos;

        public List<EnemyBase> enemies = new List<EnemyBase>();
        public EnemyBase SelectedEnemy = null;

        public ActionController ActionController { get; private set; }

        private void Awake()
        {
            Instance = this;

            TimelineController = GetComponent<TimelineController>();
            ActionController = GetComponent<ActionController>();
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