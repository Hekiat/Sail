using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public abstract class ActionBaseConfiguration : ScriptableObject
    {
        public ActionID ActionID = ActionID.UNDEFINED;

        public string Name = "ActionName";

        public int ActionSlot = 0;

        public int VariableSlot = 0;

        public int SecondaryVariableSlot = 0;
    }
}
