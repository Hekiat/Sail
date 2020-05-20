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

        private TileCoord Target;
        private MotionState StateName = null;

        public override void start()
        {
            base.start();

            Target = BattleFSM.Instance.TileSelectionController.selectedTiles()[0];

            playMotion();
        }

        public override void setup(List<ActionBase> secondaryActions)
        {
            base.setup(secondaryActions);

            StateName = EmMotionStates.Jump;

            foreach (var action in secondaryActions)
            {
                // Better ID management
                if (action.id() == ActionID.ATTACK)
                {
                    StateName = EmMotionStates.JumpAttack;
                }
            }
        }

        private void playMotion()
        {
            Unit.MotionController.requestMotion(StateName, 0.2f);
        }

        public override void run()
        {
            if (StateName == EmMotionStates.Jump)
            {
                jump();
            }

            if (StateName == EmMotionStates.JumpAttack)
            {
                jumpAttack();
            }

            if (ActionEnded)
            {
                Unit.Coord = Target;
                Unit.MotionController.requestMotion(EmMotionStates.Idle, 0.2f);
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
            var time = Unit.MotionController.currentStateNormalizedTime();
            if(time < 0.33f)
            {
                //var remainingFrames = 60f;
                //var targetDir = tile.transform.position - Unit.transform.position;
                //targetDir.y = 0f;
                //
                //var angle = Vector3.SignedAngle(Unit.transform.forward, targetDir, Vector3.up);
                //var deltaAngle = angle / remainingFrames;
                //Unit.transform.Rotate(Vector3.up, deltaAngle);
                //remainingFrames -= 1f;

                applyHoming(Target);
                return;
            }

            if (Moving == false)
            {
                Tweener = ActionFunctionLibrary.moveTo(Unit.gameObject, Target);
                Moving = true;
            }

            if (Unit.MotionController.currentStateNormalizedTime() >= 1.0f)
            {
                ActionEnded = true;
                Moving = false;
            }
        }

        bool Moving = false;
        Tweener Tweener = null;

        private void jumpAttack()
        {
            var time = Unit.MotionController.currentStateNormalizedTime();
            if(time < 0.14f)
            {
                //var targetDir = tile.transform.position - Unit.transform.position;
                //targetDir.y = 0f;
                //
                //var angle = Vector3.SignedAngle(Unit.transform.forward, targetDir, Vector3.up);
                //var deltaAngle = angle / remainingFrames;
                //Unit.transform.Rotate(Vector3.up, deltaAngle);
                //
                //remainingFrames -= 1f;

                applyHoming(Target);
                return;
            }

            if (Moving == false)
            {
                Tweener = ActionFunctionLibrary.moveTo(Unit.gameObject, Target);
                Moving = true;
            }

            if (time >= 1.0f)
            {
                ActionEnded = true;
                Moving = false;
            }
        }
    }

}