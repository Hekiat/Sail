using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class TimelineCharacterWidget : MonoBehaviour
    {
        public Unit unit = null;

        public float currentTimer = 100f;
        public RectTransform RectTrans = null;

        private void Awake()
        {
            RectTrans = GetComponent<RectTransform>();
        }
    }
}
