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

        public Button CancelBtn = null;
        public Button AcceptBtn = null;

        public List<Sprite> SecondaryActionSlotImg = new List<Sprite>();

        // Data
        ActionBase Action = null;
        int SlotCount = 0;

        // Components
        private Text Text = null;
        private List<SecondaryActionWidget> SecondaryActionWidgets = new List<SecondaryActionWidget>();

        private void Start()
        {
            Text = ActionNameGO.GetComponent<Text>();

            transform.gameObject.SetActive(false);

            CancelBtn.onClick.AddListener(clear);
            AcceptBtn.onClick.AddListener(accept);
        }

        public void setAction(ActionBase action)
        {
            if (Action != null)
            {
                if (SecondaryActionWidgets.Count < SlotCount)
                {
                    setSecondaryAction(action, SecondaryActionWidgets.Count);
                }

                return;
            }

            // Action
            Action = action;
            SlotCount = Action.ActionSlots.Count;

            // Init Action
            // -> Action Name
            Text.text = Action.Name;

            // -> Action Display Background
            if (SlotCount < SecondaryActionSlotImg.Count)
            {
                ActionSlotBackground.GetComponent<Image>().sprite = SecondaryActionSlotImg[SlotCount];
            }
            else
            {
                Debug.Log("This number of action slot is not supported by UI.");
            }

            //for (int i = 0; i < Action.ActionSlots.Count; ++i)
            //{
            //    GameObject prefabInst = Instantiate(SecondaryActionWidgetPrefab) as GameObject;
            //    prefabInst.transform.SetParent(transform, false);
            //    //prefabInst.transform.SetParent(PluginSlotsGO.transform, false);
            //}
        }

        private void setSecondaryAction(ActionBase action, int slotID)
        {
            var targetGO = slotTarget(slotID);

            GameObject prefabInst = Instantiate(SecondaryActionWidgetPrefab) as GameObject;
            prefabInst.transform.SetParent(transform, false);
            
            var targetTransform = targetGO.GetComponent<RectTransform>();
            var saw = prefabInst.GetComponent<SecondaryActionWidget>();
            saw.setAction(action, targetTransform);

            SecondaryActionWidgets.Add(saw);
        }

        private GameObject slotTarget(int slotID)
        {
            if (SlotCount == 0)
            {
                return null;
            }
            else if (SlotCount == 1)
            {
                return SecondaryActionCenterTargetGO;
            }
            else if (SlotCount == 2)
            {
                if (slotID == 0)
                {
                    return SecondaryActionLeftTargetGO;
                }
                else
                {
                    return SecondaryActionRightTargetGO;
                }
            }
            else if (SlotCount == 3)
            {
                if (slotID == 0)
                {
                    return SecondaryActionLeftTargetGO;
                }
                else if(slotID == 1)
                {
                    return SecondaryActionCenterTargetGO;
                }
                else
                {
                    return SecondaryActionRightTargetGO;
                }
            }

            return null;
        }

        void clear()
        {
            transform.gameObject.SetActive(false);

            Action = null;

            foreach (var aw in SecondaryActionWidgets)
            {
                Destroy(aw.gameObject);
            }
            SecondaryActionWidgets.Clear();
        }

        void accept()
        {
            clear();
        }
    }
}