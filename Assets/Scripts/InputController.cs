using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace sail
{
    public class InputController : MonoBehaviour
    {
        public static event EventHandler<CustomEventArgs<Vector3>> ClickEvent = delegate {};

        void Start()
        {
        
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(sail.MouseButton.Left))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                ClickEvent(this, new CustomEventArgs<Vector3>(Input.mousePosition));
            }
        }
    }
}