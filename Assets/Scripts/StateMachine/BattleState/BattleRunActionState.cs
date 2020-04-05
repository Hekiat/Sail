using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sail
{
    public class BattleRunActionState : BattleBaseState
    {
        public ActionBase Action = null;

        public override void Enter()
        {
            base.Enter();

            BattleFSM.Instance.ActionController.request();
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