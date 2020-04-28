using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    [CreateAssetMenu(fileName = "FireConfiguration", menuName = "Custom/FireConfiguration", order = 3)]
    public class FireConfiguration : ActionBaseConfiguration
    {
        public int Damage = 3;
        public int Range = 3;
    }
}

