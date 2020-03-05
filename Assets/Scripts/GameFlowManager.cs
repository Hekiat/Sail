using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class GameFlowManager : MonoBehaviour
    {
        public GameObject HUDPrefab = null;

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



