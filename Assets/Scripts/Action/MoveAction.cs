using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using sail.animation;

namespace sail
{
    public class MoveAction : ActionBase
    {
        public static new ActionID ID { get; set; } = ActionID.MOVE;
        public override ActionID id() { return ID; }

        public override int SelectionCount => 1;

        private List<TileCoord> Path = null;
        private TileCoord Target;

        private MoveConfiguration Config = null;

        private string StateName = string.Empty;

        public override void configure(ActionBaseConfiguration config)
        {
            base.configure(config);

            Config = config as MoveConfiguration;
        }

        public override void start()
        {
            base.start();

            var owner = BattleFSM.Instance;
            var character = owner.SelectedEnemy;

            StateName = "Walk";

            foreach (var sa in SecondaryActions)
            {
                if (sa.id() == ActionID.ATTACK)
                {
                    StateName = "DashAttack";
                }
            }

            character.Animator.CrossFade(StateName, 0.5f);

            Target = owner.TileSelectionController.selectedTiles()[0];

            if (StateName == "Walk")
            {
                Path = AStarSearch.search(owner.SelectedEnemy.Coord, Target);
                Path.RemoveAt(0);
            }

            foreach(var sa in SecondaryActions)
            {
                if (sa.id() == ActionID.SHIELD)
                {
                    var characterTrans = BattleFSM.Instance.SelectedEnemy.transform;
                    var HologramInst = GameObject.Instantiate(Config.HologramPrefab, characterTrans.position, characterTrans.rotation);
                    var ps = HologramInst.GetComponentInChildren<ParticleSystem>();
                    ps.Play();
                }
            }
        }

        public override void run()
        {
            if (StateName == "Walk")
            {
                //yield return walk();
                walk();
            }

            if (StateName == "DashAttack")
            {
                //yield return dashAttack();
                dashAttack();
            }
        }

        public override List<ActionSelectionModel> selectionModels()
        {
            var selectionModel = new AreaTileSelection();
            selectionModel.Range = 5;
            selectionModel.ShapeType = AreaTileSelection.AreaType.Circle;

            var model = new ActionSelectionModel(selectionModel, null);
            var models = new List<ActionSelectionModel>();
            models.Add(model);

            return models;
        }

        private Tweener CurrentTweener = null;
        private bool Moving = false;
        private int PathIndex = 0;

        // Actions
        private void walk()
        {
            var character = BattleFSM.Instance.SelectedEnemy;

            if (PathIndex == Path.Count)
            {
                if (CurrentTweener == null)
                {
                    character.Coord = Path[Path.Count - 1];

                    Path.Clear();
                    ActionEnded = true;
                    PathIndex = 0;
                }
                
                return;
            }

            var target = Path[PathIndex];

            var tile = BattleFSM.Instance.board.getTile(target);
            var targetPos = tile.transform.position;
            var targetDir = targetPos - character.transform.position;
            targetDir.y = 0f;

            const float maxRotationAngle = 2f;
            var angle = Vector3.SignedAngle(character.transform.forward, targetDir, Vector3.up);
            angle = angle < 0f ? angle = Math.Max(angle, -maxRotationAngle) : angle = Math.Min(angle, maxRotationAngle);
            character.transform.Rotate(Vector3.up, angle);

            if (CurrentTweener != null)
            {
                return;
            }

            BattleFSM.Instance.board.getTile(target).GetComponent<MeshRenderer>().material.color = Color.yellow;

            Func<float, float, float, float> fun = EasingFunctions.Linear;

            if (PathIndex == 0)
            {
                fun = EasingFunctions.EaseInSine;
            }
            else if (PathIndex == Path.Count - 1)
            {
                fun = EasingFunctions.EaseOutSine;

                character.Animator.CrossFade("Idle", 0.5f);
            }

            //yield return ActionFunctionLibrary.moveTo(character.gameObject, target, fun);
            CurrentTweener = ActionFunctionLibrary.moveTo(character.gameObject, target, fun);

            BattleFSM.Instance.board.getTile(target).GetComponent<MeshRenderer>().material.color = Color.white;

            ++PathIndex;
        }

        private void dashAttack()
        {
            var character = BattleFSM.Instance.SelectedEnemy;

            var dir = Target - character.Coord;

            if (dir.Square.x < 0)
            {
                dir = -1 * TileCoord.AxisX;
            }
            else if (dir.Square.x > 0)
            {
                dir = TileCoord.AxisX;
            }
            else if (dir.Square.y < 0)
            {
                dir = -1 * TileCoord.AxisY;
            }
            else if (dir.Square.y > 0)
            {
                dir = TileCoord.AxisY;
            }

            if(CurrentTweener == null && Moving == false)
            {
                Moving = true;
                CurrentTweener = ActionFunctionLibrary.moveTo(character.gameObject, Target - dir, EasingFunctions.EaseInSine);
            }

            if (CurrentTweener != null)
            {
                var targetPos = BattleFSM.Instance.board.getTile(Target).transform.position;
                var targetDir = targetPos - BattleFSM.Instance.board.getTile(Target - dir).transform.position;

                const float maxRotationAngle = 2f;
                var angle = Vector3.SignedAngle(character.transform.forward, targetDir, Vector3.up);
                angle = angle < 0f ? angle = Math.Max(angle, -maxRotationAngle) : angle = Math.Min(angle, maxRotationAngle);
                character.transform.Rotate(Vector3.up, angle);

                return;
            }

            var targetEM = BattleFSM.Instance.enemies.Find(x => x.Coord == Target);
            if (targetEM != null)
            {
                var damageInterface = targetEM as IDamageable;
                damageInterface.Damage(20);
            }

            //yield return new WaitUntil(() => character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f);

            if (character.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                character.Coord = Target - dir;
                character.Animator.CrossFade("Idle", 0.5f);
                ActionEnded = true;
            }
        }
    }
}