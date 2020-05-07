using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public interface IDamageable
    {
        int Health { get; }

        void Damage(int damageTaken);

        void Heal(int healAmount);
    }
}