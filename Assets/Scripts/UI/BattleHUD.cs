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
        public ActionSetupWidget ActionWidget = null;
        public Toggle ShowHideActionTgl = null;
        public TimelineWidget TimelineWidget = null;
        public GameObject MapWidgetGO = null;
        public GameObject WorldMapWidgetGO = null;
        public GameObject MapBackgroundGO = null;
        public Button ToggleMapBtn = null;
        public Button ToggleWorldMapBtn = null;

        public event Action<ActionBase, List<ActionBase>> OnActionSetupSelected;
        public event Action<ActionBase, List<ActionBase>> OnActionSetupAccepted;

        private void Awake()
        {

        }

        void Start()
        {
            RootCanvas = transform.GetChild(0).gameObject;

            ActionListWidget.ActionSelected += onActionSelected;
            ActionWidget.OnActionSelected += onActionSelected;
            ActionWidget.OnActionAccepted += onActionAccepted;
            ActionWidget.OnActionCanceled += onActionCancel;

            showActionSetupWidgets(false);
            MapWidgetGO.SetActive(false);
            WorldMapWidgetGO.SetActive(false);
            MapBackgroundGO.SetActive(false);

            ToggleMapBtn.onClick.AddListener(() => MapWidgetGO.SetActive(!MapWidgetGO.activeSelf));
            ToggleWorldMapBtn.onClick.AddListener(() => WorldMapWidgetGO.SetActive(!WorldMapWidgetGO.activeSelf));

            GlobalManagers.hud = this;
        }

        void Update()
        {
            MapBackgroundGO.SetActive(MapWidgetGO.activeSelf | WorldMapWidgetGO.activeSelf);
        }

        void onActionSelected(ActionBase action, List<ActionBase> secondaryActions)
        {
            OnActionSetupSelected?.Invoke(action, secondaryActions);
        }

        void onActionAccepted(ActionBase action, List<ActionBase> secondaryActions)
        {
            Debug.Log($"Run: {action.Name} secondary action {secondaryActions.Count}");
            OnActionSetupAccepted?.Invoke(action, secondaryActions);
        }

        void onActionCancel()
        {
            OnActionSetupSelected?.Invoke(null, new List<ActionBase>());
        }

        void onActionSelected(ActionBase action)
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