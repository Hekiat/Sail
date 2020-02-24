using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

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
    List<ActionSlot> ActionSlots = new List<ActionSlot>();
    List<VariableBase> VariableSlots = new List<VariableBase>();

    // As a slotted action
    List<VariableBase> SecondaryVariableSlots = new List<VariableBase>();

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
}

[CreateAssetMenu(fileName = "ActionListConfiguration", menuName = "Custom/ActionListConfiguration", order = 0)]
public class ActionListConfiguration : ScriptableObject
{
    public List<ActionBaseConfiguration> ActionList = null;
}

public abstract class ActionBaseConfiguration : ScriptableObject
{
    public ActionID ActionID = ActionID.UNDEFINED;

    public string Name = "ActionName";

    public int ActionSlot = 0;

    public int VariableSlot = 0;

    public int SecondaryVariableSlot = 0;
}


[CreateAssetMenu(fileName = "MoveConfiguration", menuName = "Custom/MoveConfiguration", order = 2)]
public class MoveConfiguration : ActionBaseConfiguration
{
    public int MoveRange = 3;
}


