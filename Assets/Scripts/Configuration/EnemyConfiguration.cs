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

    [CreateAssetMenu(fileName = "EnemyConfiguration", menuName = "Custom/EnemyConfiguration", order = 0)]
    public class EnemyConfiguration : ScriptableObject
    {
        public EnemyID ID = EnemyID.em0000;

        public string Name = string.Empty;
        public int Health = 1000;
    }
}