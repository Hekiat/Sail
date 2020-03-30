using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace sail
{
    public class TimelineCharacterWidget : MonoBehaviour
    {
        // From inspector
        public Text Text = null;

        // From script
        private Unit _Unit = null;
        public Unit Unit
        {
            get { return _Unit; }
            set { _Unit = value; updateUI(); }
        }

        public float currentTimer = 100f;

        [HideInInspector]
        public RectTransform RectTrans = null;

        private void Awake()
        {
            RectTrans = GetComponent<RectTransform>();
        }

        private void updateUI()
        {
            Debug.Log(Unit.UnitName);
            Text.text = Unit.UnitName.Substring(0, 1);
        }
    }
}
