﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace sail
{
    public class ActionListWidget : MonoBehaviour
    {
        public GameObject ActionButtonPrefab = null;
        public GameObject ActionListContent = null; // transform.Find("Canvas/ActionList/ScrollList/Viewport/Content").gameObject;

        // Declare the event.
        public event Action<ActionBase> ActionSelected = null;

        void Start()
        {
            // PROTO / test instanciation
            foreach (var action in GlobalManagers.actionManager.Actions)
            {
                GameObject prefabInst = Instantiate(ActionButtonPrefab) as GameObject;
                prefabInst.transform.SetParent(ActionListContent.transform, false);

                var actionWidget = prefabInst.GetComponent<ActionWidget>();
                actionWidget.ActionSelected += showAction;
                actionWidget.setup(action);
            }
        }


        void Update()
        {

        }


        void showAction(ActionBase action)
        {
            ActionSelected?.Invoke(action);
        }
    }
}