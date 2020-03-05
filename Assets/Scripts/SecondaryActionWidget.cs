using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace sail
{
    public class SecondaryActionWidget : MonoBehaviour
    {
        public ActionBase Action { get; set; } = null;

        // Components
        public Text ActionNameTxt = null;
        //private Image Image = null;

        private void Awake()
        {
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

            ActionNameTxt.text = Action.Name;

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

