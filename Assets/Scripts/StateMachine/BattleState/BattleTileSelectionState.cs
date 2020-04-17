using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace sail
{
    public class BattleTileSelectionState : BattleBaseState
    {
        public override void Enter()
        {
            base.Enter();

            owner.TileSelectionController.enable();
        }

        public override void Exit()
        {
            base.Exit();
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            InputController.MoveEvent += OnMouseMove;
            InputController.ClickEvent += OnMouseClick;
        }

        protected override void RemoveListeners()
        {
            base.RemoveListeners();
            InputController.MoveEvent -= OnMouseMove;
            InputController.ClickEvent -= OnMouseClick;
        }

        void OnMouseMove(object sender, CustomEventArgs<Vector3> e)
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, sail.LayerMask.Terrain))
            {
                var tileGO = hitInfo.collider.transform.gameObject;
                var tile = tileGO.GetComponent<Tile>();
                owner.TileSelectionController.HoveredTile = tile.Coord;
            }
            else
            {
                owner.TileSelectionController.HoveredTile = null;
            }
        }

        void OnMouseClick(object sender, CustomEventArgs<Vector3> e)
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Debug.DrawRay(Camera.main.transform.position, ray.direction, Color.magenta);

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, sail.LayerMask.Terrain))
            {
                var tileGO = hitInfo.collider.transform.gameObject;
                Tile tile = tileGO.GetComponent<Tile>();
                owner.TileSelectionController.select(tile.Coord);

                owner.ChangeToState<BattleRunActionState>();
            }
            //else
            //{
            //    return;
            //}

            //var path = AStarSearch.search(owner.SelectedEnemy.Coord, tile.Coord);
            //owner.TileSelectionController.highlight(path);

        }
    }
}
