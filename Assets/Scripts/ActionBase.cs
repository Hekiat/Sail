using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

namespace sail
{
    public class ActionSlot
    {
        ActionBase function = null;

        bool isEmpty { get { return function == null; } }
    }

    public class VariableSlot
    {
        string description = String.Empty;

        VariableBase variable = null;

        bool isEmpty { get { return variable == null; } }
    }

    public abstract class VariableBase
    {
    }

    public class IntVariable : VariableBase
    {
        public int value = 0;
    }

    public class FloatVariable : VariableBase
    {
        public float value = 0f;
    }

    public enum VariableID
    {
        INT,
        FLOAT,
        COUNT
    }

    public enum ActionID
    {
        UNDEFINED,
        MOVE,
        ATTACK,
        COUNT
    }


    public abstract class ActionBase
    {
        public string Name { get; private set; } = "Action Name";

        // As an action
        public List<ActionSlot> ActionSlots = new List<ActionSlot>();
        public List<VariableBase> VariableSlots = new List<VariableBase>();

        private List<ActionBase> SecondaryActions = new List<ActionBase>();

        // As a slotted action
        public List<VariableBase> SecondaryVariableSlots = new List<VariableBase>();

        public virtual void configure(ActionBaseConfiguration config)
        {
            Assert.IsNotNull(config, "Action Configuration is null. Type: " + GetType());

            Name = config.Name;

            for (int i = 0; i < config.ActionSlot; i++)
            {
                ActionSlots.Add(new ActionSlot());
            }

            for (int i = 0; i < config.VariableSlot; i++)
            {
                VariableSlots.Add(new FloatVariable());
            }

            for (int i = 0; i < config.SecondaryVariableSlot; i++)
            {
                SecondaryVariableSlots.Add(new FloatVariable());
            }
        }

        public virtual void start(List<ActionBase> secondaryActions)
        {
            SecondaryActions = new List<ActionBase>(secondaryActions);
        }

        public virtual IEnumerator run(){ yield break; }
    }
}