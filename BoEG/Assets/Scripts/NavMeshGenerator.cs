using System.Collections.Generic;
using System.Linq;
using Graphing;
using Triggers;
using UnityEngine;
using Util;

public class NavMeshGenerator : MonoBehaviour
{
    public const int WorldMask = (int) LayerMaskHelper.World;
    public int Precision = 100;
    public Bounds Bounds;

    private Graph<Vector3> _graph;


// Use this for initialization
    void Start()
    {
//        _graph = BuildGraph();
        var points = CalculatePoints(Bounds, Precision);
        _graph = Create(points);
        Debug.Log("Original\n" + _graph.DebugOutput());
        Simplify(_graph);
        Debug.Log("Simple\n" + _graph.DebugOutput());
    }

    private void OnDrawGizmos()
    {
        const float bleed = 0.99f;
        const float scale = 0.05f;
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position + Bounds.center, Bounds.extents);
        if (_graph != null)
        {
            var count = _graph.Polygons.Count();
            var counter = 0;

//            var cache = new Dictionary<BasicGraph<Vector3>.Polygon, Color>();

            foreach (var poly in _graph.Polygons)
            {
//                if (cache.ContainsKey(poly))
//                    Gizmos.color = cache[poly];
//                else
//                {
//                }


                var center = Vector3.zero;
                var edgeCounter = 0;
                foreach (var edge in poly.Walk())
                {
                    center += edge.Origin.Data;
                    edgeCounter++;
                }

                if (edgeCounter != 0)
                    center /= edgeCounter;
                foreach (var edge in poly.Walk())
                {
                    Gizmos.color = Color.HSVToRGB((float) counter / count, 1f, 1f);
                    var offset = Vector3.zero; //!edge.Active ? Vector3.down : Vector3.up;
                    offset += transform.position;
                    var origin = Vector3.Lerp(center, edge.Origin.Data, bleed);
                    Gizmos.DrawSphere(origin + offset, scale);
                    if (edge.Target != null)
                    {
                        var target = Vector3.Lerp(center, edge.Target.Data, bleed);
                        var drift = Vector3.Cross(target - origin, Vector3.up).normalized * scale;
                        Gizmos.DrawLine(origin + offset, target + offset + drift);
//                        Debug.LogWarning("!");
                    }
                }

                counter++;
            }
        }
    }

    private static bool SimplifyEdges()
//    private static bool SimplifyEdges(IEnumerable<BasicGraph<Vector3>.Polygon> graph,
//        BasicGraph<Vector3>.Polygon nullPolygon)
    {
        return false;
//        HashSet<BasicGraph<Vector3>.HalfEdge> removalQueue = new HashSet<BasicGraph<Vector3>.HalfEdge>();
//
//        foreach (var poly in graph)
//        {
//            foreach (var edge in poly.WalkAlongPolygon())
//            {
//                var prevNode = edge.Previous.Origin;
//                var curNode = edge.Origin;
//                var nextNode = edge.Next.Origin;
//
//                var prevCurDir = curNode.Data - prevNode.Data;
//                var curNextDir = nextNode.Data - curNode.Data;
//
//                if (!Vector3.Dot(prevCurDir, curNextDir).SafeEquals(1f)) continue;
//                if (!removalQueue.Contains(edge.Twin)) //If our twin is not in the set...
//                    //Try adding ourselves!
//                    removalQueue.Add(edge);
//            }
//        }
//
//        foreach (var edge in removalQueue)
//        {
//            BasicGraph<Vector3>.RemoveEdge(edge);
//        }
//
//        return removalQueue.Count > 0;
    }

    private static bool SimplifyEdges(Graph<Vector3> graph)
    {
        return false;
//        var dict = new Dictionary<Graph<Vector3>.HalfEdge, Vector3>();
//        foreach (var edge in graph.HalfEdges)
//        {
//            var normal = (edge.Target.Data-edge.Origin.Data).normalized;
//            dict[edge] = normal;
//        }
//
//        var removalSet = new HashSet<Graph<Vector3>.Edge>();
//        var removalQueue = new List<Graph<Vector3>.HalfEdge>();
//
//        foreach (var edge in graph.HalfEdges)
//        {
//            if (!edge.Active || !edge.Twin.Active)
//                continue;
//
//            var normal = dict[edge];
//            var twinNormal = dict[edge.Twin];
//
//            var nextEdge = edge.Next;
//            if (!nextEdge.Active || !nextEdge.Twin.Active)
//                continue;
//
//            var nextNormal = dict[nextEdge];
//            var nextTwinNormal = dict[nextEdge.Twin];
//
//            if (Vector3.Dot(normal, nextNormal).SafeEquals(1f) &&
//                Vector3.Dot(twinNormal, nextTwinNormal).SafeEquals(1f))
//            {
//                if (removalSet.Add(edge.Pair))
//                    removalQueue.Add(edge);
//            }
//        }
//
//        foreach (var edge in removalQueue)
//        {
////            Debug.Log("Before Merge : " + graph.DebugOutput());
//            edge.MergeEdge();
////            Debug.Log("After Merge : " + graph.DebugOutput());
//        }
//
//        return removalQueue.Count > 0;
    }

    private static bool SimplifyPolygons(Graph<Vector3> graph)
    {
        var dict = new Dictionary<Graph<Vector3>.Polygon, Vector3>();
        foreach (var poly in graph.Polygons)
        {
            var l = poly.Edge.Previous.Origin.Data;
            var c = poly.Edge.Origin.Data;
            var r = poly.Edge.Next.Origin.Data;
            var normal = Vector3.Cross(l - c, r - c).normalized;
            dict[poly] = normal;
        }

        var removalQueue = new HashSet<Graph<Vector3>.Edge>();

        foreach (var poly in graph.Polygons)
        {
            if (!poly.Active)
                continue;

            var normal = dict[poly];
            foreach (var halfedge in poly.Walk())
            {
                var twinPoly = halfedge.Twin.Polygon;
                if (!twinPoly.Active)
                    continue;
                var twinNormal = dict[twinPoly];
                if (Vector3.Dot(normal, twinNormal).SafeEquals(1f))
                {
                    removalQueue.Add(halfedge.Pair);
                }
            }
        }

        foreach (var edge in removalQueue)
        {
//            Debug.Log("Before Merge : " + graph.DebugOutput());
            edge.Alpha.MergeFace();
//            Debug.Log("After Merge : " + graph.DebugOutput());
        }

        return removalQueue.Count > 0;
    }

    private static bool Simplify(Graph<Vector3> graph)
    {
        var simplified = false;
        var edited = false;
        do
        {
            edited = false;
            edited |= SimplifyPolygons(graph);
            edited |= SimplifyEdges(graph);
            simplified |= edited;
        } while (edited);

        return simplified;
    }
    private static Graph<T>.Node[,] CreateNodes<T>(Graph<T> graph, T[,] points)
    {
        var xLen = points.GetLength(0);
        var yLen = points.GetLength(1);
        var nodes = new Graph<T>.Node[xLen, yLen];
        for (var x = 0; x < xLen; x++)
        for (var y = 0; y < yLen; y++)
            nodes[x, y] = graph.CreateNode(points[x, y]);
        return nodes;
    }

    private static Graph<T>.HalfEdge[,,] CreateInteriorEdges<T>(Graph<T> graph, Graph<T>.Node[,] nodes)
    {
        var xLen = nodes.GetLength(0) - 1;
        var yLen = nodes.GetLength(1) - 1;
        var edges = new Graph<T>.HalfEdge[xLen, yLen, 6];
        for (var x = 0; x < xLen; x++)
        for (var y = 0; y < yLen; y++)
        {
            for (var z = 0; z < 6; z++)
                edges[x, y, z] = graph.CreateHalfEdgeRaw();
            //Left
            //Up
            //Right
            //Down
            //CrossUpLeft
            //CrossDownRight

            var nodeUL = nodes[x, y];
            var nodeUR = nodes[x + 1, y];
            var nodeLL = nodes[x, y + 1];
            var nodeLR = nodes[x + 1, y + 1];

            var left = edges[x, y, 0].SetOrigin(nodeLL);
            var up = edges[x, y, 1].SetOrigin(nodeUL);
            var right = edges[x, y, 2].SetOrigin(nodeUR);
            var down = edges[x, y, 3].SetOrigin(nodeLR);
            var crossUL = edges[x, y, 4].SetOrigin(nodeUR);
            var crossDR = edges[x, y, 5].SetOrigin(nodeLL);

            var polyUL = graph.CreatePolygonRaw();
            left.SetNext(up).SetPolygon(polyUL);
            up.SetNext(crossUL).SetPolygon(polyUL);
            crossUL.SetNext(left).SetPolygon(polyUL);
            polyUL.SetEdge(crossUL);

            var polyDR = graph.CreatePolygonRaw();
            right.SetNext(down).SetPolygon(polyDR);
            down.SetNext(crossDR).SetPolygon(polyDR);
            crossDR.SetNext(right).SetPolygon(polyDR);
            polyDR.SetEdge(crossDR);


            var crossEdge = graph.CreateEdgeRaw();
            crossEdge.SetPair(crossUL);
            crossUL.SetPair(crossEdge);
            crossDR.SetPair(crossEdge);
            crossUL.SetTwin(crossDR);

            if (y > 0)
            {
                var neighborDown = edges[x, y - 1, 3];
                var pair = graph.CreateEdgeRaw();
                up.SetTwin(neighborDown);

                pair.SetPair(up);
                up.SetPair(pair);
                neighborDown.SetPair(pair);
            }

            if (x > 0)
            {
                var neighborRight = edges[x - 1, y, 2];
                var pair = graph.CreateEdgeRaw();
                left.SetTwin(neighborRight);

                pair.SetPair(left);
                left.SetPair(pair);
                neighborRight.SetPair(pair);
            }
        }

        return edges;
    }

    private static void CreateExteriorEdges<T>(Graph<T> graph, Graph<T>.HalfEdge[,,] edges)
    {
        var boundPoly = graph.CreatePolygonRaw().SetActive(false);

        var tempEdges = new List<Graph<T>.HalfEdge>();
        var xLen = edges.GetLength(0);
        var yLen = edges.GetLength(1);

        for (var x = 0; x < xLen; x++)
        {
            const int y = 0;
            var edge = edges[x, y, 1]; //Up
            var node = edge.Next.Origin;
            var twinEdge = graph.CreateHalfEdgeRaw().SetPolygon(boundPoly).SetOrigin(node);
            var edgePair = graph.CreateEdgeRaw();
            edge.SetTwin(twinEdge);

            edge.SetPair(edgePair);
            twinEdge.SetPair(edgePair);
            edgePair.SetPair(edge);

            twinEdge.SetActive(false);
            tempEdges.Add(twinEdge);
        }

        for (var y = 0; y < yLen; y++)
        {
            var x = xLen - 1;
            var edge = edges[x, y, 2]; //Right
            var node = edge.Next.Origin;
            var twinEdge = graph.CreateHalfEdgeRaw().SetPolygon(boundPoly).SetOrigin(node);
            var edgePair = graph.CreateEdgeRaw();
            edge.SetTwin(twinEdge);

            edge.SetPair(edgePair);
            twinEdge.SetPair(edgePair);
            edgePair.SetPair(edge);

            twinEdge.SetActive(false);
            tempEdges.Add(twinEdge);
        }

        for (var x = xLen - 1; x >= 0; x--)
        {
            var y = yLen - 1;
            var edge = edges[x, y, 3]; //Down
            var node = edge.Next.Origin;
            var twinEdge = graph.CreateHalfEdgeRaw().SetPolygon(boundPoly).SetOrigin(node);
            var edgePair = graph.CreateEdgeRaw();
            edge.SetTwin(twinEdge);

            edge.SetPair(edgePair);
            twinEdge.SetPair(edgePair);
            edgePair.SetPair(edge);

            twinEdge.SetActive(false);
            tempEdges.Add(twinEdge);
        }

        for (var y = yLen - 1; y >= 0; y--)
        {
            const int x = 0;
            var edge = edges[x, y, 0]; //Left
            var node = edge.Next.Origin;
            var twinEdge = graph.CreateHalfEdgeRaw().SetPolygon(boundPoly).SetOrigin(node);
            var edgePair = graph.CreateEdgeRaw();
            edge.SetTwin(twinEdge);

            edge.SetPair(edgePair);
            twinEdge.SetPair(edgePair);
            edgePair.SetPair(edge);

            twinEdge.SetActive(false);
            tempEdges.Add(twinEdge);
        }

        for (var i = 0; i < tempEdges.Count; i++)
        {
            tempEdges[i].SetNext(tempEdges[(i + 1) % tempEdges.Count]);
        }

        tempEdges.Clear();
    }

    private static Graph<T> Create<T>(T[,] points)
    {
        var graph = new Graph<T>();
        Graph<T>.Node[,] nodes = CreateNodes(graph, points);
        Graph<T>.HalfEdge[,,] edges = CreateInteriorEdges(graph, nodes);
        CreateExteriorEdges(graph, edges);


//        CreateEdges(graph, nodes);
//
//        graph.CreateEdge(graph.Nodes[0], graph.Nodes[1]);


        return graph;
    }

    //On a failure, lerps neighbors to create a point.
    private static Vector3[,] CalculatePoints(Bounds bounds, int precision = 100)
    {
        //These two lines make a grid with precision number of subdivisions;
        var grid = new Vector3?[precision + 2, precision + 2];
        precision++;
        
        var minPos = bounds.center - bounds.extents / 2f;
        var maxPos = bounds.center + bounds.extents / 2f;

//        var lerpedQueue = new Queue<KeyValuePair<Vector2Int, Vector3>>();

        for (var x = 0; x <= precision; x++)
        for (var z = 0; z <= precision; z++)
        {
            var xPos = Mathf.Lerp(minPos.x, maxPos.x, (float) x / precision);
            var yPos = maxPos.y;
            var zPos = Mathf.Lerp(minPos.z, maxPos.z, (float) z / precision);

            var ray = new Ray(new Vector3(xPos, yPos, zPos), Vector3.down);
            RaycastHit hitinfo;
            if (Physics.Raycast(ray, out hitinfo, Mathf.Abs(maxPos.y - minPos.y), WorldMask))
            {
                grid[x, z] = hitinfo.point;
            }
            else
            {
                grid[x, z] = null;
            }
        }

        var nGrid = new Vector3[precision + 1, precision + 1];
        for (var x = 0; x <= precision; x++)
        for (var z = 0; z <= precision; z++)
        {
            nGrid[x, z] = grid[x, z].HasValue ? grid[x, z].Value : FindLerp(grid, x, z);
        }

        return nGrid;
    }

    private static Vector3 FindLerp(Vector3?[,] grid, int x, int y)
    {
        var lerped = Vector3.zero;
        var pos = new Vector2Int(x, y);
        var counter = 0;
        for (int xO = -1; xO <= 1; xO++)
        for (int zO = -1; zO <= 1; zO++)
        {
            var nPos = pos + new Vector2Int(xO, zO);
            if (nPos.x < 0 || nPos.x >= grid.GetLength(0) || nPos.y < 0 || nPos.y >= grid.GetLength(1))
                continue;

            if (grid[nPos.x, nPos.y].HasValue)
            {
                lerped += grid[nPos.x, nPos.y].Value;
                counter++;
            }
        }

        if (counter > 0)
            lerped /= counter;

        return lerped;
    }

// Update is called once per frame
    public bool Rerun = false;

    void Update()
    {
        if (Rerun)
        {
            Rerun = false;
            Start();
        }
    }
}