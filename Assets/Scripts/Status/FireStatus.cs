using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class FireStatus : StatusBase
    {
        //public override StatusID ID { get; } = StatusID.Fire;

        public override void run()
        {
            base.run();

            var owner = Owner as IDamageable;
            owner.Damage(Value);
            Value -= 1;
        }
    }
}