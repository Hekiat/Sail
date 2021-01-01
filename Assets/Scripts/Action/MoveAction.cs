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

        private MotionState State = null;

        public override void configure(ActionBaseConfiguration config)
        {
            base.configure(config);

            Config = config as MoveConfiguration;
        }

        public override void start()
        {
            base.start();

            State = EmMotionStates.Walk;

            foreach (var sa in SecondaryActions)
            {
                if (sa.id() == ActionID.ATTACK)
                {
                    State = EmMotionStates.DashAttack;
                }
            }

            Unit.MotionController.requestMotion(State, 0.5f);

            Target = BattleFSM.Instance.TileSelectionController.selectedTiles()[0];

            if (State == EmMotionStates.Walk)
            {
                //Path = AStarSearch.search(Unit.Coord, Target);
                Path = BattleFSM.Instance.board.getPath(Unit.Coord, Target);
                Path.RemoveAt(0);
            }

            foreach(var sa in SecondaryActions)
            {
                if (sa.id() == ActionID.SHIELD)
                {
                    var characterTrans = Unit.transform;
                    var HologramInst = GameObject.Instantiate(Config.HologramPrefab, characterTrans.position, characterTrans.rotation);
                    var ps = HologramInst.GetComponentInChildren<ParticleSystem>();
                    ps.Play();
                }
            }
        }

        public override void run()
        {
            if (State == EmMotionStates.Walk)
            {
                walk();
            }

            if (State == EmMotionStates.DashAttack)
            {
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
            if (PathIndex == Path.Count)
            {
                if (CurrentTweener == null)
                {
                    Unit.Coord = Path[Path.Count - 1];

                    Path.Clear();
                    ActionEnded = true;
                    PathIndex = 0;
                }
                
                return;
            }

            var target = Path[PathIndex];
            applyHoming(target);

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

                Unit.MotionController.requestMotion(EmMotionStates.Idle, 0.5f);
            }

            CurrentTweener = ActionFunctionLibrary.moveTo(Unit.gameObject, target, fun);

            BattleFSM.Instance.board.getTile(target).GetComponent<MeshRenderer>().material.color = Color.white;

            ++PathIndex;
        }

        private void dashAttack()
        {
            var dir = Target - Unit.Coord;

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
                CurrentTweener = ActionFunctionLibrary.moveTo(Unit.gameObject, Target - dir, EasingFunctions.EaseInSine);
            }

            if (CurrentTweener != null)
            {
                applyHoming(Target);
                return;
            }

            var targetEM = BattleFSM.Instance.enemies.Find(x => x.Coord == Target);
            if (targetEM != null)
            {
                var damageInterface = targetEM as IDamageable;
                damageInterface.Damage(20);
            }

            if (Unit.MotionController.currentStateNormalizedTime() >= 0.95f)
            {
                Unit.Coord = Target - dir;
                Unit.MotionController.requestMotion(EmMotionStates.Idle, 0.5f);
                ActionEnded = true;
            }
        }
    }
}