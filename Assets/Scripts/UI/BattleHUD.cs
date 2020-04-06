using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace sail
{
    public class BattleHUD : MonoBehaviour
    {
        // Prefab list
        //public GameObject ActionWidgetPrefab = null;

        // General purpose
        private GameObject RootCanvas = null;

        // inner widgets
        public ActionListWidget ActionListWidget = null;
        public ActionWidget ActionWidget = null;
        public Toggle ShowHideActionTgl = null;
        public TimelineWidget TimelineWidget = null;

        public event ActionWidget.ActionAcceptedDelegate OnActionSetupAccepted;

        private void Awake()
        {

        }

        void Start()
        {
            RootCanvas = transform.GetChild(0).gameObject;

            ActionListWidget.ActionSelected += showAction;
            ActionWidget.OnActionAccepted += onActionAccepted;
            ActionWidget.OnActionCanceled += onActionCancel;

            showActionSetupWidgets(false);

            GlobalManagers.hud = this;
        }

        void Update()
        {
        }

        void onActionAccepted(ActionBase action, List<ActionBase> secondaryActions)
        {
            Debug.Log($"Run: {action.Name} secondary action {secondaryActions.Count}");
            OnActionSetupAccepted?.Invoke(action, secondaryActions);
        }

        void onActionCancel()
        {
            //ShowHideActionTgl.isOn = false;
            //ShowHideActionTgl.gameObject.SetActive(false);
        }

        void showAction(ActionBase action)
        {
            ActionWidget.setAction(action);

            showActionSetupWidgets(true);
            ActionWidget.gameObject.SetActive(true);
        }

        public void showActionSetupWidgets(bool show)
        {
            ShowHideActionTgl.isOn = show;

            ActionWidget.gameObject.SetActive(false);
            ActionListWidget.gameObject.SetActive(show);
            ShowHideActionTgl.gameObject.SetActive(show);
        }
    }
}