using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Graphing
{
    public partial class Graph<NodeData>
    {
        public class Edge
        {
            public Graph<NodeData> Graph { get; private set; }

            public Edge(Graph<NodeData> graph)
            {
                Graph = graph;
            }

            public HalfEdge Alpha { get; private set; }

            public HalfEdge Beta
            {
                get { return Alpha != null ? Alpha.Twin : null; }
            }

            public bool OwnsEdges(HalfEdge a)
            {
                if (a == null)
                    return Alpha == null && Beta == null;

                return
                    (Alpha == a && Beta == a.Twin) ||
                    (Alpha == a.Twin && Beta == a);
            }

            public void SetPair(HalfEdge alpha)
            {
                if (OwnsEdges(alpha)) return;
                Alpha = alpha;
                if (Alpha != null)
                    Alpha.SetPair(this);
                if (Beta != null)
                    Beta.SetPair(this);
            }

            public void Delete()
            {
                Graph.DeleteEdge(this);
            }
        }

        public class HalfEdge
        {
            public HalfEdge(Graph<NodeData> graph)
            {
                Graph = graph;
                Active = true;
            }

            public Graph<NodeData> Graph { get; private set; }
            public bool Active { get; private set; }

            public HalfEdge SetActive(bool isActive)
            {
                Active = isActive;
                return this;
            }

            public Node Origin { get; private set; }

            public Node Target
            {
                get { return (Twin != null ? Twin.Origin : null); }
            }

            public HalfEdge SetOrigin(Node node)
            {
                Origin = node;
                if (node != null && node.Edge != this)
                    node.SetEdge(this);
                return this;
            }

            public Edge Pair { get; private set; }

            public void SetPair(Edge edge) //, bool updateEdge = false)
            {
                Pair = edge;
//                if (!updateEdge || edge.OwnsEdges(this)) return;
//                edge.SetPair(this);
            }

            public HalfEdge Next { get; private set; }

            public HalfEdge SetNext(HalfEdge edge)
            {
                Next = edge;
                if (edge != null && edge.Previous != this)
                    edge.SetPrevious(this);
                return this;
            }

            public HalfEdge Previous { get; private set; }

            public HalfEdge SetPrevious(HalfEdge edge)
            {
                Previous = edge;
                if (edge != null && edge.Next != this)
                    edge.SetNext(this);
                return this;
            }

            public HalfEdge Twin { get; private set; }

            public HalfEdge SetTwin(HalfEdge edge)
            {
                Twin = edge;
                if (edge != null && edge.Twin != this)
                    edge.SetTwin(this);
                return this;
            }

            public Polygon Polygon { get; private set; }

            public HalfEdge SetPolygon(Polygon poly)
            {
                Polygon = poly;
                if (poly != null && Polygon.Edge != this)
                    Polygon.SetEdge(this);
                return this;
            }

            public void Delete()
            {
                Pair.Delete();
            }

            public HalfEdge AddAfter(Node node)
            {
                var halfEdge = Graph.CreateEdge(Target, node);
                var tempPoly = halfEdge.Polygon;
                tempPoly.Delete();

                halfEdge.SetPolygon(Polygon);
                halfEdge.Twin.SetPolygon(Polygon);

                halfEdge.Twin.SetNext(Next);
                //Next will get overwritten, so we do it afterwards
                halfEdge.SetPrevious(this);
                return halfEdge;
            }

            public HalfEdge AddBefore(Node node)
            {
                return Previous.AddAfter(node);
            }

            public HalfEdge SplitFace(HalfEdge edge)
            {
                if (Polygon != edge.Polygon)
                    throw new Exception();
                if (edge == this)
                    throw new Exception();


                var halfEdge = Graph.CreateEdge(Origin, edge.Origin);
                var halfEdgePoly = halfEdge.Polygon;
                var halfEdgeTwinPoly = Polygon;

                halfEdge.SetNext(edge);
                halfEdge.Twin.SetNext(this);

                var walk = halfEdge.WalkPolygon().ToArray();
                foreach (var tempEdge in walk)
                {
                    tempEdge.SetPolygon(halfEdgePoly);
                }

                walk = halfEdge.Twin.WalkPolygon().ToArray();
                foreach (var tempEdge in walk)
                {
                    tempEdge.SetPolygon(halfEdgeTwinPoly);
                }

                return halfEdge;
            }

            public void MergeEdge()
            {
                throw new NotImplementedException();
                //Implimentation does not work
                if (Twin == Next && Twin == Previous)
                {
                    Delete();
                }
                else if (Twin == Next)
                {
                    Previous.SetNext(Twin.Next);
                    Delete();
                }
                else if (Twin == Previous)
                {
                    Next.SetPrevious(Twin.Previous);
                    Delete();
                }
                else
                {
                    //Skip Self
                    Previous.SetNext(Next);

                    //Skip Twin
                    Twin.Previous.SetNext(Twin.Next);
                    //Fix Twin's Next
                    Twin.Origin.SetEdge(Next);
                    Twin.Next.SetOrigin(Twin.Origin);

                    if (Origin.Edge == this)
                    {
                        HalfEdge tempEdge = null;
                        foreach (var edge in Origin.Walk())
                        {
                            if (this == edge) continue;
                            tempEdge = edge;
                            break;
                        }
                        Origin.SetEdge(tempEdge);
                    }
                    Delete();
                }
            }


            public void MergeFace()
            {
                if (Twin.Polygon == Polygon)
                {
                    //TODO Fix this later
//                    MergeEdge();
                    return;
                }

                if (Polygon.Edge == this)
                    Polygon.SetEdge(Next);

                var poly = Twin.Polygon;
                foreach (var edge in poly.Walk())
                {
                    edge.Polygon = Polygon;
                }

                poly.Delete();

                Origin.SetEdge(Twin.Next);
                Twin.Origin.SetEdge(Next);

                Next.SetPrevious(Twin.Previous);
                Previous.SetNext(Twin.Next);
                Delete();
            }

            public IEnumerable<HalfEdge> WalkPolygon(bool skipInactive = true)
            {
                var cur = this;
                var start = cur;
                //Debug.Log(Graph.DebugOutput()); 
                do
                {
                    if (!(skipInactive && !cur.Active))
                        yield return cur;
                    cur = cur.Next;
                } while (cur != start);
            }

            public IEnumerable<HalfEdge> WalkOrigin(bool skipInactive = true)
            {
                var cur = this;
                var start = cur;
                do
                {
                    if (!(skipInactive && !cur.Active))
                        yield return cur;
                    cur = cur.Twin.Next;
                } while (cur != start);
            }

            public IEnumerable<HalfEdge> WalkEdge(int maxEdges)
            {
                var counter = 0;
                foreach (var edge in WalkEdge())
                {
                    if (counter > maxEdges) continue;
                    yield return edge;
                    counter++;
                }
            }

            public IEnumerable<HalfEdge> WalkEdge()
            {
                var cur = this;
                var start = cur;
                do
                {
                    if (cur.Active)
                    {
                        yield return cur;
                        cur = cur.Next;
                    }
                    else
                    {
                        var innerCur = cur;
                        var innerStart = cur;
                        var forceExit = false;
                        do
                        {
                            innerCur = innerCur.Twin.Next;
                            if (!innerCur.Active) continue;
                            yield return innerCur;
                            forceExit = true;
                        } while (innerCur != innerStart && !forceExit);
                    }
                } while (cur != start);
            }
        }
    }
}