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
        }

        public override void Exit()
        {
            base.Exit();
        }

        protected override void AddListeners()
        {
            InputController.ClickEvent += OnMouseClick;
        }

        protected override void RemoveListeners()
        {
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

            if (tile.Coord.Square.x == 0 && tile.Coord.Square.y == 0)
            {
                board.clearTilesSelection();
                return;
            }

            board.setTilesSelected(AStarSearch.search(new TileCoord(0, 0), tile.Coord));

            BattleFSM.Instance.ActionController.requestAction(GlobalManagers.actionManager.Actions[1], new List<ActionBase>());

            owner.ChangeToState<BattleUnitSelectionState>();
        }
    }
}
