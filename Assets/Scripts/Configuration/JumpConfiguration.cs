using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    [CreateAssetMenu(fileName = "JumpConfiguration", menuName = "Custom/JumpConfiguration", order = 3)]
    public class JumpConfiguration : ActionBaseConfiguration
    {
        public int Range = 3;
    }
}