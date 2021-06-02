using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

namespace sail
{
    public class ActionManager : MonoBehaviour
    {
        // All existing actions
        static List<Type> ActionTypes = new List<Type>()
        {
            typeof(IdleAction),
            typeof(MoveAction),
            typeof(AttackAction),
            typeof(RangedAttackAction),
            typeof(ShieldAction),
            typeof(FireAction),
            typeof(RepairAction),
            typeof(JumpAction)
        };

        // public
        public ActionConfigurationList ActionConfigurationList = null;


        //private
        public List<ActionBase> Actions { get; } = new List<ActionBase>();

        private void Awake()
        {
            GlobalManagers.actionManager = this;
        }

        void Start()
        {
            Assert.IsNotNull(ActionConfigurationList, "@ActionManager: Action List is null, please set the Scriptable Object parameter. ");

            // Pre gather action ids
            var actionTypesID = new List<ActionID>();
            foreach (var actionType in ActionTypes)
            {
                var idProperty = actionType.GetProperty("ID");
                Assert.IsNotNull(idProperty, $"@ActionManager: Type: {actionType.Name} doesn't have an ID property.");

                var id = (ActionID)idProperty.GetValue(null, null);
                Assert.AreNotEqual(id, ActionID.UNDEFINED, $"@ActionManager: Type: {actionType.Name} has an Undefined an ID property.");

                actionTypesID.Add(id);
            }

            // find config associated type
            foreach (var config in ActionConfigurationList.ActionList)
            {
                var foundIndex = actionTypesID.FindIndex(e => e == config.ActionID);

                Assert.AreNotEqual(foundIndex, -1, "@ActionManager: Action in the configure is not a registered type.");

                var actionType = ActionTypes[foundIndex];
                var instance = (ActionBase)Activator.CreateInstance(actionType);
                instance.configure(config);
                Actions.Add(instance);

                Debug.Log($"@ActionManager: Added action {instance.Name}");
            }
        }

        void OnDestroy()
        {
            GlobalManagers.actionManager = null;
        }

        void Update()
        {

        }
    }
}