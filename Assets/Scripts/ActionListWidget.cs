using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace sail
{
    public class ActionListWidget : MonoBehaviour
    {
        public GameObject ActionButtonPrefab = null;
        public GameObject ActionListContent = null; // transform.Find("Canvas/ActionList/ScrollList/Viewport/Content").gameObject;

        public delegate void ActionSelectedDelegate(ActionBase action);

        // Declare the event.
        public event ActionSelectedDelegate ActionSelected;

        void Start()
        {
            //for (int i = 0; i < 20; i++)
            foreach (var action in GlobalManagers.actionManager.Actions)
            {
                GameObject prefabInst = Instantiate(ActionButtonPrefab) as GameObject;
                prefabInst.transform.SetParent(ActionListContent.transform, false);
                prefabInst.GetComponent<Button>().onClick.AddListener(() => { showAction(action); });
                prefabInst.GetComponentInChildren<Text>().text = action.Name;
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