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
        IDLE,
        MOVE,
        ATTACK,
        RANGED_ATTACK,
        SHIELD,
        FIRE,
        REPAIR,
        JUMP,
        COUNT
    }


    public abstract class ActionBase
    {
        public static ActionID ID { get; set; } = ActionID.UNDEFINED;

        public virtual ActionID id() { return ID; }

        public string Name { get; private set; } = "UNDEFINED";

        public Unit Unit { get; private set; } = null;

        // Configure data
        public int Cost { get; private set; }
        public int MaxSecondaryActionSlots { get; private set; }

        // Runtime data
        protected List<ActionBase> SecondaryActions = new List<ActionBase>();

        public virtual void configure(ActionBaseConfiguration config)
        {
            Assert.IsNotNull(config, "Action Configuration is null. Type: " + GetType());

            Name = config.Name;
            MaxSecondaryActionSlots = config.ActionSlot;
            Cost = config.Cost;
        }

        public virtual int SelectionCount { get { return selectionModels().Count; } }

        public virtual List<ActionSelectionModel> selectionModels() { return new List<ActionSelectionModel>(); }

        public virtual void setup(List<ActionBase> secondaryActions)
        {
            SecondaryActions = new List<ActionBase>(secondaryActions);
        }

        public virtual void start()
        {
            ActionEnded = false;
            Unit = BattleFSM.Instance.SelectedEnemy;
        }

        public virtual bool ActionEnded { get; protected set; }

        public virtual void run(){ ActionEnded = true; }

        public virtual List<ActionEffectBase> predictEffects(List<TileCoord> targets) { return new List<ActionEffectBase>(); }

        public void applyHoming(Vector3 targetPosition)
        {
            var targetDir = targetPosition - Unit.transform.position;
            targetDir.y = 0f;

            const float maxRotationAngle = 2f;
            var angle = Vector3.SignedAngle(Unit.transform.forward, targetDir, Vector3.up);
            var deltaAngle = angle < 0f ? Mathf.Max(angle, -maxRotationAngle) : Mathf.Min(angle, maxRotationAngle);
            Unit.transform.Rotate(Vector3.up, deltaAngle);
        }

        public void applyHoming(TileCoord target)
        {
            var tile = BattleFSM.Instance.board.getTile(target);
            applyHoming(tile.transform.position);
        }
    }
}