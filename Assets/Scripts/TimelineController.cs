using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class TimelineController : MonoBehaviour
    {
        public EnemyBase getNextEnemy()
        {
            BattleFSM battleFSM = BattleFSM.Instance;

            if (battleFSM.enemies.Count == 0)
            {
                return null;
            }

            List<EnemyBase> enemies = new List<EnemyBase>(battleFSM.enemies);
            enemies.Sort((a, b) => a.Cooldown.CompareTo(b.Cooldown));

            var minCD = enemies[0].Cooldown;
            foreach (var unit in enemies)
            {
                unit.Cooldown -= minCD;
            }

            return enemies[0];
        }
    }
}

