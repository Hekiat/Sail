using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class Projectile : MonoBehaviour
    {
        public Vector3 StartPosition;
        public Vector3 EndPosition;

        // m/s
        public float Speed = 1f;

        void Start()
        {

        }

        void Update()
        {
            var dir = EndPosition - transform.position;

            var delta = dir.normalized * Speed * Time.deltaTime;
            transform.position += delta;

            if (dir.magnitude < 0.5f)
            {
                Destroy(gameObject);
            }
        }
    }
}