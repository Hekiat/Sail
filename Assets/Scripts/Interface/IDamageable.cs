﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public interface IDamageable
    {
        void Damage(int damageTaken);
    }
}