using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class Unit : MonoBehaviour, IDamageable, IHealable, IShieldable
    {
        public string UnitName;

        public TileCoord Coord { get; set; }

        public Animator Animator { get; private set; } = null;

        public int Cooldown = 0;
        public int Health { get; protected set; }
        public int Shield { get; protected set; }

        public int MaxHealth { get; protected set; }

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
        void IHealable.Heal(int healAmount)
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

        void IShieldable.Shield(int shieldAmount)
        {
            Shield += shieldAmount;
            Health = Mathf.Max(Health, 0);

            WorldUIController.addFloatingText(shieldAmount.ToString(), transform, Color.cyan);
        }
    }
}