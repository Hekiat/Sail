using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;


public class ActionDatabase
{

    /// List of all ActionID / ActionType dictionnary
    static List<Tuple<ActionID, Type>> ActionTypes = new List<Tuple<ActionID, Type>>()
    {
        new Tuple<ActionID, Type>(ActionID.MOVE, typeof(MoveAction))
    };
}
