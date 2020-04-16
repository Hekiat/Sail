﻿using System.Collections;
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
            InputController.ClickEvent += OnMouseClick;
        }

        protected override void RemoveListeners()
        {
            base.RemoveListeners();
            InputController.ClickEvent -= OnMouseClick;
        }

        void OnMouseClick(object sender, CustomEventArgs<Vector3> e)
        {
            Tile tile = null;

            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Debug.DrawRay(Camera.main.transform.position, ray.direction, Color.magenta);

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, sail.LayerMask.Terrain))
            {
                var tileGO = hitInfo.collider.transform.gameObject;
                tile = tileGO.GetComponent<Tile>();
            }
            else
            {
                return;
            }

            //if (tile.Coord.Square.x == 0 && tile.Coord.Square.y == 0)
            //{
            //    board.clearTilesSelection();
            //    return;
            //}

            board.clearTilesSelection();

            var path = AStarSearch.search(owner.SelectedEnemy.Coord, tile.Coord);
            owner.TileSelectionController.highlight(path);
            owner.TileSelectionController.select(path[path.Count-1]);

            owner.ChangeToState<BattleRunActionState>();
        }
    }
}
