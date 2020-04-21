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

        public override int SelectionCount => 1; //{ get { return 1; } }

        private List<TileCoord> Path = null;

        public override void start()
        {
            base.start();

            var owner = BattleFSM.Instance;
            var character = owner.SelectedEnemy;
            character.Animator.CrossFade("Walk", 0.5f);

            Path = AStarSearch.search(owner.SelectedEnemy.Coord, owner.TileSelectionController.selectedTiles()[0]);
            Path.RemoveAt(0);

            //owner.board.getTile(Target).GetComponent<MeshRenderer>().material.color = Color.yellow;
        }

        public override IEnumerator run()
        {
            var character = BattleFSM.Instance.SelectedEnemy;

            int i = 0;
            foreach (var target in Path)
            {
                BattleFSM.Instance.board.getTile(target).GetComponent<MeshRenderer>().material.color = Color.yellow;

                Func<float, float, float, float> fun = EasingFunctions.Linear;

                if (i == 0)
                {
                    fun = EasingFunctions.EaseInSine;
                }
                else if (i == Path.Count - 1)
                {
                    fun = EasingFunctions.EaseOutSine;

                    character.Animator.CrossFade("Idle", 0.5f);
                }

                //yield return ActionFunctionLibrary.moveTo(character.gameObject, target, fun);

                var moveToCoroutine = ActionFunctionLibrary.moveTo(character.gameObject, target, fun);

                while (moveToCoroutine.MoveNext())
                {
                    var tile = BattleFSM.Instance.board.getTile(target);
                    var targetPos = tile.transform.position;
                    var targetDir = targetPos - character.transform.position;
                    targetDir.y = 0f;
                
                    const float maxRotationAngle = 2f;
                    var angle = Vector3.SignedAngle(character.transform.forward, targetDir, Vector3.up);
                    angle = angle < 0f ? angle = Math.Max(angle, -maxRotationAngle) : angle = Math.Min(angle, maxRotationAngle);
                    character.transform.Rotate(Vector3.up, angle);
                
                    yield return null;
                }

                BattleFSM.Instance.board.getTile(target).GetComponent<MeshRenderer>().material.color = Color.white;

                ++i;
            }

            character.Coord = Path[Path.Count-1];
        }

        public override List<ActionSelectionModel> selectionModels()
        {
            var selectionModel = new AreaTileSelection();
            selectionModel.Range = 5;
            selectionModel.ShapeType = AreaTileSelection.AreaType.Circle;

            var model = new ActionSelectionModel(selectionModel, new AreaTileSelection());
            var models = new List<ActionSelectionModel>();
            models.Add(model);

            return models;
        }
    }
}