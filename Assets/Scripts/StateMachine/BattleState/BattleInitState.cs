using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace sail
{
    public class BattleInitState : BattleBaseState
    {
        public override void Enter()
        {
            base.Enter();
            StartCoroutine(Init());
        }

        IEnumerator Init()
        {
            board.Generate();

            if (owner.HUDPrefab)
            {
                var go = Instantiate(owner.HUDPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
                spawnTest();
                owner.hud = go.GetComponent<BattleHUD>();
            }

            yield return null;

            owner.ChangeToState<BattleUnitSelectionState>();
        }

        void spawnTest()
        {
            //foreach (var prefab in UnitsPrefab)
            {
                var heightOffset = Vector3.up;
                var pos = board.getTile(new TileCoord(0, 0)).GameObject.transform.position;

                var unitGO = Instantiate(owner.UnitsPrefab[0], pos + heightOffset, Quaternion.identity);
                var unit = unitGO.GetComponent<Unit>();
                unit.Cooldown = 5;
                units.Add(unit);

                pos = board.getTile(new TileCoord(board.Width - 1, board.Height - 1)).GameObject.transform.position;
                unitGO = Instantiate(owner.UnitsPrefab[1], pos + heightOffset, Quaternion.identity);
                unit = unitGO.GetComponent<Unit>();
                unit.Cooldown = 10;
                units.Add(unit);
            }
        }
    }
}
