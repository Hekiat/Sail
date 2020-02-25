using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace sail
{
    public class ActionWidget : MonoBehaviour
    {
        // Prefab list
        public GameObject PluginWidgetPrefab = null;

        // Game Objects
        public GameObject PluginSlotsGO = null;
        public GameObject ActionNameGO = null;

        // Data
        ActionBase Action = null;

        // Components
        private Text Text = null;

        private void Start()
        {
            Text = ActionNameGO.GetComponent<Text>();

            transform.gameObject.SetActive(false);
        }

        public void setAction(ActionBase action)
        {
            Action = action;

            // Clear old action
            foreach (Transform child in PluginSlotsGO.transform)
            {
                Destroy(child.gameObject);
            }

            // Init Action
            // -> Action Name
            Text.text = Action.Name;

            // -> Action Slots
            for (int i = 0; i < Action.ActionSlots.Count; ++i)
            {
                GameObject prefabInst = Instantiate(PluginWidgetPrefab) as GameObject;
                prefabInst.transform.SetParent(PluginSlotsGO.transform, false);
            }
        }
    }
}