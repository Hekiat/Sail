using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sail
{
    public class BattleRunActionState : BattleBaseState
    {
        public ActionBase Action = null;

        public TileCoord Target { get; set; }

        public override void Enter()
        {
            base.Enter();

            owner.TileSelectionController.select(AStarSearch.search(new TileCoord(0, 0), Target));

            BattleFSM.Instance.ActionController.requestAction(GlobalManagers.actionManager.Actions[1], new List<ActionBase>());
        }

        public override void Exit()
        {
            base.Exit();

            owner.TileSelectionController.clear();
        }

        protected override void AddListeners()
        {
            base.AddListeners();

            owner.ActionController.OnActionEnded += onActionEnded;
        }

        protected override void RemoveListeners()
        {
            base.RemoveListeners();

            owner.ActionController.OnActionEnded -= onActionEnded;
        }

        private void onActionEnded(object s, EventArgs e)
        {
            owner.ChangeToState<BattleUnitSelectionState>();
        }
    }
}