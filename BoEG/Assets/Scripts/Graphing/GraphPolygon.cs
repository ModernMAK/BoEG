using System.Collections.Generic;

namespace Graphing
{
    public partial class Graph<NodeData>
    {
        public class Polygon
        {
            public Polygon(Graph<NodeData> graph)
            {
                Graph = graph;
                Active = true;
            }

            public Graph<NodeData> Graph { get; private set; }
            public bool Active { get; private set; }

            public Polygon SetActive(bool isActive)
            {
                Active = isActive;
                return this;
            }
            public HalfEdge Edge { get; private set; }

            public void SetEdge(HalfEdge edge)
            {
                Edge = edge;
                if (edge != null && edge.Polygon != this)
                    edge.SetPolygon(this);
            }

            public void Delete()
            {
                Graph.DeletePolygon(this);
            }

            public IEnumerable<HalfEdge> Walk(bool skipInactive = true)
            {
                return Edge.WalkPolygon(skipInactive);
            }
        }
    }
}