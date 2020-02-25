using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    [CreateAssetMenu(fileName = "MoveConfiguration", menuName = "Custom/MoveConfiguration", order = 1)]
    public class MoveConfiguration : ActionBaseConfiguration
    {
         public int MoveRange = 3;
    }
}
