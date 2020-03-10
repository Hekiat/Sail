using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class TimelineController : MonoBehaviour
    {
        public Unit getNextUnit()
        {
            BattleFSM battleFSM = BattleFSM.Instance;

            List<Unit> units = new List<Unit>(battleFSM.units);
            units.Sort((a, b) => a.Cooldown.CompareTo(b.Cooldown));

            var minCD = units[0].Cooldown;

            foreach (var unit in units)
            {
                unit.Cooldown -= minCD;
            }

            return units[0];
        }
    }
}

