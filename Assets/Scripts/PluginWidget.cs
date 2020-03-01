using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace sail
{
    public class PluginWidget : MonoBehaviour
    {
        ActionBase Action = null;

        // Game Objects
        public GameObject ActionNameGO = null;

        // Components
        private Text Text = null;
        private Image Image = null;

        void Start()
        {
            Text = ActionNameGO.GetComponent<Text>();
            Image = GetComponent<Image>();
            clear();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void setAction(ActionBase action)
        {
            Action = action;

            if (Action == null)
            {
                clear();
                return;
            }

            Text.text = Action.Name;
        }

        public void clear()
        {
            //Text.text = string.Empty;
            //Image.color = Color.black;
            //Text.transform.gameObject.SetActive(false);
        }
    }
}

