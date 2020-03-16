using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace sail
{
    public enum EnemyID
    {
        em0000,
        em1000
    }

    public class EnemyConfiguration : ScriptableObject
    {
        public EnemyID ID = EnemyID.em0000;

        public int Heath = 1000;
    }
}