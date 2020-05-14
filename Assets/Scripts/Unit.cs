using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class Unit : MonoBehaviour, IDamageable
    {
        public string UnitName;

        public TileCoord Coord { get; set; }

        public Animator Animator { get; private set; } = null;

        public int Cooldown = 0;
        public int Health;
        public int Shield;

        public int MaxHealth;

        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        void Start()
        {

        }

        void Update()
        {

        }

        // Interface

        //  IDamageable
        int IDamageable.Health { get { return Health; } }

        void IDamageable.Heal(int healAmount)
        {
            Health += healAmount;
            Health = Mathf.Min(Health, MaxHealth);

            WorldUIController.addFloatingText(healAmount.ToString(), transform, Color.green);
        }

        void IDamageable.Damage(int damage)
        {
            Health -= damage;
            Health = Mathf.Max(Health, 0);

            WorldUIController.addFloatingText(damage.ToString(), transform, Color.red);
        }
    }
}