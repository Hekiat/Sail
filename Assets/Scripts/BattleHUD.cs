using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace sail
{
    public class BattleHUD : MonoBehaviour
    {
        // Prefab list
        public GameObject ActionWidgetPrefab = null;

        // General purpose
        private GameObject RootCanvas = null;

        // inner widgets
        public ActionListWidget ActionListWidget = null;
        private ActionWidget ActionWidget = null;

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

            RootCanvas = transform.GetChild(0).gameObject;

            var actionWidgetGO = Instantiate(ActionWidgetPrefab) as GameObject;

            ActionWidget = actionWidgetGO.GetComponent<ActionWidget>();
            actionWidgetGO.transform.SetParent(RootCanvas.transform, false);
            ActionWidget.GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.6f, 0.6f);

            ActionListWidget.ActionSelected += showAction;

            GlobalManagers.hud = this;
        }

        void Update()
        {

        }

        void showAction(ActionBase action)
        {
            ActionWidget.gameObject.SetActive(true);
            ActionWidget.setAction(action);
        }
    }
}