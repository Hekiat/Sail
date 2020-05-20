using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class AttackAction : ActionBase
    {
        public static new ActionID ID { get; set; } = ActionID.ATTACK;

        public override ActionID id() { return ID; }

        public override int SelectionCount => 1;

        private TileCoord Target;

        public override void start()
        {
            base.start();

            Unit.MotionController.requestMotion(EmMotionStates.BasicAttack, 0.2f);
            //Animator.CrossFade("BasicAttack", 0.2f);

            Target = BattleFSM.Instance.TileSelectionController.selectedTiles()[0];
        }

        public override void run()
        {
            var mc = Unit.MotionController;

            if (mc.CurrentState != EmMotionStates.BasicAttack)
            {
                ActionEnded = true;
                return;
            }

            var tile = BattleFSM.Instance.board.getTile(Target);

            // Homing
            var targetDir = tile.transform.position - Unit.transform.position;
            targetDir.y = 0f;

            const float maxRotationAngle = 2f;
            var angle = Vector3.SignedAngle(Unit.transform.forward, targetDir, Vector3.up);
            var deltaAngle = angle < 0f ? Mathf.Max(angle, -maxRotationAngle) : Mathf.Min(angle, maxRotationAngle);
            Unit.transform.Rotate(Vector3.up, deltaAngle);

            if (Mathf.Abs(angle) > 2f)
            {
                return;
            }

            if (mc.currentStateNormalizedTime() >= 1.0f)
            {
                // Target
                var targetEM = BattleFSM.Instance.enemies.Find(x => x.Coord == Target);
                if (targetEM != null)
                {
                    var damageInterface = targetEM as IDamageable;
                    damageInterface.Damage(10);
                }

                mc.requestMotion(EmMotionStates.Idle, 0.2f);
                ActionEnded = true;
            }
        }

        public override List<ActionSelectionModel> selectionModels()
        {
            var selectionModel = new AreaTileSelection();
            selectionModel.Range = 1;
            selectionModel.ShapeType = AreaTileSelection.AreaType.Cross;

            var model = new ActionSelectionModel(selectionModel, null);
            var models = new List<ActionSelectionModel>();
            models.Add(model);

            return models;
        }
    }
}