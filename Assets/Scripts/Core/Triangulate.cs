using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public struct Triangle
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

    public static class Triangulate
    {
        private static Vector2[] computeSuperTriangle(List<Vector2> samples)
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
            var p2 = new Vector2(xmin + hwidth, 3000);// height * Mathf.Tan(height / hwidth)); // need to be fixed

            return new Vector2[3] { p0, p1, p2 };
        }

        //bowyer watson
        public static List<Triangle> run(List<Vector2> points)
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
            foreach (var p in stPoints)
            {
                points.Add(p);
            }

            var st = new Triangle(stPointStartIndex, stPointStartIndex + 1, stPointStartIndex + 2, points);
            triangles.Add(st);

            //foreach (var p in points)
            for (int pi = 0; pi < points.Count; ++pi)
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

    }
}