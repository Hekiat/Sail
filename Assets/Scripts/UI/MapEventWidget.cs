using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class MapEventWidget : MonoBehaviour
    {
        public Vector2Int Index { get; set; }

        List<MapEventWidget> NextEvent = new List<MapEventWidget>();

        void Start()
        {

        }

        void Update()
        {

        }

        public void addNextEvent(MapEventWidget mew)
        {
            NextEvent.Add(mew);
        }
    }
}


