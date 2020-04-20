using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class ShieldAction : ActionBase
    {
        public static new ActionID ID { get; set; } = ActionID.SHIELD;

        public override int SelectionCount => 1;

        private Animator Animator = null;

        public override void start()
        {
            base.start();

            Animator = BattleFSM.Instance.SelectedEnemy.Animator;
            Animator.CrossFade("Shield", 0.5f);
        }

        public override IEnumerator run()
        {
            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Shield") && Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }

            Animator.CrossFade("Idle", 0.5f);
            Animator = null;
        }

        public override List<ActionSelectionModel> selectionModels()
        {
            var selectionModel = new SelfTileSelection();

            var model = new ActionSelectionModel(selectionModel, new SelfTileSelection());
            var models = new List<ActionSelectionModel>();
            models.Add(model);

            return models;
        }
    }

}