using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace sail
{
    public class ActionWidget : MonoBehaviour
    {
        // Prefab list
        public GameObject SecondaryActionWidgetPrefab = null;

        // Game Objects

        public GameObject ActionNameGO = null;
        public GameObject ActionSlotBackground = null;

        public GameObject SecondaryActionLeftTargetGO = null;
        public GameObject SecondaryActionCenterTargetGO = null;
        public GameObject SecondaryActionRightTargetGO = null;

        public List<Sprite> SecondaryActionSlotImg = new List<Sprite>();

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

            // Init Action
            // -> Action Name
            Text.text = Action.Name;

            // -> Action Display Background
            var slotCount = Action.ActionSlots.Count;
            if (slotCount < SecondaryActionSlotImg.Count)
            {
                ActionSlotBackground.GetComponent<Image>().sprite = SecondaryActionSlotImg[slotCount];
            }
            else
            {
                Debug.Log("This number of action slot is not supported by UI.");
            }

            // -> Action Slots
            //for (int i = 0; i < Action.ActionSlots.Count; ++i)
            //{
            //    GameObject prefabInst = Instantiate(SecondaryActionWidgetPrefab) as GameObject;
            //    prefabInst.transform.SetParent(transform, false);
            //    //prefabInst.transform.SetParent(PluginSlotsGO.transform, false);
            //}
        }
    }
}