using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace sail
{
    public class ActionWidget : MonoBehaviour
    {
        public ActionBase Action { get; private set; } = null;

        public TextMeshProUGUI ActionNameText = null;
        public TextMeshProUGUI CostText = null;
        
        public event Action<ActionBase> ActionSelected = null;

        public void setup(ActionBase action)
        {
            ActionNameText.text = action.Name;
            CostText.text = action.Cost.ToString();
            GetComponent<Button>().onClick.AddListener(() => { ActionSelected(action); });
        }

        void Start()
        {

        }

        void Update()
        {

        }
    }

}