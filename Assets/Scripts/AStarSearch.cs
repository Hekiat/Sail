using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sail
{
    public abstract class GraphBase<T>
    {
        public List<Node<T>> nodes { get; private set; } = new List<Node<T>>();
        public Func<T, T, float> cost = null;

        public abstract List<Node<T>> neighbors(Node<T> node);

        public virtual Node<T> addNode(T pos)
        {
            var n = new Node<T>(pos, nodes.Count);
            nodes.Add(n);
            return n;
        }

        public virtual void setNodeActive(Node<T> n, bool active)
        {
            n.active = active;
            nodes[n.id] = n;
        }

        public virtual void clear()
        {
            nodes.Clear();
        }
    }

    public struct Node<T>
    {
        public int id;
        public T value;
        public bool active;

        public Node(T _pos, int _id)
        {
            value = _pos;
            id = _id;
            active = true;
        }
    }

    public struct Edge
    {
        public int v0;
        public int v1;
        public bool active;

        public Edge(int _v0, int _v1)
        {
            v0 = _v0;
            v1 = _v1;
            active = true;
        }

        public bool isEqual(Edge other)
        {
            return (other.v0 == v0 && other.v1 == v1) || (other.v0 == v1 && other.v1 == v0);
        }
    }

    public class AStarSearch<T> where T : struct
    {
        public Dictionary<int, Node<T>> cameFrom = new Dictionary<int, Node<T>>();
        public Dictionary<int, float> costSoFar = new Dictionary<int, float>();
        public Func<T, T, T, float> heuristic = null;

        public GraphBase<T> Graph { get; private set; } = null;
        public Node<T>? StartNode { get; private set; } = null;
        public Node<T>? GoalNode { get; private set; } = null;

        public AStarSearch(GraphBase<T> graph)
        {
            Graph = graph;
        }

        void clear()
        {
            cameFrom.Clear();
            costSoFar.Clear();
        }

        public List<T> getPath(T start, T goal)
        {
            updateStartGoalNodes(start, goal);

            if (Graph == null || StartNode == null || GoalNode == null)
            {
                return new List<T>();
            }

            run(StartNode.Value, GoalNode.Value);

            return extractValue(StartNode.Value, GoalNode.Value);
        }

        public List<T> getPath(Node<T> start, Node<T> goal)
        {
            StartNode = start;
            GoalNode = goal;

            if (Graph == null || StartNode == null || GoalNode == null)
            {
                return new List<T>();
            }

            run(StartNode.Value, GoalNode.Value);

            return extractValue(StartNode.Value, GoalNode.Value);
        }

        public List<Node<T>> getPathNodes(T start, T goal)
        {
            updateStartGoalNodes(start, goal);

            if (Graph == null || StartNode == null || GoalNode == null)
            {
                return new List<Node<T>>();
            }

            run(StartNode.Value, GoalNode.Value);

            return extractNodes(StartNode.Value, GoalNode.Value);
        }

        public List<Node<T>> getPathNodes(Node<T> start, Node<T> goal)
        {
            StartNode = start;
            GoalNode = goal;

            if (Graph == null || StartNode == null || GoalNode == null)
            {
                return new List<Node<T>>();
            }

            run(StartNode.Value, GoalNode.Value);

            return extractNodes(StartNode.Value, GoalNode.Value);
        }

        private void run(Node<T> start, Node<T> goal)
        {
            clear();

            var frontier = new PriorityQueue<Node<T>>();
            frontier.add(start, 0);

            cameFrom[start.id] = start;
            costSoFar[start.id] = 0;

            while (frontier.isEmpty() == false)
            {
                var current = frontier.pop();

                if (current.Equals(goal))
                {
                    break;
                }

                foreach (var next in Graph.neighbors(current))
                {
                    var costToNext = Graph.cost == null ? 1f : Graph.cost(current.value, next.value);
                    float newCost = costSoFar[current.id] + costToNext;
                    if (!costSoFar.ContainsKey(next.id) || newCost < costSoFar[next.id])
                    {
                        costSoFar[next.id] = newCost;
                        float priority = newCost + heuristic(start.value, next.value, goal.value);
                        frontier.add(next, priority);
                        cameFrom[next.id] = current;
                    }
                }
            }
        }

        private List<Node<T>> extractNodes(Node<T> start, Node<T> goal)
        {
            var path = new List<Node<T>>();
            var current = goal;
            path.Add(current);

            while (current.Equals(start) == false)
            {
                current = cameFrom[current.id];
                path.Add(current);
            }

            path.Reverse();

            return path;
        }

        private List<T> extractValue(Node<T> start, Node<T> goal)
        {
            var path = new List<T>();
            var id = goal;
            path.Add(id.value);

            while (id.Equals(start) == false)
            {
                id = cameFrom[id.id];
                path.Add(id.value);
            }

            path.Reverse();

            return path;
        }

        void updateStartGoalNodes(T start, T goal)
        {
            var startIndex = Graph.nodes.FindIndex((item) => item.value.Equals(start));
            var goalIndex = Graph.nodes.FindIndex((item) => item.value.Equals(goal));

            if (startIndex == -1 || goalIndex == -1)
            {
                Debug.Log("AStar run with unfound value node.");
                StartNode = null;
                GoalNode = null;
                return;
            }

            StartNode = Graph.nodes[startIndex];
            GoalNode = Graph.nodes[goalIndex];
        }
    }
}