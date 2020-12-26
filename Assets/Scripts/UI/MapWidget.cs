using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class MapWidget : MonoBehaviour
    {
        // Prefabs
        public GameObject MapEventWidgetPrefab;
        public GameObject MapEventLinkWidgetPrefab;

        // Child Widgets
        public GameObject EventList;

        public int Depth { get; private set; }

        private List<MapEventWidget> MapEventWidgets = new List<MapEventWidget>();

        // Start is called before the first frame update
        void Start()
        {
            var rootRect = EventList.GetComponent<RectTransform>().rect;
            var min = Vector2.zero;
            var max = new Vector2(rootRect.width, rootRect.height);
            var samples = new PoissonSampling(Radius, min, max, 30, isPositionConstraintOK).get();
            
            var graph = new Graph<Vector2>();
            foreach(var s in samples)
            {
                graph.addNode(s);
            }

            var triangles = triangulate(samples);
            var edges = filterEdges(triangles, samples);

            foreach(var e in edges)
            {
                graph.addEdge(e);
            }

            spawnEdges(graph);
            spawnNodes(graph.nodes);

            AStarSearch<Vector2> aStar = new AStarSearch<Vector2>();
            aStar.heuristic = (Vector2 a, Vector2 b) => { return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y); };
            aStar.run(graph, graph.nodes[1], graph.nodes[2]);
            var path = aStar.computePath(graph.nodes[1], graph.nodes[2]);

            for(int i=1; i<path.Count; ++i)
            {
                spawnEdge(path[i - 1].pos, path[i].pos, Color.red);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        float Radius = 150f;

        // adjust later
        bool isPositionConstraintOK(Vector2 pos)
        {
            var rootRect = EventList.GetComponent<RectTransform>().rect;
            if (pos.x < Radius || pos.x > rootRect.width - Radius
                || pos.y < Radius || pos.y > rootRect.height - Radius)
            {
                return false;
            }

            return true;
        }

        

        void spawnEdge(Vector2 p0, Vector2 p1, Color? color = null)
        {
            Vector3 differenceVector = p1 - p0;

            if (differenceVector.magnitude > Radius * 3)
            {
                return;
            }

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

        List<Edge> filterEdges(List<Triangle> triangles, List<Vector2> points)
        {
            List<Edge> edges = new List<Edge>();
            foreach (var t in triangles)
            {
                foreach(var e in t.edges)
                {
                    if (edges.Exists((elem) => elem.isEqual(e)))
                    {
                        continue;
                    }

                    bool reverse = points[e.v0].x > points[e.v1].x;

                    var v0 = reverse ? e.v1 : e.v0;
                    var v1 = reverse ? e.v0 : e.v1;

                    edges.Add(new Edge(v0, v1));
                }
            }

            return edges;
        }

        void spawnEdges(Graph<Vector2> graph)
        {
            foreach (var eList in graph.edges)
            {
                foreach(var e in eList)
                {
                    spawnEdge(graph.nodes[e.v0].pos, graph.nodes[e.v1].pos);
                }
            }
        }

        void spawnNodes(List<Node<Vector2>> nodes)
        {
            // generate game objects
            foreach (var n in nodes)
            {
                var eventWidget = Instantiate(MapEventWidgetPrefab, EventList.transform);
                var t = eventWidget.GetComponent<RectTransform>();
                t.anchorMin = new Vector2(0f, 0f);
                t.anchorMax = new Vector2(0f, 0f);
                t.anchoredPosition = n.pos;
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

        public struct Node<T>
        {
            public int id;
            public T pos;

            public Node(T _pos, int _id)
            {
                pos = _pos;
                id = _id;
            }
        }

        public class Graph<T>
        {
            public List<Node<T>> nodes { get; private set; } = new List<Node<T>>();
            public List<List<Edge>> edges { get; private set; } = new List<List<Edge>>();

            public List<Node<T>> neighbors(Node<T> node)
            {
                var neighbors = new List<Node<T>>();

                foreach(var e in edges[node.id])
                {
                    neighbors.Add(nodes[e.v1]);
                }

                return neighbors;
            }

            public float cost(Node<T> a, Node<T> b)
            {
                return 1f;
            }

            public Node<T> addNode(T pos)
            {
                var n = new Node<T>(pos, nodes.Count);
                nodes.Add(n);
                edges.Add(new List<Edge>());
                return n;
            }

            public void addEdge(Edge e)
            {
                edges[e.v0].Add(e);
            }
        }

        public class AStarSearch<T>
        {
            public Dictionary<Node<T>, Node<T>> cameFrom = new Dictionary<Node<T>, Node<T>>();
            public Dictionary<Node<T>, float> costSoFar = new Dictionary<Node<T>, float>();
            public Func<T, T, float> heuristic = null;

            public void run(Graph<T> graph, Node<T> start, Node<T> goal)
            {
                var frontier = new AStarSearch.PriorityQueue<Node<T>>();
                frontier.add(start, 0);

                cameFrom[start] = start;
                costSoFar[start] = 0;

                while (frontier.isEmpty() == false)
                {
                    var current = frontier.pop();
                    Console.WriteLine("current " + current.id);
                    if (current.Equals(goal))
                    {
                        break;
                    }

                    foreach (var next in graph.neighbors(current))
                    {
                        float newCost = costSoFar[current] + graph.cost(current, next);
                        if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                        {
                            Console.WriteLine("current neighbor " + next.id + " " + newCost);
                            costSoFar[next] = newCost;
                            float priority = newCost + heuristic(next.pos, goal.pos);
                            frontier.add(next, priority);
                            cameFrom[next] = current;
                        }
                    }
                }
            }

            public List<Node<T>> computePath(Node<T> start, Node<T> goal)
            {
                var path = new List<Node<T>>();

                var id = goal;
                path.Add(id);

                while (id.Equals(start) == false)
                {
                    id = cameFrom[id];
                    path.Add(id);
                }

                path.Reverse();

                return path;
            }
        }
    }
}