using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace sail
{
    public class ActionSetupWidget : MonoBehaviour
    {
        // Prefab list
        public GameObject SecondaryActionWidgetPrefab = null;

        // Game Objects
        public Text ActionNameText = null;
        public Image ActionSlotBackgroundImg = null;

        public GameObject SecondaryActionLeftTargetGO = null;
        public GameObject SecondaryActionCenterTargetGO = null;
        public GameObject SecondaryActionRightTargetGO = null;

        public Button AcceptBtn = null;
        public Button CancelBtn = null;

        public List<Sprite> SecondaryActionSlotImg = new List<Sprite>();

        // Data
        ActionBase Action = null;
        int SlotCount = 0;
        private List<SecondaryActionWidget> SecondaryActionWidgets = new List<SecondaryActionWidget>();

        // Events
        public event Action<ActionBase, List<ActionBase>> OnActionSelected = null;
        public event Action<ActionBase, List<ActionBase>> OnActionAccepted = null;
        public event Action OnActionCanceled = null;

        private void Start()
        {
            CancelBtn.onClick.AddListener(onCancel);
            AcceptBtn.onClick.AddListener(onAccept);
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
            SlotCount = Action.MaxSecondaryActionSlots;

            // Init Action
            // -> Action Name
            ActionNameText.text = Action.Name;

            // -> Action Display Background
            if (SlotCount < SecondaryActionSlotImg.Count)
            {
                ActionSlotBackgroundImg.sprite = SecondaryActionSlotImg[SlotCount];
            }
            else
            {
                Debug.Log("This number of action slot is not supported by UI.");
            }

            OnActionSelected(Action, new List<ActionBase>());
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

            List<ActionBase> secondaryActions = new List<ActionBase>();
            foreach (var s in SecondaryActionWidgets)
            {
                secondaryActions.Add(s.Action);
            }
            OnActionSelected(Action, secondaryActions);
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

        void onCancel()
        {
            OnActionCanceled();

            clear();
        }

        void onAccept()
        {
            List<ActionBase> secondaryActions = new List<ActionBase>();
            foreach (var saw in SecondaryActionWidgets)
            {
                secondaryActions.Add(saw.Action);
            }

            OnActionAccepted(Action, secondaryActions);

            clear();
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
    }
}