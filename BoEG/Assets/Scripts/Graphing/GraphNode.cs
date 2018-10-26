using System.Collections.Generic;

namespace Graphing
{
    public partial class Graph<NodeData>
    {
        public class Node
        {
            public Node(Graph<NodeData> graph)
            {
                Graph = graph;
            }
            
            public Graph<NodeData> Graph { get; private set; }
            public HalfEdge Edge { get; private set; }
            

            public void SetEdge(HalfEdge edge)
            {
                Edge = edge;
                if (edge != null && edge.Origin != this)
                    edge.SetOrigin(this);
            }
            public NodeData Data { get; private set; }
            public void SetData(NodeData data)
            {
                Data = data;
            }
            public void Delete()
            {
                Graph.DeleteNode(this);
            }
            public IEnumerable<HalfEdge> Walk(bool skipInactive = true)
            {
                return Edge.WalkOrigin(skipInactive);
            }
        }
    }
}