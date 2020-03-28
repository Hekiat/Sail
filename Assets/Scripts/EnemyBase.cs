using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class EnemyBase : Unit
    {
        public EnemyConfiguration Configuration = null;

        void Start()
        {
            configure();
        }

        void Update()
        {
        
        }

        void configure()
        {
            if(Configuration == null)
            {
                Debug.LogWarning("Enemy Core doesn't have configuration file");
                return;
            }

            UnitName = Configuration.Name;
            MaxHealth = Configuration.Health;
            Health = MaxHealth;
        }
    }
}