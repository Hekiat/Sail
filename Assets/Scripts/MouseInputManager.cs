using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace sail
{
    public class TileSelectionController : MonoBehaviour
    {
        Camera CurrentCamera = null;

        public GameObject HoveredGameObject { get; private set; } = null;

        // Start is called before the first frame update
        void Start()
        {
            CurrentCamera = Camera.main;
        }

        void OnDestroy()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        Tile getUnderMouseTile()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return null;
            }


            Tile tile = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            //Debug.DrawRay(Camera.main.transform.position, ray.direction, Color.magenta);

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, sail.LayerMask.Terrain))
            {
                var tileGO = hitInfo.collider.transform.gameObject;

                return tileGO.GetComponent<Tile>();

                //if (Input.GetMouseButtonDown(sail.MouseButton.Left))
                //{
                //    GlobalManagers.board.TileClicked(HoveredGameObject);
                //}
            }
            else
            {
                HoveredGameObject = null;
            }

            return tile;
        }
    }
}