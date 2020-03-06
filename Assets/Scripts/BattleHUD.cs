using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        private void Awake()
        {

        }

        void Start()
        {
            // Remove all children from content widget
            //for (int i=0; i< actionListContent.transform.childCount; ++i)
            //{
            //    var child = actionListContent.transform.GetChild(i);
            //    Destroy(child.gameObject);
            //}

            //var actionWidgetGO = Instantiate(ActionWidgetPrefab) as GameObject;
            //
            //ActionWidget = actionWidgetGO.GetComponent<ActionWidget>();
            //actionWidgetGO.transform.SetParent(RootCanvas.transform, false);
            //var rectTrans = ActionWidget.GetComponent<RectTransform>();
            //rectTrans.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            //rectTrans.anchoredPosition -= Vector2.up * 60f;

            RootCanvas = transform.GetChild(0).gameObject;

            ActionListWidget.ActionSelected += showAction;
            ActionWidget.OnActionAccepted += onActionAccepted;
            ActionWidget.OnActionCanceled += onActionCancel;

            showActionSetupWidgets(false);

            GlobalManagers.gameManager.TurnChanged += onTurnChanged;
            GlobalManagers.gameManager.PhaseChanged += onPhaseChanged;

            GlobalManagers.hud = this;
        }

        void Update()
        {
        }

        void onTurnChanged(TurnType type)
        {

        }

        void onPhaseChanged(PlayerTurnPhase phase)
        {
            if (phase == PlayerTurnPhase.ActionSetup)
            {
                showActionSetupWidgets(true);
            }
            else
            {
                showActionSetupWidgets(false);
            }
        }


        void onActionAccepted(ActionBase action, List<ActionBase> secondaryActions)
        {
            GlobalManagers.gameManager.runAction(action, secondaryActions);
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
        }

        void showActionSetupWidgets(bool show)
        {
            ShowHideActionTgl.isOn = show;

            ActionWidget.gameObject.SetActive(show);
            ActionListWidget.gameObject.SetActive(show);
            ShowHideActionTgl.gameObject.SetActive(show);
        }
    }
}