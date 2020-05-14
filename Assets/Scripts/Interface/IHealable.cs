using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public interface IHealable
    {
        void Heal(int healAmount);
    }
}