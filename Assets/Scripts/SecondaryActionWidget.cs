using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace sail
{
    public class SecondaryActionWidget : MonoBehaviour
    {
        ActionBase Action = null;

        // Game Objects
        public GameObject ActionNameGO = null;

        // Components
        private Text Text = null;
        private Image Image = null;

        private void Awake()
        {
            Text = ActionNameGO.GetComponent<Text>();
            Image = GetComponent<Image>();
        }

        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void setAction(ActionBase action, RectTransform rectTrans)
        {
            Action = action;

            Text.text = Action.Name;

            var currentRectTrans = GetComponent<RectTransform>();
            StartCoroutine(startTransition(currentRectTrans, rectTrans));
        }

        IEnumerator startTransition(RectTransform self, RectTransform target)
        {
            self.rotation = target.rotation;
            self.position = target.position + self.transform.up * 100f;

            var initPos = self.position;

            var iteration = 30;
            for (int i = 0; i <= iteration; ++i)
            {
                var t = (float)i / iteration;
                self.position = Vector3.Lerp(initPos, target.position, t);
                yield return null;
            }
        }
    }
}

