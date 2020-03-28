using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class Unit : MonoBehaviour
    {
        public string UnitName;

        public TileCoord Coord { get; set; }

        public int Cooldown = 0;
        public int Health;

        public int MaxHealth;

        void Start()
        {

        }

        void Update()
        {

        }
    }
}