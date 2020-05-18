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

        private Animator Animator = null;

        public override void start()
        {
            base.start();

            Animator = BattleFSM.Instance.SelectedEnemy.Animator;
            Animator.CrossFade("Shield", 0.2f);
        }

        public override void run()
        {
            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Shield") == false)
            {
                return;
            }

            if (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                return;
            }

            // Transitioning
            //yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).IsName("Shield"));

            // Waiting for the end of the motion
            //yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

            var shieldable = BattleFSM.Instance.SelectedEnemy as IShieldable;
            shieldable.Shield(6);

            Animator.CrossFade("Idle", 0.2f);
            Animator = null;
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