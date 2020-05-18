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

        private Animator Animator = null;

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

            Animator = BattleFSM.Instance.SelectedEnemy.Animator;

            Animator.CrossFade("Fire", 0.2f);
            //Animator.applyRootMotion = true;

            Target = BattleFSM.Instance.TileSelectionController.selectedTiles()[0];
        }

        public override void run()
        {
            //if (Animator.HasState(0, Animator.StringToHash("Fire")) == false)
            //{
            //    return;
            //}

            var character = BattleFSM.Instance.SelectedEnemy;
            var tile = BattleFSM.Instance.board.getTile(Target);

            // Homing
            //bool homingEnded = false;
            //while (homingEnded == false)
            if(Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.35f)
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

                return;
                //yield return null;
            }

            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Fire") == false)
            {
                return;
            }

            if (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.35f && FirePS.isPlaying == false)
            {
                FireInst.transform.position = Animator.GetBoneTransform(HumanBodyBones.RightHand).position + character.transform.forward * 0.3f;
                var rotation = character.transform.eulerAngles;
                rotation.x += 90f;
                FireInst.transform.eulerAngles = rotation;
                FirePS.Play(true);
            }

            // Transitioning
            //yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).IsName("Fire"));
            //
            //yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.35f);
            //FireInst.transform.position = character.transform.position + Vector3.up * 1f + character.transform.forward * 1f;

            //FireInst.transform.position = Animator.GetBoneTransform(HumanBodyBones.RightHand).position + character.transform.forward * 0.3f;

            

            // Waiting for the end of the motion
            //yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

            if (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                // Target
                var targetEM = BattleFSM.Instance.enemies.Find(x => x.Coord == Target);
                if (targetEM != null)
                {
                    var damageInterface = targetEM as IDamageable;
                    damageInterface.Damage(10);
                }

                Animator.CrossFade("Idle", 0.2f);
                //Animator.applyRootMotion = false;
                Animator = null;

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