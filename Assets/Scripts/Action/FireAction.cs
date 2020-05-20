using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class FireAction : ActionBase
    {
        public static new ActionID ID { get; set; } = ActionID.FIRE;
        public override ActionID id() { return ID; }

        public override int SelectionCount => 1;

        private TileCoord Target;

        private GameObject FireInst = null;
        private ParticleSystem FirePS = null;

        public override void configure(ActionBaseConfiguration config)
        {
            base.configure(config);

            var c = config as FireConfiguration;
            if (c == null)
            {
                return;
            }

            FireInst = GameObject.Instantiate(c.FireFXPrefab);
            FirePS = FireInst.GetComponent<ParticleSystem>();
            FirePS.Stop(true);
        }

        public override void start()
        {
            base.start();

            Unit.MotionController.requestMotion(EmMotionStates.Fire, 0.2f);

            Target = BattleFSM.Instance.TileSelectionController.selectedTiles()[0];
        }

        public override void run()
        {
            

            float currentTime = Unit.MotionController.currentStateNormalizedTime();

            if (currentTime < 0.35f)
            {
                applyHoming(Target);
                return;
            }

            if (currentTime >= 0.35f && FirePS.isPlaying == false)
            {
                FireInst.transform.position = Unit.Animator.GetBoneTransform(HumanBodyBones.RightHand).position + Unit.transform.forward * 0.3f;
                var rotation = Unit.transform.eulerAngles;
                rotation.x += 90f;
                FireInst.transform.eulerAngles = rotation;
                FirePS.Play(true);
            }

            if (currentTime >= 1.0f)
            {
                // Target
                var targetEM = BattleFSM.Instance.enemies.Find(x => x.Coord == Target);
                if (targetEM != null)
                {
                    var damageInterface = targetEM as IDamageable;
                    damageInterface.Damage(10);
                }

                Unit.MotionController.requestMotion(EmMotionStates.Idle, 0.2f);
                FirePS.Stop(true);
                ActionEnded = true;
            }
        }

        public override List<ActionSelectionModel> selectionModels()
        {
            var selectionModel = new AreaTileSelection();
            selectionModel.Range = 1;
            selectionModel.ShapeType = AreaTileSelection.AreaType.Cross;

            var targetModel = new ConeTileSelection();
            targetModel.Range = 3;

            var model = new ActionSelectionModel(selectionModel, targetModel);
            var models = new List<ActionSelectionModel>();
            models.Add(model);

            return models;
        }
    }

}