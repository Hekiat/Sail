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

        private Animator Animator = null;
        private TileCoord Target;

        public override void start()
        {
            base.start();

            Animator = BattleFSM.Instance.SelectedEnemy.Animator;

            BattleFSM.Instance.SelectedEnemy.MotionController.requestMotion(EmMotionStates.BasicAttack, 0.2f);
            //Animator.CrossFade("BasicAttack", 0.2f);

            Target = BattleFSM.Instance.TileSelectionController.selectedTiles()[0];
        }

        public override void run()
        {
            var mc = BattleFSM.Instance.SelectedEnemy.MotionController;

            if (mc.CurrentState != EmMotionStates.BasicAttack)
            {
                ActionEnded = true;
                return;
            }

            var character = BattleFSM.Instance.SelectedEnemy;
            var tile = BattleFSM.Instance.board.getTile(Target);

            // Homing
            var targetDir = tile.transform.position - character.transform.position;
            targetDir.y = 0f;

            const float maxRotationAngle = 2f;
            var angle = Vector3.SignedAngle(character.transform.forward, targetDir, Vector3.up);
            var deltaAngle = angle < 0f ? Mathf.Max(angle, -maxRotationAngle) : Mathf.Min(angle, maxRotationAngle);
            character.transform.Rotate(Vector3.up, deltaAngle);

            if (Mathf.Abs(angle) > 2f)
            {
                return;
            }

            float currentTime = mc.currentStateNormalizedTime();

            if (currentTime >= 1.0f)
            {
                // Target
                var targetEM = BattleFSM.Instance.enemies.Find(x => x.Coord == Target);
                if (targetEM != null)
                {
                    var damageInterface = targetEM as IDamageable;
                    damageInterface.Damage(10);
                }

                mc.requestMotion(EmMotionStates.Idle, 0.2f);

                Animator = null;
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