using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class WorldMapItem : MonoBehaviour
    {
        public Vector3 Position => _Renderer.bounds.center;

        private Renderer _Renderer = null;

        // Start is called before the first frame update
        void Start()
        {
            _Renderer = GetComponent<Renderer>();
            Debug.Log("" + _Renderer);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}