using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sail
{
    public class PriorityQueue<T>
    {
        private List<Tuple<float, T>> elements = new List<Tuple<float, T>>();

        bool dirty = false;

        public void add(T element, float priority)
        {
            elements.Add(new Tuple<float, T>(priority, element));
            dirty = true;
        }

        public T pop()
        {
            if (isEmpty())
            {
                return default(T);
            }

            if (dirty == true)
            {
                sort();
            }

            var elem = elements[0].Item2;
            elements.RemoveAt(0);

            return elem;
        }

        public bool isEmpty()
        {
            return elements.Count == 0;
        }

        private void sort()
        {
            dirty = false;
            elements.Sort((a, b) => a.Item1.CompareTo(b.Item1));
        }
    }
}