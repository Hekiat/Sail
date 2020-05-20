using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class RangedAttackAction : ActionBase
    {
        public static new ActionID ID { get; set; } = ActionID.RANGED_ATTACK;
        public override ActionID id() { return ID; }

        public override int SelectionCount => 1;

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

            Unit.MotionController.requestMotion(EmMotionStates.RangedAttack, 0.2f);

            Target = BattleFSM.Instance.TileSelectionController.selectedTiles()[0];
        }

        public override void run()
        {
            var time = Unit.MotionController.currentStateNormalizedTime();
            if (time < 0.4f)
            {
                applyHoming(Target);

                return;
            }

            if (ProjectileSpawned == false)
            {
                var tile = BattleFSM.Instance.board.getTile(Target);
                var characterPos = Unit.transform.position;

                var projectileStartPos = characterPos + Vector3.up * 1.3f + Unit.transform.forward;
                var projectileGO = GameObject.Instantiate(ProjectilePrefab, projectileStartPos, Quaternion.identity, Unit.transform);
                ProjectileInst = projectileGO.GetComponent<Projectile>();
                ProjectileInst.StartPosition = projectileStartPos;
                ProjectileInst.EndPosition = tile.transform.position + Vector3.up * (1f + 1.3f) + tile.HeightOffset;
                ProjectileInst.Speed = 10f;
                ProjectileSpawned = true;
            }

            if(ProjectileSpawned == true && ProjectileInst != null)
            {
                return;
            }

            if (time >= 1.0f)
            {
                var targetEM = BattleFSM.Instance.enemies.Find(x => x.Coord == Target);
                if (targetEM != null)
                {
                    var damageInterface = targetEM as IDamageable;
                    damageInterface.Damage(20);
                }

                Unit.MotionController.requestMotion(EmMotionStates.Idle, 0.2f);
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