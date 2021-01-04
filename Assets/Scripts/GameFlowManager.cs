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
    }
}



