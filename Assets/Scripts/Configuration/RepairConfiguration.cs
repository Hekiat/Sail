using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    [CreateAssetMenu(fileName = "RepairConfiguration", menuName = "Custom/RepairConfiguration", order = 1)]
    public class RepairConfiguration : ActionBaseConfiguration
    {
        public int HealAmount = 3;
    }
}