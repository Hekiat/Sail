using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class ShieldAction : ActionBase
    {
        public static new ActionID ID { get; set; } = ActionID.SHIELD;
        public override ActionID id() { return ID; }

        public override int SelectionCount => 1;

        public override void start()
        {
            base.start();

            Unit.MotionController.requestMotion(EmMotionStates.Shield, 0.2f);
        }

        public override void run()
        {
            if (Unit.MotionController.currentStateNormalizedTime() < 1.0f)
            {
                return;
            }

            var shieldable = BattleFSM.Instance.SelectedEnemy as IShieldable;
            shieldable.Shield(6);

            Unit.MotionController.requestMotion(EmMotionStates.Idle, 0.2f);
            ActionEnded = true;
        }

        public override List<ActionSelectionModel> selectionModels()
        {
            var selectionModel = new SelfTileSelection();

            var model = new ActionSelectionModel(selectionModel, null);
            var models = new List<ActionSelectionModel>();
            models.Add(model);

            return models;
        }
    }

}