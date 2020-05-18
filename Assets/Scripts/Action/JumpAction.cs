using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sail.animation;

namespace sail
{
    public class JumpAction : ActionBase
    {
        public static new ActionID ID { get; set; } = ActionID.JUMP;
        public override ActionID id() { return ID; }

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
                if (action.id() == ActionID.ATTACK)
                {
                    StateName = "JumpAttack";
                }
            }
        }

        private void playMotion()
        {
            
            Animator.CrossFade(StateName, 0.2f);
        }

        public override void run()
        {
            if(Animator.GetCurrentAnimatorStateInfo(0).IsName(StateName) == false)
            {
                return;
            }

            // Transitioning
            //yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).IsName(StateName));

            if (StateName == "Jump")
            {
                //yield return jump();
                jump();
            }

            if (StateName == "JumpAttack")
            {
                //yield return jumpAttack();
                jumpAttack();
            }

            if (ActionEnded)
            {
                BattleFSM.Instance.SelectedEnemy.Coord = Target;

                Animator.CrossFade("Idle", 0.2f);
                Animator = null;
            }
        }

        public override List<ActionSelectionModel> selectionModels()
        {
            var selectionModel = new AreaTileSelection();
            selectionModel.Range = 3;
            selectionModel.ShapeType = AreaTileSelection.AreaType.Circle;

            TileSelectionModelBase targetModel = null;

            foreach (var action in SecondaryActions)
            {
                if (action.id() == ActionID.ATTACK)
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
        private void jump()
        {
            var character = BattleFSM.Instance.SelectedEnemy;
            var tile = BattleFSM.Instance.board.getTile(Target);

            // Homing
            var remainingFrames = 60f;
            //bool homingEnded = false;
            //while (homingEnded == false)
            if(Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.33f)
            {
                var targetDir = tile.transform.position - character.transform.position;
                targetDir.y = 0f;

                var angle = Vector3.SignedAngle(character.transform.forward, targetDir, Vector3.up);
                var deltaAngle = angle / remainingFrames;
                character.transform.Rotate(Vector3.up, deltaAngle);

                remainingFrames -= 1f;

                if (remainingFrames <= 1f)
                {
                    //homingEnded = true;
                }

                return;
            }

            //// Jump start frames
            //yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.33f);
            //
            //// Move root
            //yield return ActionFunctionLibrary.moveTo(BattleFSM.Instance.SelectedEnemy.gameObject, Target);
            //
            //// Waiting for the end of the motion
            //yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

            if (Moving == false)
            {
                Tweener = ActionFunctionLibrary.moveTo(BattleFSM.Instance.SelectedEnemy.gameObject, Target);
                Moving = true;
            }

            if (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                ActionEnded = true;
                Moving = false;
            }
        }

        bool Moving = false;
        Tweener Tweener = null;

        private void jumpAttack()
        {
            var character = BattleFSM.Instance.SelectedEnemy;
            var tile = BattleFSM.Instance.board.getTile(Target);

            // Homing
            var remainingFrames = 30f;
            //bool homingEnded = false;
            //while (homingEnded == false)
            if(Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.14f)
            {
                var targetDir = tile.transform.position - character.transform.position;
                targetDir.y = 0f;

                var angle = Vector3.SignedAngle(character.transform.forward, targetDir, Vector3.up);
                var deltaAngle = angle / remainingFrames;
                character.transform.Rotate(Vector3.up, deltaAngle);

                remainingFrames -= 1f;

                if (remainingFrames <= 1f)
                {
                    //homingEnded = true;
                }
                return;

                //yield return null;
            }

            // Jump start frames
            //yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.14f);

            // Move root
            //yield return ActionFunctionLibrary.moveTo(BattleFSM.Instance.SelectedEnemy.gameObject, Target);

            // Waiting for the end of the motion
            //yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

            if (Moving == false)
            {
                Tweener = ActionFunctionLibrary.moveTo(BattleFSM.Instance.SelectedEnemy.gameObject, Target);
                Moving = true;
            }

            if (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                ActionEnded = true;
                Moving = false;
            }
        }
    }

}