using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    [CreateAssetMenu(fileName = "AttackConfiguration", menuName = "Custom/AttackConfiguration", order = 3)]
    public class AttackConfiguration : ActionBaseConfiguration
    {
        public int Damage = 3;
    }
}

