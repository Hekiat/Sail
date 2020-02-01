using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PluginSlot
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

public class VariableBase
{
    int value = 5;
}

public enum FunctionID
{
    MOVE,
    ATTACK,
    COUNT
}

public class ActionBase
{
    FunctionID FunctionID = FunctionID.MOVE;

    // As a main function
    List<PluginSlot> FunctionSlots = new List<PluginSlot>();
    List<VariableBase> FunctionVariableSlots = new List<VariableBase>();

    // As a plugin
    List<VariableBase> PluginVariableSlots = new List<VariableBase>();
}

[CreateAssetMenu(fileName = "Data", menuName = "Custom/Function", order = 1)]
public class FunctionClass : ScriptableObject
{
    public string Name = "New MyScriptableObject";

    public FunctionID FunctionID = FunctionID.MOVE;

    public int FunctionSlot = 0;

    public int FunctionVariableSlot = 0;

    public int PluginVariableSlot = 0;
}