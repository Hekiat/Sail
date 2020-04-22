using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class RangedAttackAction : ActionBase
    {
        public static new ActionID ID { get; set; } = ActionID.RANGED_ATTACK;

        public override int SelectionCount => 1;

        private Animator Animator = null;
        private TileCoord Target;

        private GameObject ProjectilePrefab = null;

        public override void configure(ActionBaseConfiguration config)
        {
            base.configure(config);

            var selfConfig = config as RangedAttackConfiguration;
            ProjectilePrefab = selfConfig.ProjectilePrefab;
        }

        public override void start()
        {
            base.start();

            Animator = BattleFSM.Instance.SelectedEnemy.Animator;
            Animator.CrossFade("RangedAttack", 0.2f);

            Target = BattleFSM.Instance.TileSelectionController.selectedTiles()[0];
        }

        public override IEnumerator run()
        {
            if (Animator.HasState(0, Animator.StringToHash("RangedAttack")) == false)
            {
                yield break;
            }

            var character = BattleFSM.Instance.SelectedEnemy;
            var targetPos = character.transform.position;
            var tile = BattleFSM.Instance.board.getTile(Target);

            // Homing
            bool homingEnded = false;
            while (homingEnded == false)
            {
                var targetDir = tile.transform.position - character.transform.position;
                targetDir.y = 0f;

                const float maxRotationAngle = 2f;
                var angle = Vector3.SignedAngle(character.transform.forward, targetDir, Vector3.up);
                var deltaAngle = angle < 0f ? Mathf.Max(angle, -maxRotationAngle) : Mathf.Min(angle, maxRotationAngle);
                character.transform.Rotate(Vector3.up, deltaAngle);

                if (Mathf.Abs(angle) < 2f)
                {
                    homingEnded = true;
                }

                yield return null;
            }

            // Transitioning
            yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).IsName("RangedAttack"));

            // Waiting for the end of the motion
            yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f);

            var characterPos = character.transform.position;

            var projectileStartPos = characterPos + Vector3.up * 1.3f + character.transform.forward;
            var tileGO = GameObject.Instantiate(ProjectilePrefab, projectileStartPos, Quaternion.identity, character.transform);

            // WAIT END PROJECTILE

            // Waiting for the end of the motion
            yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

            // Target
            var targetEM = BattleFSM.Instance.enemies.Find(x => x.Coord == Target);
            if (targetEM != null)
            {
                var damageInterface = targetEM as IDamageable;
                damageInterface.Damage(20);
            }

            Animator.CrossFade("Idle", 0.2f);
            //Animator.applyRootMotion = false;
            Animator = null;
        }

        public override List<ActionSelectionModel> selectionModels()
        {
            var selectionModel = new AreaTileSelection();
            selectionModel.Range = 20;
            selectionModel.ShapeType = AreaTileSelection.AreaType.Circle;

            var model = new ActionSelectionModel(selectionModel, new AreaTileSelection());
            var models = new List<ActionSelectionModel>();
            models.Add(model);

            return models;
        }
    }
}