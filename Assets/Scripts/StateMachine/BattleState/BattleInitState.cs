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
                var tileCoord = new TileCoord(0, 0);
                var pos = board.getTile(tileCoord).gameObject.transform.position;

                var enemyGO = Instantiate(owner.UnitsPrefab[0], pos + heightOffset, Quaternion.identity);
                var em = enemyGO.GetComponent<EnemyBase>();
                //em.UnitName = 
                em.Cooldown = 5;
                em.Coord = tileCoord;
                enemies.Add(em);

                tileCoord = new TileCoord(board.Width - 1, board.Height - 1);
                pos = board.getTile(tileCoord).gameObject.transform.position;
                enemyGO = Instantiate(owner.UnitsPrefab[1], pos + heightOffset, Quaternion.identity);
                em = enemyGO.GetComponent<EnemyBase>();
                em.Coord = tileCoord;
                em.Cooldown = 10;
                enemies.Add(em);

                owner.SelectedEnemy = enemies[0];
            }
        }
    }
}
