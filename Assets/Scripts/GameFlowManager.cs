using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class GameFlowManager : MonoBehaviour
    {
        void Awake()
        {
            GlobalManagers.gameManager = this;
        }

        void Start()
        {
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



