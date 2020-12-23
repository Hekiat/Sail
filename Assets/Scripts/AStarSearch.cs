using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sail
{
    public class AStarSearch
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

                //Debug.Log("SORT " + elements[0].Item1 + " " + elements[elements.Count-1].Item1);
            }
        }


        public static List<TileCoord> search(TileCoord start, TileCoord goal)
        {
            //Debug.Log($"Start {start.Square.x}, {start.Square.y} Goal {goal.Square.x}, {goal.Square.y}");

            var frontier = new PriorityQueue<TileCoord>();

            frontier.add(start, 0f);

            var came_from = new Dictionary<TileCoord, TileCoord>();
            var cost_so_far = new Dictionary<TileCoord, float>();
            came_from[start] = start;
            cost_so_far[start] = 0;

            var board = GlobalManagers.board;

            while (frontier.isEmpty() == false)
            {
                var current = frontier.pop();
                if (current == goal)
                {
                    //Debug.Log($"Goal found {current.Square.x}, {current.Square.y}");
                    break;
                }

                var neighbors = board.neighbors(current);
                foreach (var next in neighbors)
                {
                    var new_cost = cost_so_far[current] + cost(current, next);

                    if (cost_so_far.ContainsKey(next) == false || new_cost < cost_so_far[next])
                    {
                        cost_so_far[next] = new_cost;

                        //var priority = new_cost + heuristic(next, goal);
                        var priority = new_cost + heuristic(start, next, goal);
                        frontier.add(next, priority);
                        came_from[next] = current;
                        //Debug.Log($"{next.Square.x}, {next.Square.y} inserted");
                    }
                }
            }

            // Reconstruct Path
            List<TileCoord> path = new List<TileCoord>();
            TileCoord id = goal;
            path.Add(id);

            while (id.Equals(start) == false)
            {
                id = came_from[id];
                path.Add(id);
            }

            path.Reverse();

            return path;
        }

        public static float cost(TileCoord a, TileCoord b)
        {
            return 1f;
        }

        public static float heuristic(TileCoord a, TileCoord b)
        {
            return Mathf.Abs(a.Square.x - b.Square.x) + Mathf.Abs(a.Square.y - b.Square.y);
        }

        public static float heuristic(TileCoord start, TileCoord current, TileCoord goal)
        {
            var h = heuristic(current, goal);
            var dx1 = current.Square.x - goal.Square.x;
            var dy1 = current.Square.y - goal.Square.y;
            var dx2 = start.Square.x - goal.Square.x;
            var dy2 = start.Square.y - goal.Square.y;
            var cross = Mathf.Abs(dx1 * dy2 - dx2 * dy1);
            return h + cross * 0.001f;
        }
    }
}