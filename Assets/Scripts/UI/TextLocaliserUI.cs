using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace sail
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextLocaliserUI : MonoBehaviour
    {
        TextMeshProUGUI TextField = null;

        public string key = string.Empty;

        // Start is called before the first frame update
        void Start()
        {
            TextField = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            var value = LocalizationSystem.getLocalisedValue(key);
            TextField.text = value;
        }
    }
}