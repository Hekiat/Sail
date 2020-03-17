using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class BattleIdleState : BattleBaseState
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
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Debug.DrawRay(Camera.main.transform.position, ray.direction, Color.magenta);

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, sail.LayerMask.Terrain) == false)
            {
                return;
            }

            var coord = hitInfo.collider.gameObject.GetComponent<Tile>().Coord;
            //get em from coord
            //var em = hitInfo.collider.gameObject.GetComponent<EnemyCore>();

            //BattleFSM.Instance.SelectedEnemy = em;

            owner.ChangeToState<BattleSetupActionState>();
        }
    }
}

