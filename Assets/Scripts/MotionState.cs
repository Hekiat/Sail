using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class MotionState
    {
        public int Hash { get; private set; }
        public string Name { get; private set; }

        public MotionState(string name)
        {
            Name = name;
            Hash = Animator.StringToHash(Name);
        }

        public MotionState(string name, int hash)
        {
            Name = name;
            Hash = hash;
        }

        public static bool operator ==(MotionState a, MotionState b)
        {
            return a.Hash == b.Hash;
        }

        public static bool operator !=(MotionState a, MotionState b)
        {
            return a.Hash != b.Hash;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is MotionState))
            {
                return false;
            }

            return Hash == ((MotionState)obj).Hash;
        }

        public override int GetHashCode()
        {
            return Hash;
        }
    }
}