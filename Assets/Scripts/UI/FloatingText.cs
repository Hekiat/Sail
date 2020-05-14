using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace sail
{
    public class FloatingText : MonoBehaviour
    {
        public Text Text;
        public RectTransform RectTransform;

        private float Timer;
        private Vector3 StartPosition;
        private float TimerDuration = 1f;

        // Start is called before the first frame update
        void Start()
        {
            Timer = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            Timer += Time.deltaTime;

            updateTextPos();

            if (Timer >= TimerDuration)
            {
                Destroy(gameObject);
            }
        }

        public void setText(string text)
        {
            Text.text = text;
        }

        public void setColor(Color color)
        {
            Text.color = color;
        }

        public void setWorldPos(Vector3 pos)
        {
            StartPosition = pos;
        }

        private void updateTextPos()
        {
            var offset = Timer / TimerDuration * Vector3.up;
            RectTransform.position = Camera.main.WorldToScreenPoint(StartPosition + offset);

            var s = TimerDuration - Timer / TimerDuration;
            RectTransform.localScale = new Vector3(s, s, s);
        }
    }
}