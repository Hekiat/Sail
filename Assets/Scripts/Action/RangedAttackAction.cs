﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class RangedAttackAction : ActionBase
    {
        public static new ActionID ID { get; set; } = ActionID.RANGED_ATTACK;
        public override ActionID id() { return ID; }

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

        public override void run()
        {
            //if (Animator.HasState(0, Animator.StringToHash("RangedAttack")) == false)
            //{
            //    //yield break;
            //    return;
            //}
            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("RangedAttack") == false)
            {
                return;
            }

            var character = BattleFSM.Instance.SelectedEnemy;
            var targetPos = character.transform.position;
            var tile = BattleFSM.Instance.board.getTile(Target);

            // Homing
            //bool homingEnded = false;
            //while (homingEnded == false)
            if(Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.4f)
            {
                var targetDir = tile.transform.position - character.transform.position;
                targetDir.y = 0f;

                const float maxRotationAngle = 2f;
                var angle = Vector3.SignedAngle(character.transform.forward, targetDir, Vector3.up);
                var deltaAngle = angle < 0f ? Mathf.Max(angle, -maxRotationAngle) : Mathf.Min(angle, maxRotationAngle);
                character.transform.Rotate(Vector3.up, deltaAngle);

                if (Mathf.Abs(angle) < 2f)
                {
                    //homingEnded = true;
                }

                // yield return null;
                return;
            }

            // Transitioning
            //yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).IsName("RangedAttack"));

            // Waiting for the end of the motion
            //yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f);


            if (ProjectileSpawned == false)
            {
                var characterPos = character.transform.position;

                var projectileStartPos = characterPos + Vector3.up * 1.3f + character.transform.forward;
                var projectileGO = GameObject.Instantiate(ProjectilePrefab, projectileStartPos, Quaternion.identity, character.transform);
                ProjectileInst = projectileGO.GetComponent<Projectile>();
                ProjectileInst.StartPosition = projectileStartPos;
                ProjectileInst.EndPosition = tile.transform.position + Vector3.up * (1f + 1.3f) + tile.HeightOffset;
                ProjectileInst.Speed = 10f;
                //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = projectile.EndPosition;
                ProjectileSpawned = true;
            }


            if(ProjectileSpawned == true && ProjectileInst != null)
            {
                return;
            }

            // wait projectile to reach
            //yield return new WaitUntil(() => ProjectileInst == null);

            // Waiting for the end of the motion
           // yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

            if (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                // Target
                var targetEM = BattleFSM.Instance.enemies.Find(x => x.Coord == Target);
                if (targetEM != null)
                {
                    var damageInterface = targetEM as IDamageable;
                    damageInterface.Damage(20);
                }

                Animator.CrossFade("Idle", 0.2f);
                Animator = null;
                ProjectileInst = null;
                ProjectileSpawned = false;
                ActionEnded = true;
            }
        }

        Projectile ProjectileInst = null;
        bool ProjectileSpawned = false;

        public override List<ActionSelectionModel> selectionModels()
        {
            var selectionModel = new AreaTileSelection();
            selectionModel.ShapeType = AreaTileSelection.AreaType.Circle;
            selectionModel.Range = 50;
            selectionModel.Filter = TileSelectionFilter.ENEMIES;

            var model = new ActionSelectionModel(selectionModel, null);
            var models = new List<ActionSelectionModel>();
            models.Add(model);

            return models;
        }
    }
}