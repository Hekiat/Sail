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

        void Start()
        {
        
        }

        private void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                owner.ChangeToState<BattleUnitSelectionState>();
            }
        }
    }
}