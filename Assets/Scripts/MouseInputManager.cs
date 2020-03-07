using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace sail
{
    public class MouseInputManager : MonoBehaviour
    {
        Camera CurrentCamera = null;

        public GameObject HoveredGameObject { get; private set; } = null;

        // Start is called before the first frame update
        void Start()
        {
            CurrentCamera = Camera.main;

            GlobalManagers.mouseInputManager = this;
        }

        void OnDestroy()
        {
            GlobalManagers.mouseInputManager = null;
        }

        // Update is called once per frame
        void Update()
        {
            updateHoveredObject();
        }

        void updateHoveredObject()
        {

            //var eventData = new PointerEventData(EventSystem.current);
            //eventData.position = Input.mousePosition;
            //var results = new List<RaycastResult>();
            //EventSystem.current.RaycastAll(eventData, results);
            //
            //foreach (var result in results)
            //{
            //    Debug.Log("Over: " + result.gameObject.name);
            //}

            // If mouse is over a UI component dont do anything
            //if (EventSystem.current.currentSelectedGameObject != null)
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            Debug.DrawRay(Camera.main.transform.position, ray.direction, Color.magenta);

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, sail.LayerMask.Terrain))
            {
                HoveredGameObject = hitInfo.collider.transform.gameObject;

                if (Input.GetMouseButtonDown(sail.MouseButton.Left))
                {
                    GlobalManagers.boardManager.TileClicked(HoveredGameObject);
                }
            }
            else
            {
                HoveredGameObject = null;
            }
        }
    }
}