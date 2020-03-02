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
        int SlotCount = 0;

        // Components
        private Text Text = null;
        private List<GameObject> SecondaryActionWidgets = new List<GameObject>();

        private void Start()
        {
            Text = ActionNameGO.GetComponent<Text>();

            transform.gameObject.SetActive(false);
        }

        public void setAction(ActionBase action)
        {
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

            // -> Action Slots
            foreach (var aw in SecondaryActionWidgets)
            {
                Destroy(aw);
            }

            SecondaryActionWidgets.Clear();

            for (int i = 0; i < SlotCount; i++)
            {
                setSecondaryAction(i);
            }

            //for (int i = 0; i < Action.ActionSlots.Count; ++i)
            //{
            //    GameObject prefabInst = Instantiate(SecondaryActionWidgetPrefab) as GameObject;
            //    prefabInst.transform.SetParent(transform, false);
            //    //prefabInst.transform.SetParent(PluginSlotsGO.transform, false);
            //}
        }

        private void setSecondaryAction(int slotID)
        {
            GameObject prefabInst = Instantiate(SecondaryActionWidgetPrefab) as GameObject;
            prefabInst.transform.SetParent(transform, false);

            var rectTrans = prefabInst.GetComponent<RectTransform>();
            var targetGO = slotTarget(slotID);
            var target = targetGO.GetComponent<RectTransform>();

            StartCoroutine(SecondaryActionTransition(rectTrans, target));

            SecondaryActionWidgets.Add(prefabInst);
        }

        IEnumerator SecondaryActionTransition(RectTransform self, RectTransform target)
        {
            self.rotation = target.rotation;
            self.position = target.position + self.transform.up * 100f;

            var initPos = self.position;

            var iteration = 30;
            for (int i=0; i<=iteration; ++i)
            {
                var t = (float)i / iteration;
                self.position = Vector3.Lerp(initPos, target.position, t);
                yield return null;
            }
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
    }
}