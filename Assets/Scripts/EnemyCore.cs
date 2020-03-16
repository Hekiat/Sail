using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class EnemyCore : MonoBehaviour
    {
        public EnemyConfiguration Configuration = null;

        public int Cooldown = 0;
        public int Health;

        // Configure data
        public string CharacterName;
        public int MaxHealth;

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

            CharacterName = Configuration.Name;
            MaxHealth = Configuration.Health;
            Health = MaxHealth;
        }
    }
}