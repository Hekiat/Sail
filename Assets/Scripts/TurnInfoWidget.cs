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

        TurnType TurnType;
        PlayerTurnPhase PlayerTurnPhase;

        private void Start()
        {
            

            updateWidgets();
        }

        void Update()
        {
            if (   GlobalManagers.gameManager.TurnType != TurnType
                || GlobalManagers.gameManager.PlayerTurnPhase != PlayerTurnPhase)
            {
                updateWidgets();
            }
        }

        void updateWidgets()
        {
            TurnType = GlobalManagers.gameManager.TurnType;
            PlayerTurnPhase = GlobalManagers.gameManager.PlayerTurnPhase;

            //Enum.GetName(TurnType.GetType(), TurnType);

            TurnTypeText.text = TurnType.ToString();
            TurnPhaseText.text = PlayerTurnPhase.ToString();

            TurnPhaseText.gameObject.SetActive(TurnType == TurnType.Player);
        }
    }
}
