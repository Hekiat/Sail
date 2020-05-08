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

            var character = BattleFSM.Instance.SelectedEnemy;
            var tile = BattleFSM.Instance.board.getTile(Target);

            // Homing
            var remainingFrames = 60f;
            bool homingEnded = false;
            while (homingEnded == false)
            {
                var targetDir = tile.transform.position - character.transform.position;
                targetDir.y = 0f;

                var angle = Vector3.SignedAngle(character.transform.forward, targetDir, Vector3.up);
                var deltaAngle = angle / remainingFrames;
                character.transform.Rotate(Vector3.up, deltaAngle);

                remainingFrames -= 1f;

                if (remainingFrames <= 1f)
                {
                    homingEnded = true;
                }

                yield return null;
            }

            // Jump start frames
            yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.33f);

            // Move root
            yield return ActionFunctionLibrary.moveTo(BattleFSM.Instance.SelectedEnemy.gameObject, Target);

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