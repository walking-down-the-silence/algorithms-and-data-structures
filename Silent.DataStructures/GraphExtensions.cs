using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.DataStructures;

namespace Common.Algorithms
{
    public static class GraphExtensions
    {
        /// <summary>
        /// Parses the input lines of string and fills the <see cref="Graph"/> instance
        /// </summary>
        /// <param name="graph"> <see cref="Graph"/> instance. </param>
        /// <param name="lines"> Lines of strings to parse. </param>
        /// <param name="directedGraph"> Defines if parsed edges should be threated as directed. </param>
        public static void Parse(this Graph graph, string[] lines, bool directedGraph = false)
        {
            foreach (string line in lines)
            {
                var labels = line.Split(' ');
                var startLabel = labels[0];
                var endLabel = labels[1];
                var weight = labels.Length > 2 ? int.Parse(labels[2]) : 1;

                if (!graph.ContainsVertex(startLabel))
                {
                    graph.SetVertex(new Vertex(startLabel));
                }

                if (!graph.ContainsVertex(endLabel))
                {
                    graph.SetVertex(new Vertex(endLabel));
                }

                var startVertex = graph[startLabel];
                var endVertex = graph[endLabel];

                graph.SetEdge(new Edge(startVertex, endVertex, weight));

                if (!directedGraph)
                {
                    graph.SetEdge(new Edge(endVertex, startVertex, weight));
                }
            }
        }

        /// <summary>
        /// Breadth-First-Search algorithm that traverses the graph
        /// </summary>
        /// <param name="graph"> <see cref="Graph"/> instance. </param>
        /// <param name="startVertex"> <see cref="Vertex"/> to start traversing from. </param>
        /// <param name="endVertex"> <see cref="Vertex"/> to end traversing at. </param>
        /// <returns> The traversed path. </returns>
        public static IEnumerable<Vertex> BreadthFirstSearch(this Graph graph, Vertex startVertex, Vertex endVertex)
        {
            var result = new List<Vertex>();
            var visited = new HashSet<Vertex>();
            var queue = new Queue<Vertex>();
            queue.Enqueue(startVertex);

            while (queue.Count > 0)
            {
                var currentVertex = queue.Dequeue();

                if (visited.Contains(currentVertex))
                    continue;

                visited.Add(currentVertex);
                var neighbors = currentVertex.OutboundEdges.Where(edge => !visited.Contains(edge.EndVertex)).Select(edge => edge.EndVertex);

                result.Add(currentVertex);

                foreach (var neighbor in neighbors)
                {
                    queue.Enqueue(neighbor);
                }
            }

            return result;
        }

        /// <summary>
        /// Depth-First-Search algorithm that traverses the graph
        /// </summary>
        /// <param name="graph"> <see cref="Graph"/> instance. </param>
        /// <param name="startVertex"> <see cref="Vertex"/> to start traversing from. </param>
        /// <param name="endVertex"> <see cref="Vertex"/> to end traversing at. </param>
        /// <returns> The traversed path. </returns>
        public static IEnumerable<Vertex> DepthFirstSearch(this Graph graph, Vertex startVertex, Vertex endVertex)
        {
            var result = new List<Vertex>();
            var visited = new HashSet<Vertex>();
            var stack = new Stack<Vertex>();
            stack.Push(startVertex);

            while (stack.Count > 0)
            {
                var currentVertex = stack.Pop();

                if (visited.Contains(currentVertex))
                    continue;

                visited.Add(currentVertex);
                var neighbors = currentVertex.OutboundEdges.Where(edge => !visited.Contains(edge.EndVertex)).Select(edge => edge.EndVertex);

                result.Add(currentVertex);

                foreach (var neighbor in neighbors)
                {
                    stack.Push(neighbor);
                }
            }

            return result;
        }

        #region Shortest Path Finding Algorithms

        /// <summary>
        /// Dijkstras algorithm that traverses the graph to find shortest path
        /// </summary>
        /// <param name="graph"> <see cref="Graph"/> instance. </param>
        /// <param name="startVertex"> The start <see cref="Vertex"/> of path to calculate minimum distance for. </param>
        /// <param name="endVertex"> The end <see cref="Vertex"/> of path to calculate minimum distance for. </param>
        /// <returns> The shortest (minimum cost) path from starting point to ending point. </returns>
        public static Pathway Dijkstra(this Graph graph, Vertex startVertex, Vertex endVertex)
        {
            var roadmap = Dijkstra(graph, startVertex);
            return roadmap[endVertex];
        }

        /// <summary>
        /// Dijkstras algorithm that traverses the graph to find shortest path
        /// </summary>
        /// <param name="graph"> <see cref="Graph"/> instance. </param>
        /// <param name="startVertex"> The start <see cref="Vertex"/> of path to calculate minimum distance for. </param>
        /// <returns> The shortest (minimum cost) path from starting point to all other. </returns>
        public static PathwayCollection Dijkstra(this Graph graph, Vertex startVertex)
        {
            var distances = graph.Vertices.ToDictionary(key => key, value => long.MaxValue);
            distances[startVertex] = 0;
            var distancesMinimumHeap = new BinaryHeap<VertexDistancePair>(
                BinaryHeapType.MinimumHeap,
                graph.Vertices.Count,
                new VertexDistancePairComparer());
            distancesMinimumHeap.Add(new VertexDistancePair(startVertex, 0));

            var pathVertices = new Dictionary<Vertex, Vertex>();

            while (distancesMinimumHeap.Count > 0)
            {
                var shortestDistanceVertexPair = distancesMinimumHeap.Remove();
                var shortestDistanceVertex = shortestDistanceVertexPair.Vertex;

                foreach (var outboundEdge in shortestDistanceVertex.OutboundEdges)
                {
                    var outboundEndVertex = outboundEdge.EndVertex;
                    long alternativeDistance = distances[shortestDistanceVertex] + outboundEdge.Weight;

                    if (alternativeDistance < distances[outboundEndVertex])
                    {
                        distances[outboundEndVertex] = alternativeDistance;
                        pathVertices[outboundEndVertex] = shortestDistanceVertex;
                        distancesMinimumHeap.Add(new VertexDistancePair(outboundEndVertex, alternativeDistance));
                    }
                }
            }

            return new PathwayCollection(pathVertices, distances, startVertex);
        }

        /// <summary>
        /// A* algorithm that traverses the graph to find shortest path between set vertices
        /// </summary>
        /// <param name="graph"> <see cref="Graph"/> instance. </param>
        /// <param name="startVertex"> The start <see cref="Vertex"/> of path to calculate minimum distance for. </param>
        /// <param name="endVertex"> The end <see cref="Vertex"/> of path to calculate minimum distance for. </param>
        /// <returns> The shortest (minimum cost) path from starting point to ending point. </returns>
        public static Pathway AStar(this Graph graph, Vertex startVertex, Vertex endVertex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Bellman-Ford's algorithm for finding shortest paths in a graph and detect negative cost cycles
        /// </summary>
        /// <param name="graph"> <see cref="Graph"/> instance. </param>
        /// <param name="startVertex"> The start <see cref="Vertex"/> of path to calculate minimum distance for. </param>
        /// <returns> The shortest (minimum cost) path from starting point to all other. </returns>
        public static PathwayCollection BellmanFord(this Graph graph, Vertex startVertex)
        {
            var distances = graph.Vertices.ToDictionary(key => key, value => long.MaxValue);
            distances[startVertex] = 0;
            var pathVertices = new Dictionary<Vertex, Vertex>();

            for (int i = 0; i < graph.Vertices.Count - 1; i++)
            {
                foreach (var vertex in graph.Vertices)
                {
                    var currentDistance = distances[vertex];

                    foreach (var inboundEdge in vertex.InboundEdges)
                    {
                        var alternativeDistance = distances[inboundEdge.StartVertex] + inboundEdge.Weight;
                        if (alternativeDistance < currentDistance)
                        {
                            distances[vertex] = alternativeDistance;
                            pathVertices[vertex] = inboundEdge.StartVertex;
                        }
                    }
                }
            }

            foreach (var vertex in graph.Vertices)
            {
                foreach (var inboundEdge in vertex.InboundEdges)
                {
                    var alternativeDistance = distances[inboundEdge.StartVertex] + inboundEdge.Weight;
                    if (alternativeDistance < distances[vertex])
                    {
                        throw new NegativeCostCycleException();
                    }
                }
            }

            return new PathwayCollection(pathVertices, distances, startVertex);
        }

        /// <summary>
        /// Floyd–Warshall algorithm for finding shortest paths in a weighted graph with positive or negative edge weights (but with no negative cycles)
        /// </summary>
        /// <param name="graph"> <see cref="Graph"/> instance. </param>
        /// <returns> The shortest (minimum cost) path from any vertexs to all other vertices. </returns>
        public static Roadmap FloydWarshall(this Graph graph)
        {
            var distances = new Dictionary<Tuple<Vertex, Vertex>, long>();

            foreach (var vertexI in graph.Vertices)
            {
                foreach (var vertexK in graph.Vertices)
                {
                    var key = new Tuple<Vertex, Vertex>(vertexI, vertexK);
                    distances[key] = vertexI.Equals(vertexK) ? 0 : long.MaxValue;
                }
            }

            foreach (var edge in graph.Edges)
            {
                var key = new Tuple<Vertex, Vertex>(edge.StartVertex, edge.EndVertex);
                distances[key] = edge.Weight;
            }

            foreach (var vertexK in graph.Vertices)
            {
                foreach (var vertexI in graph.Vertices)
                {
                    foreach (var vertexJ in graph.Vertices)
                    {
                        var keyIJ = new Tuple<Vertex, Vertex>(vertexI, vertexJ);
                        var keyIK = new Tuple<Vertex, Vertex>(vertexI, vertexK);
                        var keyKJ = new Tuple<Vertex, Vertex>(vertexK, vertexJ);

                        if (distances[keyIJ] > distances[keyIK] + distances[keyKJ])
                        {
                            distances[keyIJ] = distances[keyIK] + distances[keyKJ];
                        }
                    }
                }
            }

            return new Roadmap(distances);
        }

        #endregion

        /// <summary>
        /// Tarjans algorithm to topologically sort the graph
        /// </summary>
        /// <param name="graph"> <see cref="Graph"/> instance. </param>
        /// <param name="recursive"> Defines if Tarjan's algorithm should perform a recursive or stack-based search. </param>
        /// <returns> The topologically sorted strongly connected conponents. </returns>
        public static IEnumerable<Vertex> Tarjan(this Graph graph, bool recursive = false)
        {
            TarjanDepthFirstSearchDelegate tarjanDfs = recursive
                ? new TarjanDepthFirstSearchDelegate(TarjanDfsRecursive)
                : new TarjanDepthFirstSearchDelegate(TarjanDfsStack);

            var topologicalOrderSet = new HashSet<Vertex>();
            var visitedVertices = graph.Vertices.ToDictionary(key => key, value => TarjansVisitStatus.NotVisited);

            foreach (var vertex in graph.Vertices)
            {
                if (!tarjanDfs(vertex, topologicalOrderSet, visitedVertices))
                {
                    throw new NotDirectlyAcyclicGraphException();
                }
            }

            return topologicalOrderSet;
        }

        #region Minimum Spanning Tree Algorithms

        /// <summary>
        /// Prim's algorithm to find the minimum span tree on graph
        /// </summary>
        /// <param name="graph"> <see cref="Graph"/> instance. </param>
        /// <returns> Minimum span tree containing edges and minimum distance. </returns>
        public static MinimumSpanTree PrimsMinimumSpanningTree(this Graph graph)
        {
            if (graph.Vertices.Count == 0 || graph.Edges.Count == 0)
                return new MinimumSpanTree(Enumerable.Empty<Edge>().ToList(), 0);

            var currentVertex = graph.Vertices.First();
            int minimumDistance = 0;
            var minimumSpanTree = new List<Edge>();
            var edgesToVisit = new BinaryHeap<Edge>(BinaryHeapType.MinimumHeap, currentVertex.OutboundEdges.Count, new EdgeComparer());
            var verticesVisited = new HashSet<Vertex>();

            while (minimumSpanTree.Count < graph.Vertices.Count - 1)
            {
                foreach (var edge in currentVertex.OutboundEdges)
                {
                    edgesToVisit.Add(edge);
                }

                verticesVisited.Add(currentVertex);
                Edge minimumEdge = null;

                while (edgesToVisit.Count > 0)
                {
                    minimumEdge = edgesToVisit.Remove();
                    if (verticesVisited.Contains(minimumEdge.StartVertex) != verticesVisited.Contains(minimumEdge.EndVertex))
                        break;
                }

                if (minimumEdge == null)
                {
                    throw new MultipleMinimumSpanningTreesException();
                }

                minimumSpanTree.Add(minimumEdge);
                minimumDistance += minimumEdge.Weight;
                currentVertex = verticesVisited.Contains(minimumEdge.EndVertex)
                                      ? minimumEdge.StartVertex
                                      : minimumEdge.EndVertex;
            }

            return new MinimumSpanTree(minimumSpanTree, minimumDistance);
        }

        /// <summary>
        /// Kruskal's algorithm to find the minimum span tree on graph
        /// </summary>
        /// <param name="graph"> <see cref="Graph"/> instance. </param>
        /// <returns> Minimum span tree containing edges and minimum distance. </returns>
        public static MinimumSpanTree KruskalsMinimumSpanningTree(this Graph graph)
        {
            if (graph.Vertices.Count == 0 || graph.Edges.Count == 0)
                return new MinimumSpanTree(Enumerable.Empty<Edge>().ToList(), 0);

            var minimumSpanTree = new List<Edge>();
            var minimumDistance = 0;
            var unionFind = new DisjointSet<Vertex>(graph.Vertices);
            var edgesToVisit = new BinaryHeap<Edge>(BinaryHeapType.MinimumHeap, graph.Edges.Count, new EdgeComparer());

            foreach (var edge in graph.Edges)
            {
                edgesToVisit.Add(edge);
            }

            while (minimumSpanTree.Count < graph.Vertices.Count - 1)
            {
                Edge minimumEdge = null;

                while (edgesToVisit.Count > 0)
                {
                    minimumEdge = edgesToVisit.Remove();
                    if (unionFind.Find(minimumEdge.StartVertex) != unionFind.Find(minimumEdge.EndVertex))
                        break;
                }

                if (minimumEdge == null)
                {
                    throw new MultipleMinimumSpanningTreesException();
                }

                minimumSpanTree.Add(minimumEdge);
                minimumDistance += minimumEdge.Weight;
                unionFind.Union(minimumEdge.StartVertex, minimumEdge.EndVertex);
            }

            return new MinimumSpanTree(minimumSpanTree, minimumDistance);
        }

        #endregion

        /// <summary>
        /// Recursive function for vertex traversal for Tarjans algorithm
        /// </summary>
        /// <param name="vertex"> <see cref="Vertex"/> instance to start traversing from. </param>
        /// <param name="topologicalOrder"> List of topologically sorted vertices so far. </param>
        /// <param name="visitedVertices"> Map of vertices status traversed so far. </param>
        /// <returns> Returns true if graph is a Directed Acyclic Graph. </returns>
        private static bool TarjanDfsRecursive(Vertex vertex, ICollection<Vertex> topologicalOrder, Dictionary<Vertex, TarjansVisitStatus> visitedVertices)
        {
            if (visitedVertices[vertex] == TarjansVisitStatus.Visited)
            {
                return false;
            }

            if (visitedVertices[vertex] == TarjansVisitStatus.NotVisited)
            {
                visitedVertices[vertex] = TarjansVisitStatus.Visited;

                foreach (var outboundEdge in vertex.OutboundEdges)
                {
                    TarjanDfsRecursive(outboundEdge.EndVertex, topologicalOrder, visitedVertices);
                }

                visitedVertices[vertex] = TarjansVisitStatus.Resolved;
                topologicalOrder.Add(vertex);
            }

            return true;
        }

        /// <summary>
        /// Stack-based function for vertex traversal for Tarjans algorithm
        /// </summary>
        /// <param name="vertex"> <see cref="Vertex"/> instance to start traversing from. </param>
        /// <param name="topologicalOrderSet"> Set of topologically sorted vertices so far.  </param>
        /// <param name="visitedVertices"> Map of vertices status traversed so far. </param>
        /// <returns> Returns true if graph is a Directed Acyclic Graph. </returns>
        private static bool TarjanDfsStack(Vertex vertex, ICollection<Vertex> topologicalOrderSet, Dictionary<Vertex, TarjansVisitStatus> visitedVertices)
        {
            var stack = new Stack<Vertex>();
            stack.Push(vertex);

            while (stack.Count > 0)
            {
                var currentVertex = stack.Pop();
                visitedVertices[currentVertex] = TarjansVisitStatus.Visited;
                var nextVertices = new List<Vertex>();

                foreach (var nextVertex in currentVertex.OutboundEdges.Select(x => x.EndVertex))
                {
                    if (visitedVertices[nextVertex] == TarjansVisitStatus.NotVisited)
                    {
                        nextVertices.Add(nextVertex);
                    }

                    if (visitedVertices[nextVertex] == TarjansVisitStatus.Visited)
                    {
                        return false;
                    }
                }

                if (nextVertices.Count > 0)
                {
                    stack.Push(currentVertex);

                    foreach (var nextVertex in nextVertices)
                    {
                        stack.Push(nextVertex);
                    }
                }
                else
                {
                    visitedVertices[currentVertex] = TarjansVisitStatus.Resolved;
                    topologicalOrderSet.Add(currentVertex);
                }
            }

            return true;
        }

        private delegate bool TarjanDepthFirstSearchDelegate(
            Vertex vertex,
            ICollection<Vertex> topologicalOrder,
            Dictionary<Vertex, TarjansVisitStatus> visitedVertices);

        /// <summary>
        /// Vertex visit status for Tarjan's algorithm
        /// </summary>
        private enum TarjansVisitStatus
        {
            NotVisited = 0,
            Visited = 1,
            Resolved = 2
        }

        private class EdgeComparer : IComparer<Edge>
        {
            public int Compare(Edge x, Edge y)
            {
                if (x.Weight < y.Weight)
                {
                    return -1;
                }

                if (x.Weight > y.Weight)
                {
                    return 1;
                }

                return 0;
            }
        }

        private class VertexDistancePairComparer : IComparer<VertexDistancePair>
        {
            public int Compare(VertexDistancePair x, VertexDistancePair y)
            {
                if (x.Distance < y.Distance)
                {
                    return -1;
                }

                if (x.Distance > y.Distance)
                {
                    return 1;
                }

                return 0;
            }
        }

        private class VertexDistancePair
        {
            public VertexDistancePair()
            {
            }

            public VertexDistancePair(Vertex vertex, long distance)
            {
                Vertex = vertex;
                Distance = distance;
            }

            public Vertex Vertex { get; set; }

            public long Distance { get; set; }
        }
    }

    public class Roadmap
    {
        private readonly IDictionary<Tuple<Vertex, Vertex>, long> _roadmap;

        public Roadmap(IDictionary<Tuple<Vertex, Vertex>, long> roadmap)
        {
            _roadmap = roadmap;
        }

        public long this[Vertex startVertex, Vertex endVertex]
        {
            get
            {
                var key = new Tuple<Vertex, Vertex>(startVertex, endVertex);
                return _roadmap[key];
            }
        }
    }

    public class PathwayCollection
    {
        private readonly IDictionary<Vertex, Vertex> _roadmap;

        private readonly IDictionary<Vertex, long> _distances;

        private readonly IDictionary<Vertex, Pathway> _pathways = new Dictionary<Vertex, Pathway>();

        public PathwayCollection(IDictionary<Vertex, Vertex> roadmap, IDictionary<Vertex, long> distances, Vertex startVertex)
        {
            _roadmap = roadmap;
            _distances = distances;
            StartVertex = startVertex;
        }

        public Pathway this[Vertex endVertex]
        {
            get
            {
                if (!_pathways.ContainsKey(endVertex))
                {
                    _pathways[endVertex] = ReconstructPath(endVertex);
                }

                return _pathways[endVertex];
            }
        }

        public Vertex StartVertex { get; }

        private Pathway ReconstructPath(Vertex endVertex)
        {
            var predcessor = _roadmap[endVertex];
            var pathVertices = new Stack<Vertex>();
            pathVertices.Push(endVertex);

            while (predcessor != null)
            {
                pathVertices.Push(predcessor);
                predcessor = _roadmap.ContainsKey(predcessor) ? _roadmap[predcessor] : null;
            }

            return new Pathway(pathVertices.ToList(), _distances[endVertex]);
        }
    }

    public class Pathway : IEnumerable<Vertex>
    {
        public Pathway(ICollection<Vertex> vertices, long distance)
        {
            Vertices = vertices;
            Distance = distance;
        }

        public ICollection<Vertex> Vertices { get; set; }

        public long Distance { get; set; }

        public IEnumerator<Vertex> GetEnumerator()
        {
            return Vertices.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class MinimumSpanTree : IEnumerable<Edge>
    {
        public MinimumSpanTree(ICollection<Edge> edges, int distance)
        {
            Edges = edges;
            Distance = distance;
        }

        public ICollection<Edge> Edges { get; }

        public int Distance { get; set; }

        public IEnumerator<Edge> GetEnumerator()
        {
            return Edges.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class MultipleMinimumSpanningTreesException : Exception
    {
    }

    public class NotDirectlyAcyclicGraphException : Exception
    {
    }

    public class NegativeCostCycleException : Exception
    {
    }
}