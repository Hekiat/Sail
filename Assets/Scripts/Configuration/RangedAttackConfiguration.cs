using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    [CreateAssetMenu(fileName = "RangedAttackConfiguration", menuName = "Custom/RangedAttackConfiguration", order = 3)]
    public class RangedAttackConfiguration : ActionBaseConfiguration
    {
        public GameObject ProjectilePrefab = null;

        public int Damage = 20;
    }
}