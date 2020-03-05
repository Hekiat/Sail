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

        }

        public void runAction(ActionBase action, List<ActionBase> secondaryActions)
        {
            Debug.Log($"Run: {action.Name} secondary action {secondaryActions.Count}");
        }
    }
}



