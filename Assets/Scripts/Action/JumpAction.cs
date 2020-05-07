using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class JumpAction : ActionBase
    {
        public static new ActionID ID { get; set; } = ActionID.JUMP;

        public override int SelectionCount => 1;

        private Animator Animator = null;
        private TileCoord Target;

        public override void start()
        {
            base.start();

            Animator = BattleFSM.Instance.SelectedEnemy.Animator;
            Animator.CrossFade("Jump", 0.2f);

            Target = BattleFSM.Instance.TileSelectionController.selectedTiles()[0];
        }

        public override IEnumerator run()
        {
            // Transitioning
            yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"));

            // Jump start frames
            yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.33f);

            // Move root
            Debug.Log("PRE");
            yield return ActionFunctionLibrary.moveTo(BattleFSM.Instance.SelectedEnemy.gameObject, Target);
            Debug.Log("POST");
            // Waiting for the end of the motion
            yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

            BattleFSM.Instance.SelectedEnemy.Coord = Target;

            Animator.CrossFade("Idle", 0.2f);
            Animator = null;
        }

        public override List<ActionSelectionModel> selectionModels()
        {
            var selectionModel = new AreaTileSelection();
            selectionModel.Range = 3;
            selectionModel.ShapeType = AreaTileSelection.AreaType.Circle;

            var model = new ActionSelectionModel(selectionModel, null);
            var models = new List<ActionSelectionModel>();
            models.Add(model);

            return models;
        }
    }

}