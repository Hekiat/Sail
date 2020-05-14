using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class RepairAction : ActionBase
    {
        public static new ActionID ID { get; set; } = ActionID.REPAIR;
        public override ActionID id() { return ID; }

        public override int SelectionCount => 1;

        private Animator Animator = null;

        public override void start()
        {
            base.start();

            Animator = BattleFSM.Instance.SelectedEnemy.Animator;
            Animator.CrossFade("Shield", 0.2f);
        }

        public override IEnumerator run()
        {
            // Transitioning
            yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).IsName("Shield"));

            // Waiting for the end of the motion
            yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

            var targetEM = BattleFSM.Instance.SelectedEnemy;
            var damageInterface = targetEM as IDamageable;
            damageInterface.Heal(3);

            Animator.CrossFade("Idle", 0.2f);
            Animator = null;
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