using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public enum TurnType
    {
        Player,
        Enemy
    }

    public enum PlayerTurnPhase
    {
        Start,
        ActionSetup,
        ActionTargetSelection,
        ActionRun,
        End
    }

    public class GameFlowManager : MonoBehaviour
    {
        public GameObject HUDPrefab = null;

        public TurnType TurnType { get; private set; }
        public PlayerTurnPhase PlayerTurnPhase { get; private set; }

        public delegate void TurnChangedDelegate(TurnType type);
        public event TurnChangedDelegate TurnChanged;

        public delegate void PhaseChangedDelegate(PlayerTurnPhase phase);
        public event PhaseChangedDelegate PhaseChanged;

        void Awake()
        {
            GlobalManagers.gameManager = this;
        }

        void Start()
        {
            if (HUDPrefab)
            {
                Instantiate(HUDPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            }
        }


        void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                var enumLen = System.Enum.GetNames(typeof(PlayerTurnPhase)).Length;
                PlayerTurnPhase = (PlayerTurnPhase)((int)(PlayerTurnPhase + 1) % enumLen);

                PhaseChanged(PlayerTurnPhase);
            }
        }

        void updateTurn()
        {
            if (TurnType == TurnType.Player)
            {
                updatePlayerTurn();
            }
            else
            {
                updateEnemyTurn();
            }
        }


        void updatePlayerTurn()
        {
            if (PlayerTurnPhase == PlayerTurnPhase.Start)
            {

            }
            else if (PlayerTurnPhase == PlayerTurnPhase.ActionSetup)
            {

            }
            else if (PlayerTurnPhase == PlayerTurnPhase.ActionTargetSelection)
            {

            }
            else if (PlayerTurnPhase == PlayerTurnPhase.ActionRun)
            {

            }
            else if (PlayerTurnPhase == PlayerTurnPhase.End)
            {

            }
        }

        void updateEnemyTurn()
        {

        }

        public void runAction(ActionBase action, List<ActionBase> secondaryActions)
        {
            Debug.Log($"Run: {action.Name} secondary action {secondaryActions.Count}");
        }
    }
}



