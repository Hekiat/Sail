using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace sail
{
    [CreateAssetMenu(fileName = "ActionListConfiguration", menuName = "Custom/ActionListConfiguration", order = 0)]
    public class ActionConfigurationList : ScriptableObject
    {
        public List<ActionBaseConfiguration> ActionList = null;
    }
}
