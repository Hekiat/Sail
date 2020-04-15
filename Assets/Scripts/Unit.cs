using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class Unit : MonoBehaviour
    {
        public string UnitName;

        public TileCoord Coord { get; set; }

        public Animator Animator { get; private set; } = null;

        public int Cooldown = 0;
        public int Health;

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
    }
}