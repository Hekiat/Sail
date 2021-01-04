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

        [SerializeField]
        private string _key = string.Empty;
        public string key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = key;
                applyToUI();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            TextField = GetComponent<TextMeshProUGUI>();
            applyToUI();
        }

        void applyToUI()
        {
            TextField.text = LocalizationSystem.getLocalisedValue(key);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}