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
        private string StateName = string.Empty;

        public override void start()
        {
            base.start();

            Animator = BattleFSM.Instance.SelectedEnemy.Animator;
            Target = BattleFSM.Instance.TileSelectionController.selectedTiles()[0];

            playMotion();
        }

        public override void setup(List<ActionBase> secondaryActions)
        {
            base.setup(secondaryActions);

            StateName = "Jump";

            foreach (var action in secondaryActions)
            {
                // Better ID management
                if (action.Name == "Attack")
                {
                    StateName = "JumpAttack";
                }
            }
        }

        private void playMotion()
        {
            
            Animator.CrossFade(StateName, 0.2f);
        }

        public override IEnumerator run()
        {
            // Transitioning
            yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).IsName(StateName));

            if (StateName == "Jump")
            {
                yield return jump();
            }

            if (StateName == "JumpAttack")
            {
                yield return jumpAttack();
            }

            BattleFSM.Instance.SelectedEnemy.Coord = Target;

            Animator.CrossFade("Idle", 0.2f);
            Animator = null;
        }

        public override List<ActionSelectionModel> selectionModels()
        {
            var selectionModel = new AreaTileSelection();
            selectionModel.Range = 3;
            selectionModel.ShapeType = AreaTileSelection.AreaType.Circle;

            TileSelectionModelBase targetModel = null;

            foreach (var action in SecondaryActions)
            {
                // TODO Better ID management
                if (action.Name == "Attack")
                {
                    var m = new AreaTileSelection();
                    m.Range = 1;
                    m.ShapeType = AreaTileSelection.AreaType.Cross;
                    targetModel = m;
                }
            }

            var model = new ActionSelectionModel(selectionModel, targetModel);
            var models = new List<ActionSelectionModel>();
            models.Add(model);

            return models;
        }

        // Actions
        private IEnumerator jump()
        {
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
        }

        private IEnumerator jumpAttack()
        {
            var character = BattleFSM.Instance.SelectedEnemy;
            var tile = BattleFSM.Instance.board.getTile(Target);

            // Homing
            var remainingFrames = 30f;
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
            yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.14f);

            // Move root
            yield return ActionFunctionLibrary.moveTo(BattleFSM.Instance.SelectedEnemy.gameObject, Target);

            // Waiting for the end of the motion
            yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        }
    }

}