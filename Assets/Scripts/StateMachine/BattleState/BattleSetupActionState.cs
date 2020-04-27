using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class BattleSetupActionState : BattleBaseState
    {
        public override void Enter()
        {
            base.Enter();

            owner.hud.showActionSetupWidgets(true);
        }

        public override void Exit()
        {
            base.Exit();

            owner.hud.showActionSetupWidgets(false);
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            owner.hud.OnActionSetupAccepted += onActionAccepted;
            owner.hud.OnActionSetupSelected += onActionSelected;
        }

        protected override void RemoveListeners()
        {
            base.RemoveListeners();
            owner.hud.OnActionSetupAccepted -= onActionAccepted;
            owner.hud.OnActionSetupAccepted -= onActionSelected;
        }

        void onActionSelected(ActionBase action, List<ActionBase> secondaryActions)
        {
            owner.ActionController.setup(action, secondaryActions);
        }

        void onActionAccepted(ActionBase action, List<ActionBase> secondaryActions)
        {
            if (action.SelectionCount > 0)
            {
                owner.ChangeToState<BattleTileSelectionState>();
                return;
            }

            owner.ChangeToState<BattleRunActionState>();
        }
    }
}