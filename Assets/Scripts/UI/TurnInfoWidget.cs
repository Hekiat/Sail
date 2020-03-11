using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace sail
{
    public class TurnInfoWidget : MonoBehaviour
    {
        public Text TurnTypeText = null;
        public Text TurnPhaseText = null;

        private void Start()
        {
            updateWidgets();
        }

        void Update()
        {
            updateWidgets();
        }

        void updateWidgets()
        {
            //if (BattleFSM.Instance == null || BattleFSM.Instance.CurrentState == null)
            //{
            //    return;
            //}

            //Enum.GetName(TurnType.GetType(), TurnType);

            TurnTypeText.text = "Player"; //TurnType.ToString();
            TurnPhaseText.text = BattleFSM.Instance.CurrentState.GetType().Name;

            TurnPhaseText.gameObject.SetActive(true);
        }
    }
}
