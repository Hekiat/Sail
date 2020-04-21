using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public abstract class ActionEffectBase
    {
        public Unit Owner { get; set; } = null;
        public Unit Target { get; set; } = null;
    }
}