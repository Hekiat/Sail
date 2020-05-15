using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    //public enum StatusID
    //{
    //    Unknown,
    //    Shield,
    //    Fire,
    //}

    public abstract class StatusBase
    {
        //public virtual StatusID ID { get; } = StatusID.Unknown;

        public Unit Owner { get; set; } = null;
        public int Value { get; protected set; } = 0;

        public virtual void add(int amount) { Value += amount; }
        public virtual void remove(int amount) { Value -= amount; }

        public virtual void run() { }
    }
}