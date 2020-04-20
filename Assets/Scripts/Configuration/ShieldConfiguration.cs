using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    [CreateAssetMenu(fileName = "ShieldConfiguration", menuName = "Custom/ShieldConfiguration", order = 1)]
    public class ShieldConfiguration : ActionBaseConfiguration
    {
        public int ShieldAmount = 100;
    }
}

