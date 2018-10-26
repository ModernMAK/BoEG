using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Analytics;

namespace Graphing
{
    public partial class Graph<NodeData>
    {
        public Graph()
        {
            Nodes = new List<Node>();
            HalfEdges = new List<HalfEdge>();
            Edges = new List<Edge>();
            Polygons = new List<Polygon>();
        }

        public List<Node> Nodes { get; private set; }
        public List<HalfEdge> HalfEdges { get; private set; }
        public List<Edge> Edges { get; private set; }
        public List<Polygon> Polygons { get; private set; }


        public class GraphAsserter
        {
            public GraphAsserter()
            {
                Nodes = new NodeAssertions();
                HalfEdges = new HalfedgeAssertions();
                Polygons = new PolygonAssertions();
            }

            public NodeAssertions Nodes { get; private set; }
            public HalfedgeAssertions HalfEdges { get; private set; }
            public PolygonAssertions Polygons { get; private set; }

            public class MissingInvalidHelper
            {
                public int Missing { get; set; }
                public int Invalid { get; set; }
            }

            public class BaseAssertions
            {
                public int Passed { get; protected set; }
                public int Failed { get; protected set; }

                public int Total
                {
                    get { return Passed + Failed; }
                }

                protected static bool UtilUpdate<T>(MissingInvalidHelper helper, T item, ICollection<T> container)
                {
                    bool failed = false;
                    if (item == null)
                    {
                        helper.Missing++;
                        failed = true;
                    }

                    if (!container.Contains(item))
                    {
                        helper.Invalid++;
                        failed = true;
                    }

                    return failed;
                }
            }

            public class NodeAssertions : BaseAssertions
            {
                public NodeAssertions()
                {
                    Halfedges = new MissingInvalidHelper();
                }

                public MissingInvalidHelper Halfedges { get; private set; }

                public void Update(Node node)
                {
                    var failed = UtilUpdate(Halfedges, node.Edge, node.Graph.HalfEdges);
                    if (failed)
                        Failed++;
                    else
                        Passed++;
                }
            }

            public class HalfedgeAssertions : BaseAssertions
            {
                public HalfedgeAssertions()
                {
                    Twins = new MissingInvalidHelper();
                    Origins = new MissingInvalidHelper();
                    Nexts = new MissingInvalidHelper();
                    Previous = new MissingInvalidHelper();
                    Polygons = new MissingInvalidHelper();
                    Pairs = new MissingInvalidHelper();
                }


                public MissingInvalidHelper Twins { get; private set; }
                public MissingInvalidHelper Origins { get; private set; }
                public MissingInvalidHelper Nexts { get; private set; }
                public MissingInvalidHelper Previous { get; private set; }
                public MissingInvalidHelper Pairs { get; private set; }
                public MissingInvalidHelper Polygons { get; private set; }


                public void Update(HalfEdge edge)
                {
                    var failed = UtilUpdate(Twins, edge.Twin, edge.Graph.HalfEdges);
                    failed |= UtilUpdate(Nexts, edge.Next, edge.Graph.HalfEdges);
                    failed |= UtilUpdate(Previous, edge.Previous, edge.Graph.HalfEdges);

                    failed |= UtilUpdate(Origins, edge.Origin, edge.Graph.Nodes);

                    failed |= UtilUpdate(Pairs, edge.Pair, edge.Graph.Edges);

                    failed |= UtilUpdate(Polygons, edge.Polygon, edge.Graph.Polygons);

                    if (failed)
                        Failed++;
                    else
                        Passed++;
                }
            }

            public class PolygonAssertions : BaseAssertions
            {
                public PolygonAssertions()
                {
                    Halfedges = new MissingInvalidHelper();
                }

                public MissingInvalidHelper Halfedges { get; private set; }

                public void Update(Polygon poly)
                {
                    var failed = UtilUpdate(Halfedges, poly.Edge, poly.Graph.HalfEdges);
                    if (failed)
                        Failed++;
                    else
                        Passed++;
                }
            }

            public void Update(Graph<NodeData> graph)
            {
                foreach (var node in graph.Nodes)
                {
                    Nodes.Update(node);
                }

                foreach (var halfedge in graph.HalfEdges)
                {
                    HalfEdges.Update(halfedge);
                }

                foreach (var polygon in graph.Polygons)
                {
                    Polygons.Update(polygon);
                }
            }
        }


        public string DebugOutput()
        {
            var assesment = new GraphAsserter();
            assesment.Update(this);

            string nodeString =
                "Nodes ({0}) - P({1}) - F({2})\n" +
                "\tEdges - M({3}) - I({4})\n";

            string nodeStringFull = string.Format(nodeString,
                assesment.Nodes.Total, assesment.Nodes.Passed, assesment.Nodes.Failed,
                assesment.Nodes.Halfedges.Missing, assesment.Nodes.Halfedges.Invalid);
            string halfedgeString =
                "Halfedges ({0}) - P({1}) - F({2})\n" +
                "\tTwins - M({3}) - I({4})\n" +
                "\tNexts - M({5}) - I({6})\n" +
                "\tPrevious - M({7}) - I({8})\n" +
                "\tPairs - M({9}) - I({10})\n" +
                "\tOrigins - M({11}) - I({12})\n" +
                "\tPolygons - M({13}) - I({14})\n";

            string halfedgeStringFull = string.Format(halfedgeString,
                assesment.HalfEdges.Total, assesment.HalfEdges.Passed, assesment.HalfEdges.Failed,
                assesment.HalfEdges.Twins.Missing, assesment.HalfEdges.Twins.Invalid,
                assesment.HalfEdges.Nexts.Missing, assesment.HalfEdges.Nexts.Invalid,
                assesment.HalfEdges.Previous.Missing, assesment.HalfEdges.Previous.Invalid,
                assesment.HalfEdges.Pairs.Missing, assesment.HalfEdges.Pairs.Invalid,
                assesment.HalfEdges.Origins.Missing, assesment.HalfEdges.Origins.Invalid,
                assesment.HalfEdges.Polygons.Missing, assesment.HalfEdges.Polygons.Invalid
            );

            string polyString =
                "Polygons ({0}) - P({1}) - F({2})\n" +
                "\tEdges - M({3}) - I({4})\n";
            
            string polyStringFull = string.Format(polyString,
                assesment.Polygons.Total, assesment.Polygons.Passed, assesment.Polygons.Failed,
                assesment.Polygons.Halfedges.Missing, assesment.Polygons.Halfedges.Invalid);


            return nodeStringFull + halfedgeStringFull + polyStringFull;
        }

        public Node CreateNode(NodeData data)
        {
            var node = new Node(this);
            node.SetData(data);
            Nodes.Add(node);
            return node;
        }

        public Edge CreateEdgeRaw()
        {
            var edge = new Edge(this);
            Edges.Add(edge);
            return edge;
        }

        public HalfEdge CreateHalfEdgeRaw()
        {
            var halfEdge = new HalfEdge(this);
            HalfEdges.Add(halfEdge);
            return halfEdge;
        }

        public Polygon CreatePolygonRaw()
        {
            var poly = new Polygon(this);
            Polygons.Add(poly);
            return poly;
        }

        public HalfEdge CreateEdge(Node aNode, Node bNode, bool isDirected = false)
        {
//            if (aNode.Edge != null && bNode.Edge != null)
//                throw new Exception();

            var edgeAB = new HalfEdge(this);
            var edgeBA = new HalfEdge(this);
            var edge = new Edge(this);

            var edgePolygon = CreatePolygon(edgeAB);

            edgeAB.SetOrigin(aNode);
            edgeBA.SetOrigin(bNode);

            edgeAB.SetNext(edgeBA);
            edgeAB.SetPrevious(edgeBA);
            edgeAB.SetTwin(edgeBA);

            edgeAB.SetPolygon(edgePolygon);
            edgeBA.SetPolygon(edgePolygon);

            if (isDirected)
                edgeBA.SetActive(false);

            edge.SetPair(edgeAB);

            Edges.Add(edge);
            HalfEdges.Add(edgeAB);
            HalfEdges.Add(edgeBA);
            return edgeAB;
        }

        public Polygon CreatePolygon(HalfEdge edge)
        {
            var poly = new Polygon(this);
            poly.SetEdge(edge);
            Polygons.Add(poly);
            return poly;
        }

        public void DeleteNode(Node node)
        {
            foreach (var edge in node.Walk())
            {
                edge.SetOrigin(null);
            }

            node.SetEdge(null);
            Nodes.Remove(node);
        }

        public void DeleteEdge(Edge edge)
        {
            DeleteHalfEdge(edge.Alpha);
            edge.SetPair(null);
            Edges.Remove(edge);
        }

        public void DeleteHalfEdge(HalfEdge edge)
        {
            if (edge.Origin.Edge == edge)
            {
                foreach (var temp in edge.Origin.Walk())
                {
                    if (temp == edge) continue;
                    edge.Origin.SetEdge(temp);
                    break;
                }

                if (edge.Origin.Edge == edge)
                    edge.Origin.SetEdge(null);
            }

            if (edge.Twin.Origin.Edge == edge.Twin)
            {
                foreach (var temp in edge.Twin.Origin.Walk())
                {
                    if (temp == edge.Twin) continue;
                    edge.Twin.Origin.SetEdge(temp);
                    break;
                }

                if (edge.Twin.Origin.Edge == edge)
                    edge.Twin.Origin.SetEdge(null);
            }


//            edge.Next.SetPrevious(edge.Previous);
//            edge.Twin.Next.SetPrevious(edge.Twin.Previous);

            HalfEdges.Remove(edge.Twin);
            HalfEdges.Remove(edge);

            edge.Twin.SetOrigin(null);
            edge.Twin.SetPolygon(null);
            edge.Twin.SetNext(null);
            edge.Twin.SetPrevious(null);
            edge.Twin.SetTwin(null);
            
            edge.SetPair(null);
            edge.Twin.SetPair(null);

            edge.SetOrigin(null);
            edge.SetPolygon(null);
            edge.SetNext(null);
            edge.SetPrevious(null);
            edge.SetTwin(null);
        }

        public void DeletePolygon(Polygon poly)
        {
            if (poly.Edge.Polygon == poly)
                foreach (var edge in poly.Walk())
                {
                    edge.SetPolygon(null);
                }

            poly.SetEdge(null);
            Polygons.Remove(poly);
        }
    }
}