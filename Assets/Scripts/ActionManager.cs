using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

public class ActionManager : MonoBehaviour
{
    // All existing actions
    static List<Tuple<ActionID, Type>> ActionTypes = new List<Tuple<ActionID, Type>>()
    {
        new Tuple<ActionID, Type>(ActionID.MOVE, typeof(MoveAction)),
        new Tuple<ActionID, Type>(ActionID.ATTACK, typeof(AttackAction)),
    };

    // public
    public ActionConfigurationList ActionConfigurationList = null;


    //private
    public List<ActionBase> Actions { get; } = new List<ActionBase>();

    void Start()
    {
        Assert.IsNotNull(ActionConfigurationList, "@ActionManager: Action List is null, please set the Scriptable Object parameter. ");

        foreach (var config in ActionConfigurationList.ActionList)
        {
            var actionType = ActionTypes.Find(c => c.Item1 == config.ActionID);

            Assert.IsNotNull(actionType, "@ActionManager: Action in the configure is not a registered type.");
            var instance = (ActionBase)Activator.CreateInstance(actionType.Item2);
            instance.configure(config);
            Actions.Add(instance);

            Debug.Log($"@ActionManager: Added action {instance.Name}");
        }

        GlobalManagers.actionManager = this;
    }

    void OnDestroy()
    {
        GlobalManagers.actionManager = null;
    }

    void Update()
    {
        
    }
}
