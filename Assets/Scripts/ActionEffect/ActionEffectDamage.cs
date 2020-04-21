using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class ActionEffectDamage : ActionEffectBase
    {
        public int Damage { get; set; } = 0;

        public ActionEffectDamage(Unit owner, Unit target, int damage)
        {
            Owner = owner;
            Target = target;
            Damage = damage;
        }
    }
}