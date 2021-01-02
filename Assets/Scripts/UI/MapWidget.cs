﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
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
                spawnEdge(path[i - 1].value, path[i].value, color);
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

        void createEdges()
        {
            // prepare data
            List<Vector2> positions = new List<Vector2>();
            foreach(var n in Graph.nodes)
            {
                positions.Add(n.value);
            }

            List<Triangle> triangles = Triangulate.run(positions);
            
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

                    spawnEdge(Graph.nodes[e.v0].value, Graph.nodes[e.v1].value);
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
                    spawnNode(n.value);
                }
            }
        }
    }
}