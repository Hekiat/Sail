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
        private int[] StepInstCount = null;

        // Start is called before the first frame update
        void Start()
        {
            //generate(5);
            var samples = poissonSampling();
            
            var graph = new Graph<Vector2>();
            foreach(var s in samples)
            {
                graph.addNode(s);
            }

            var triangles = triangulate(samples);
            spawnEdges(triangles);
            spawnNodes(graph.nodes);
        }

        // Update is called once per frame
        void Update()
        {

        }

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

        List<Vector2> poissonSampling()
        {
            List<Vector2> samples = new List<Vector2>();
            List<bool> isSampleActive = new List<bool>();

            Random.InitState(System.DateTime.Now.Second);

            var rootRect = EventList.GetComponent<RectTransform>().rect;

            samples.Add(new Vector2(rootRect.width / 2, rootRect.height / 2));
            // Entry / Exit node
            samples.Add(new Vector2(0, rootRect.height / 2));
            samples.Add(new Vector2(rootRect.width, rootRect.height / 2));

            isSampleActive.Add(true);
            isSampleActive.Add(true);
            isSampleActive.Add(true);

            int activeSampleIndex = -1;
            while ((activeSampleIndex = isSampleActive.FindIndex(e => e == true)) != -1)
            {
                var activeSample = samples[activeSampleIndex];

                for (int i = 0; i < CandidateMaxSample; i++)
                {
                    var c = candidate(activeSample);

                    if (isCandidateValid(c, samples))
                    {
                        samples.Add(c);
                        isSampleActive.Add(true);
                        break;
                    }

                    if (i == CandidateMaxSample - 1)
                    {
                        isSampleActive[activeSampleIndex] = false;
                    }
                }
            }

            return samples;
        }

        void spawnEdge(Edge e)
        {
            Vector3 differenceVector = e.p1 - e.p0;

            if (differenceVector.magnitude > Radius * 3)
            {
                return;
            }

            var eventLinkWidget = Instantiate(MapEventLinkWidgetPrefab, EventList.transform);
            var t = eventLinkWidget.GetComponent<RectTransform>();
            t.anchorMin = new Vector2(0f, 0f);
            t.anchorMax = new Vector2(0f, 0f);

            var canvas = t.GetComponent<UnityEngine.UI.Image>().canvas;
            float lineWidth = 5f;
            t.sizeDelta = new Vector2(differenceVector.magnitude, lineWidth);
            t.pivot = new Vector2(0, 0.5f);
            t.anchoredPosition = new Vector3(e.p0.x, e.p0.y, 0f);
            float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
            t.localRotation = Quaternion.Euler(0, 0, angle);

            Debug.Log("i " + (new Vector3(e.p0.x, e.p0.y, 0f)));
            Debug.Log(t.position);
        }


        readonly float Radius = 150f;
        readonly float SafeRadius = 300f;
        readonly int CandidateMaxSample = 30;

        Vector2 candidate(Vector2 p)
        {
            var angle = Random.Range(0, Mathf.PI * 2f);
            var radius = Random.Range(Radius, SafeRadius);

            return p + new Vector2(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
        }

        bool isCandidateValid(Vector2 candidate, List<Vector2> samples)
        {
            // in screen
            if (isPositionConstraintOK(candidate) == false)
            {
                return false;
            }

            foreach (var s in samples)
            {
                if ((s - candidate).magnitude < Radius)
                {
                    return false;
                }
            }

            return true;
        }

        Triangle superTriangle(List<Vector2> samples)
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

            return new Triangle(p0, p1, p2);
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
            var st = superTriangle(points);
            triangles.Add(st);

            foreach (var p in points)
            {
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


                for (int ti = 0; ti < badTriangles.Count; ti++)
                {
                    var t = badTriangles[ti];
                    foreach (var e in t.e)
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
                    triangles.Add(new Triangle(e.p0, e.p1, p));
                }
            }

            for (int i = triangles.Count - 1; i >= 0; --i)
            {
                var t = triangles[i];

                var hasPointInSuper = false;
                foreach (var tp in t.p)
                {
                    foreach (var stp in st.p)
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
            return triangles;
        }

        void spawnEdges(List<Triangle> triangles)
        {
            Graph<Vector2> graph = new Graph<Vector2>();

            foreach (var t in triangles)
            {
                foreach (var e in t.e)
                {
                    spawnEdge(e);
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


        class Edge
        {
            public Vector2 p0;
            public Vector2 p1;

            public Edge(Vector2 _p0, Vector2 _p1)
            {
                p0 = _p0;
                p1 = _p1;
            }

            public bool isEqual(Edge other)
            {
                return (other.p0 == p0 && other.p1 == p1) || (other.p0 == p1 && other.p1 == p0);
            }
        }

        class Triangle
        {
            public Vector2[] p = new Vector2[3];

            public Vector2 p0 { get { return p[0]; } set { p[0] = value; } }
            public Vector2 p1 { get { return p[1]; } set { p[1] = value; } }
            public Vector2 p2 { get { return p[2]; } set { p[2] = value; } }


            public Edge[] e = new Edge[3];

            public Edge e0 { get { return e[0]; } set { e[0] = value; } }
            public Edge e1 { get { return e[1]; } set { e[1] = value; } }
            public Edge e2 { get { return e[2]; } set { e[2] = value; } }

            public Vector2 circlePos;
            public float circleRad;

            public Triangle(Vector2 _p0, Vector2 _p1, Vector2 _p2)
            {
                p0 = _p0;
                p1 = _p1;
                p2 = _p2;

                e0 = new Edge(p0, p1);
                e1 = new Edge(p1, p2);
                e2 = new Edge(p0, p2);

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

            public bool isContainedInCircle(Vector2 p)
            {
                var dist = (circlePos.x - p.x) * (circlePos.x - p.x) + (circlePos.y - p.y) * (circlePos.y - p.y);
                return (dist - circleRad) <= float.Epsilon;
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
            public List<Node<T>> nodes = new List<Node<T>>();
            public Dictionary<Node<T>, Node<T>[]> edges = new Dictionary<Node<T>, Node<T>[]>();

            public Node<T>[] neighbors(Node<T> id)
            {
                return edges[id];
            }

            public float cost(Node<T> a, Node<T> b)
            {
                return 1f;
            }

            public Node<T> addNode(T pos)
            {
                var n = new Node<T>(pos, nodes.Count);
                nodes.Add(n);
                return n;
            }
        }

        public class AStarSearch<T>
        {
            public Dictionary<Node<T>, Node<T>> cameFrom = new Dictionary<Node<T>, Node<T>>();
            public Dictionary<Node<T>, float> costSoFar = new Dictionary<Node<T>, float>();

            public System.Func<T, T, float> heuristic = null;

            // Note: a generic version of A* would abstract over Location and
            // also Heuristic
            //static public double Heuristic(Node<T> a, Node<T> b)
            //{
            //    return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
            //}

            public AStarSearch(Graph<T> graph, Node<T> start, Node<T> goal)
            {
                var frontier = new AStarSearch.PriorityQueue<Node<T>>();

                frontier.add(start, 0);

                cameFrom[start] = start;
                costSoFar[start] = 0;

                while (frontier.isEmpty() == false)
                {
                    var current = frontier.pop();

                    if (current.Equals(goal))
                    {
                        break;
                    }

                    foreach (var next in graph.neighbors(current))
                    {
                        float newCost = costSoFar[current] + graph.cost(current, next);
                        if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                        {
                            costSoFar[next] = newCost;
                            float priority = newCost + heuristic(next.pos, goal.pos);
                            frontier.add(next, priority);
                            cameFrom[next] = current;
                        }
                    }
                }
            }
        }
    }
}