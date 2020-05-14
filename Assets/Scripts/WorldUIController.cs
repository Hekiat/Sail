using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class WorldUIController : MonoBehaviour
    {
        public GameObject FloatingTextPrefab;

        private static WorldUIController Instance = null;

        void Awake()
        {
            Instance = this;
        }

        public static void addFloatingText(string text, Transform location, Color color)
        {
            var ft = Instantiate(Instance.FloatingTextPrefab, Instance.transform).GetComponent<FloatingText>();
            //text.transform.SetParent(canvas.transform, false);
            //ft.transform.position = screenPosition;
            ft.setText(text);
            ft.setColor(color);
            ft.setWorldPos(location.position + Vector3.up * 2);
        }
    }
}