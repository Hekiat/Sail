﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public abstract class GraphBase<T>
    {
        public List<Node<T>> nodes { get; private set; } = new List<Node<T>>();
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

        public Func<T, T, float> cost = null;
    }

    public class MapGraph : GraphBase<Vector2>
    {
        public List<List<Edge>> edges { get; private set; } = new List<List<Edge>>();

        public override List<Node<Vector2>> neighbors(Node<Vector2> node)
        {
            var neighbors = new List<Node<Vector2>>();

            foreach (var e in edges[node.id])
            {
                if(nodes[e.v1].active == false)
                {
                    continue;
                }

                neighbors.Add(nodes[e.v1]);
            }

            return neighbors;
        }

        //public float cost(Node<T> a, Node<T> b)
        //{
        //    return 1f;
        //}

        public override Node<Vector2> addNode(Vector2 pos)
        {
            var n = new Node<Vector2>(pos, nodes.Count);
            nodes.Add(n);
            edges.Add(new List<Edge>());
            return n;
        }

        public void addEdge(Edge e)
        {
            edges[e.v0].Add(e);
        }

        public void setEdgeActive(Edge e, bool active)
        {
            e.active = active;
            var index = edges[e.v0].FindIndex((item) => item.isEqual(e));

            if(index == -1)
            {
                return;
            }

            edges[e.v0][index] = e;
        }
    }

    public struct Node<T>
    {
        public int id;
        public T pos;
        public bool active;

        public Node(T _pos, int _id)
        {
            pos = _pos;
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

    public class MapWidget : MonoBehaviour
    {
        // Prefabs
        public GameObject MapEventWidgetPrefab;
        public GameObject MapEventLinkWidgetPrefab;

        // Child Widgets
        public GameObject EventList;

        public int Depth { get; private set; }

        private List<MapEventWidget> MapEventWidgets = new List<MapEventWidget>();

        MapGraph Graph = new MapGraph();

        // Start is called before the first frame update
        void Start()
        {
            createNodes();
            createEdges();

            Graph.cost = (Vector2 a, Vector2 b) => {
                var delta = a - b;
                //delta.y = 0;
                delta.y /= 4f;
                return delta.magnitude;
            };

            computePaths();

            spawnEdges();
            spawnNodes();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void computePaths()
        {
            var acceptedNodes = new List<Node<Vector2>>();
            var acceptedEdges = new List<Edge>();

            for (int i=0; i<10; ++i)
            {
                var path = getShortestPath();
                var bypassedIndex = (int)UnityEngine.Random.Range(1, path.Count - 2);
                var bypassedNode = path[bypassedIndex];

                bypassedNode.active = false;
                Graph.nodes[bypassedNode.id] = bypassedNode;

                acceptedNodes.AddRange(path);

                for(int j=0; j<path.Count-1; ++j)
                {
                    acceptedEdges.Add(new Edge(path[j].id, path[j+1].id));
                }
            }

            // Update active status to the good value
            for (int i = 0; i < Graph.nodes.Count; ++i)
            {
                Graph.setNodeActive(Graph.nodes[i], false);
            }

            for (int i = 0; i < acceptedNodes.Count; ++i)
            {
                Graph.setNodeActive(acceptedNodes[i], true);
            }

            foreach(var el in Graph.edges)
            {
                for(int i=0; i<el.Count; ++i)
                {
                    Graph.setEdgeActive(el[i], false);
                }
            }

            for(int i=0; i<acceptedEdges.Count; ++i)
            {
                Graph.setEdgeActive(acceptedEdges[i], true);
            }
        }

        List<Node<Vector2>> getShortestPath()
        {
            AStarSearch<Vector2> aStar = new AStarSearch<Vector2>(Graph);
            aStar.heuristic = (Vector2 start, Vector2 current, Vector2 goal) => { return Math.Abs(start.x - goal.x) + Math.Abs(start.y - goal.y); };

            return aStar.getPathNodes(Graph.nodes[Graph.nodes.Count - 2], Graph.nodes[Graph.nodes.Count - 1]);
        }

        void debugDisplayPath(List<Node<Vector2>> path, Color color)
        {
            for (int i = 1; i < path.Count; ++i)
            {
                spawnEdge(path[i - 1].pos, path[i].pos, color);
            }
        }

        void createNodes()
        {
            var rootRect = EventList.GetComponent<RectTransform>().rect;

            #if false // keep ratio
            var screenMin = Mathf.Min(rootRect.width, rootRect.height);
            var screenSize = new Vector2(screenMin, screenMin);
            var screenHorizontalSafeRatio = 0.1f;
            #else
            var screenSize = new Vector2(rootRect.width, rootRect.height);
            var screenHorizontalSafeRatio = 0.1f;
            #endif

            var viewportSize = screenSize * new Vector2(1f - screenHorizontalSafeRatio * 2f, 1f);

            // Generate Random Nodes
            var min = -Vector2.one;
            var max = Vector2.one;
            float Radius = 0.20f;
            var positions = new PoissonSampling(Radius, min, max, 30, isPositionConstraintOK).get();

            for (var i = 0; i < positions.Count; ++i)
            {
                // rescale for [-1 ~ 1] to [0 ~ viewport size]
                positions[i] *= viewportSize / 2f;
                positions[i] += viewportSize / 2f;

                // offset safe zone
                positions[i] += screenSize * screenHorizontalSafeRatio;

                Graph.addNode(positions[i]);
            }

            // Add Start / Exit Nodes;
            var startPos = new Vector2(screenSize.x * screenHorizontalSafeRatio / 2f, screenSize.y / 2f);
            Graph.addNode(startPos);

            var exitPos = new Vector2(screenSize.x - screenSize.x * screenHorizontalSafeRatio / 2f, screenSize.y / 2f);
            Graph.addNode(exitPos);
        }

        bool isPositionConstraintOK(Vector2 pos)
        {
            // fit a circle
            if(pos.magnitude > 1f)
            {
                return false;
            }

            return true;
        }

        Vector2[] computeSuperTriangle(List<Vector2> samples)
        {
            var xmin = float.MaxValue;
            var xmax = float.MinValue;
            var ymin = float.MaxValue;
            var ymax = float.MinValue;

            foreach (var s in samples)
            {
                xmin = Mathf.Min(xmin, s.x);
                xmax = Mathf.Max(xmax, s.x);
                ymin = Mathf.Min(ymin, s.y);
                ymax = Mathf.Max(ymax, s.y);
            }

            var width = xmax - xmin;
            var height = ymax - ymin;
            var hwidth = width / 2f;

            var p0 = new Vector2(xmin - hwidth, ymin);
            var p1 = new Vector2(xmax + hwidth, ymin);
            var p2 = new Vector2(xmin + hwidth, 3000);// height * Mathf.Tan(height / hwidth));

            return new Vector2[3] { p0, p1, p2 };
        }

        //bowyer watson
        List<Triangle> triangulate(List<Vector2> points)
        {
            List<Triangle> triangles = new List<Triangle>();

            if (points.Count < 3)
            {
                return triangles;
            }

            // Init Delaunay triangulation.
            var stPoints = computeSuperTriangle(points);
            var stPointStartIndex = points.Count;

            // Add to points list
            foreach(var p in stPoints)
            {
                points.Add(p);
            }

            var st = new Triangle(stPointStartIndex, stPointStartIndex + 1, stPointStartIndex + 2, points);
            triangles.Add(st);

            //foreach (var p in points)
            for(int pi=0; pi<points.Count; ++pi)
            {
                var p = points[pi];

                List<Edge> polygon = new List<Edge>(); // bad polygon
                List<Triangle> badTriangles = new List<Triangle>();

                foreach (var t in triangles)
                {
                    // Check if the point is inside the triangle circumcircle.
                    if (t.isContainedInCircle(p))
                    {
                        badTriangles.Add(t);
                    }
                }

                for (int ti = 0; ti < badTriangles.Count; ++ti)
                {
                    var t = badTriangles[ti];
                    foreach (var e in t.edges)
                    {
                        // check if the edge is inside the polygon that needs to be updated
                        bool found = false;
                        for (int ti2 = 0; ti2 < badTriangles.Count; ++ti2)
                        {
                            if (ti == ti2)
                            {
                                continue;
                            }

                            var other = badTriangles[ti2];

                            if (e.isEqual(other.e0) || e.isEqual(other.e1) || e.isEqual(other.e2))
                            {
                                found = true;
                                break;
                            }
                        }

                        // if false it's a polygon boundary
                        if (found == false)
                        {
                            polygon.Add(e);
                        }
                    }
                }

                foreach (var bt in badTriangles)
                {
                    triangles.Remove(bt);
                }

                foreach (var e in polygon)
                {
                    triangles.Add(new Triangle(e.v0, e.v1, pi, points));
                }
            }

            for (int i = triangles.Count - 1; i >= 0; --i)
            {
                var t = triangles[i];

                var hasPointInSuper = false;
                foreach (var tp in t.vertexIndex)
                {
                    foreach (var stp in st.vertexIndex)
                    {
                        if (tp == stp)
                        {
                            hasPointInSuper = true;
                            break;
                        }
                    }

                    if (hasPointInSuper)
                    {
                        break;
                    }
                }

                if (hasPointInSuper)
                {
                    triangles.RemoveAt(i);
                }
            }

            // remove super triangle
            points.RemoveAt(stPointStartIndex + 2);
            points.RemoveAt(stPointStartIndex + 1);
            points.RemoveAt(stPointStartIndex);

            return triangles;
        }

        void createEdges()
        {
            // prepare data
            List<Vector2> positions = new List<Vector2>();
            foreach(var n in Graph.nodes)
            {
                positions.Add(n.pos);
            }

            List<Triangle> triangles = triangulate(positions);
            
            List<Edge> edges = new List<Edge>();
            foreach (var t in triangles)
            {
                foreach(var e in t.edges)
                {
                    if (edges.Exists((elem) => elem.isEqual(e)))
                    {
                        continue;
                    }

                    bool reverse = positions[e.v0].x > positions[e.v1].x;

                    var v0 = reverse ? e.v1 : e.v0;
                    var v1 = reverse ? e.v0 : e.v1;

                    var newEdge = new Edge(v0, v1);

                    edges.Add(newEdge);
                    Graph.addEdge(newEdge);
                }
            }
        }

        void spawnEdge(Vector2 p0, Vector2 p1, Color? color = null)
        {
            Vector3 differenceVector = p1 - p0;

            //if (differenceVector.magnitude > Radius * 3)
            //{
            //    return;
            //}

            var eventLinkWidget = Instantiate(MapEventLinkWidgetPrefab, EventList.transform);
            var t = eventLinkWidget.GetComponent<RectTransform>();
            t.anchorMin = new Vector2(0f, 0f);
            t.anchorMax = new Vector2(0f, 0f);

            var img = t.GetComponent<UnityEngine.UI.Image>();
            img.color = color ?? Color.black;

            var canvas = img.canvas;
            float lineWidth = 5f;
            t.sizeDelta = new Vector2(differenceVector.magnitude, lineWidth);
            t.pivot = new Vector2(0, 0.5f);
            t.anchoredPosition = new Vector3(p0.x, p0.y, 0f);
            float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
            t.localRotation = Quaternion.Euler(0, 0, angle);
        }

        void spawnEdges()
        {
            foreach(var eList in Graph.edges)
            {
                foreach(var e in eList)
                {
                    if(    e.active == false
                        || Graph.nodes[e.v0].active == false
                        || Graph.nodes[e.v1].active == false)
                    {
                        continue;
                    }

                    spawnEdge(Graph.nodes[e.v0].pos, Graph.nodes[e.v1].pos);
                }
            }
        }

        void spawnNode(Vector2 pos)
        {
            var eventWidget = Instantiate(MapEventWidgetPrefab, EventList.transform);
            var t = eventWidget.GetComponent<RectTransform>();
            t.anchorMin = new Vector2(0f, 0f);
            t.anchorMax = new Vector2(0f, 0f);
            t.anchoredPosition = pos;
        }

        void spawnNodes()
        {
            foreach (var n in Graph.nodes)
            {
                if(n.active)
                {
                    spawnNode(n.pos);
                }
            }
        }

        struct Triangle
        {
            public int[] vertexIndex;
            public Edge[] edges;

            public int v0 { get { return vertexIndex[0]; } set { vertexIndex[0] = value; } }
            public int v1 { get { return vertexIndex[1]; } set { vertexIndex[1] = value; } }
            public int v2 { get { return vertexIndex[2]; } set { vertexIndex[2] = value; } }

            public Edge e0 { get { return edges[0]; } set { edges[0] = value; } }
            public Edge e1 { get { return edges[1]; } set { edges[1] = value; } }
            public Edge e2 { get { return edges[2]; } set { edges[2] = value; } }

            public Vector2 circlePos;
            public float circleRad;

            public Triangle(int _v0, int _v1, int _v2, List<Vector2> points)
            {
                vertexIndex = new int[3];
                edges = new Edge[3];

                circlePos = Vector2.zero;
                circleRad = 0f;

                v0 = _v0;
                v1 = _v1;
                v2 = _v2;

                e0 = new Edge(v0, v1);
                e1 = new Edge(v1, v2);
                e2 = new Edge(v0, v2);

                updateCircumcircle(points);
            }

            public bool isContainedInCircle(Vector2 p)
            {
                var dist = (circlePos.x - p.x) * (circlePos.x - p.x) + (circlePos.y - p.y) * (circlePos.y - p.y);
                return (dist - circleRad) <= float.Epsilon;
            }

            private void updateCircumcircle(List<Vector2> points)
            {
                var p0 = points[v0];
                var p1 = points[v1];
                var p2 = points[v2];

                var ax = p1.x - p0.x;
                var ay = p1.y - p0.y;
                var bx = p2.x - p0.x;
                var by = p2.y - p0.y;

                var m = p1.x * p1.x - p0.x * p0.x + p1.y * p1.y - p0.y * p0.y;
                var u = p2.x * p2.x - p0.x * p0.x + p2.y * p2.y - p0.y * p0.y;
                var s = 1f / (2f * (ax * by - ay * bx));

                circlePos.x = ((p2.y - p0.y) * m + (p0.y - p1.y) * u) * s;
                circlePos.y = ((p0.x - p2.x) * m + (p1.x - p0.x) * u) * s;

                var dx = p0.x - circlePos.x;
                var dy = p0.y - circlePos.y;
                circleRad = dx * dx + dy * dy;
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

                if(Graph == null || StartNode == null || GoalNode == null)
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

                if(Graph == null || StartNode == null || GoalNode == null)
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

                var frontier = new AStarSearch.PriorityQueue<Node<T>>();
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
                        var costToNext = Graph.cost == null ? 1f : Graph.cost(current.pos, next.pos);
                        float newCost = costSoFar[current.id] + costToNext;
                        if (!costSoFar.ContainsKey(next.id) || newCost < costSoFar[next.id])
                        {
                            costSoFar[next.id] = newCost;
                            float priority = newCost + heuristic(start.pos, next.pos, goal.pos);
                            frontier.add(next, priority);
                            cameFrom[next.id] = current;
                        }
                    }
                }
            }

            private List<Node<T>> extractNodes(Node<T> start, Node<T> goal)
            {
                var path = new List<Node<T>>();
                var id = goal;
                path.Add(id);

                while (id.Equals(start) == false)
                {
                    id = cameFrom[id.id];
                    path.Add(id);
                }

                path.Reverse();

                return path;
            }

            private List<T> extractValue(Node<T> start, Node<T> goal)
            {
                var path = new List<T>();
                var id = goal;
                path.Add(id.pos);

                while (id.Equals(start) == false)
                {
                    id = cameFrom[id.id];
                    path.Add(id.pos);
                }

                path.Reverse();

                return path;
            }

            void updateStartGoalNodes(T start, T goal)
            {
                var startIndex = Graph.nodes.FindIndex((item) => item.pos.Equals(start));
                var goalIndex = Graph.nodes.FindIndex((item) => item.pos.Equals(goal));

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
}