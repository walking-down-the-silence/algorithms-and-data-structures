using System;
using System.Collections.Generic;
using System.Linq;

namespace Silent.Collections
{
    public static class DirectedGraphExtensions
    {
        public static bool ContainsVertex<T>(this IDirectedGraph<T> graph, T value) where T : IEquatable<T>
        {
            return graph[value] != default;
        }

        public static bool ContainsEdge<T>(this IAdjacencyMatrix<T> graph, T startValue, T endValue) where T : IEquatable<T>
        {
            return graph[startValue, endValue] != default;
        }

        /// <summary>
        /// Parses the input lines of string and fills the <see cref="Graph"/> instance
        /// </summary>
        /// <param name="graph"> <see cref="Graph"/> instance. </param>
        /// <param name="lines"> Lines of strings to parse. </param>
        /// <param name="directedGraph"> Defines if parsed edges should be threated as directed. </param>
        public static void Parse(this DirectedGraph<string> graph, string[] lines, bool directedGraph = false)
        {
            foreach (string line in lines)
            {
                var labels = line.Split(' ');
                var startLabel = labels[0];
                var endLabel = labels[1];
                var weight = labels.Length > 2 ? int.Parse(labels[2]) : 1;

                if (!graph.ContainsVertex(startLabel))
                {
                    graph.SetVertex(new Vertex<string>(startLabel));
                }

                if (!graph.ContainsVertex(endLabel))
                {
                    graph.SetVertex(new Vertex<string>(endLabel));
                }

                var startVertex = graph[startLabel];
                var endVertex = graph[endLabel];

                graph.SetEdge(new Edge<string>(startVertex, endVertex, weight));

                if (!directedGraph)
                {
                    graph.SetEdge(new Edge<string>(endVertex, startVertex, weight));
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
        public static IEnumerable<Vertex<T>> BreadthFirstSearch<T>(this DirectedGraph<T> graph, Vertex<T> startVertex, Vertex<T> endVertex)
            where T : IEquatable<T>
        {
            var result = new List<Vertex<T>>();
            var visited = new HashSet<Vertex<T>>();
            var queue = new Queue<Vertex<T>>();
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
        public static IEnumerable<Vertex<T>> DepthFirstSearch<T>(this DirectedGraph<T> graph, Vertex<T> startVertex, Vertex<T> endVertex)
            where T : IEquatable<T>
        {
            var result = new List<Vertex<T>>();
            var visited = new HashSet<Vertex<T>>();
            var stack = new Stack<Vertex<T>>();
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
        public static Pathway<T> Dijkstra<T>(this DirectedGraph<T> graph, Vertex<T> startVertex, Vertex<T> endVertex) where T : IEquatable<T>
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
        public static PathwayCollection<T> Dijkstra<T>(this DirectedGraph<T> graph, Vertex<T> startVertex) where T : IEquatable<T>
        {
            var distances = graph.Vertices.ToDictionary(key => key, value => long.MaxValue);
            distances[startVertex] = 0;
            var distancesMinimumHeap = new BinaryHeap<VertexDistancePair<T>>(
                BinaryHeapType.MinimumHeap,
                graph.Vertices.Count,
                new VertexDistancePairComparer<T>());
            distancesMinimumHeap.Add(new VertexDistancePair<T>(startVertex, 0));

            var pathVertices = new Dictionary<Vertex<T>, Vertex<T>>();

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
                        distancesMinimumHeap.Add(new VertexDistancePair<T>(outboundEndVertex, alternativeDistance));
                    }
                }
            }

            return new PathwayCollection<T>(pathVertices, distances, startVertex);
        }

        /// <summary>
        /// Bellman-Ford's algorithm for finding shortest paths in a graph and detect negative cost cycles
        /// </summary>
        /// <param name="graph"> <see cref="Graph"/> instance. </param>
        /// <param name="startVertex"> The start <see cref="Vertex"/> of path to calculate minimum distance for. </param>
        /// <returns> The shortest (minimum cost) path from starting point to all other. </returns>
        public static PathwayCollection<T> BellmanFord<T>(this DirectedGraph<T> graph, Vertex<T> startVertex) where T : IEquatable<T>
        {
            var distances = graph.Vertices.ToDictionary(key => key, value => long.MaxValue);
            distances[startVertex] = 0;
            var pathVertices = new Dictionary<Vertex<T>, Vertex<T>>();

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

            return new PathwayCollection<T>(pathVertices, distances, startVertex);
        }

        /// <summary>
        /// Floyd–Warshall algorithm for finding shortest paths in a weighted graph with positive or negative edge weights (but with no negative cycles)
        /// </summary>
        /// <param name="graph"> <see cref="Graph"/> instance. </param>
        /// <returns> The shortest (minimum cost) path from any vertexs to all other vertices. </returns>
        public static Roadmap<T> FloydWarshall<T>(this DirectedGraph<T> graph) where T : IEquatable<T>
        {
            var distances = new Dictionary<Tuple<Vertex<T>, Vertex<T>>, long>();

            foreach (var vertexI in graph.Vertices)
            {
                foreach (var vertexK in graph.Vertices)
                {
                    var key = new Tuple<Vertex<T>, Vertex<T>>(vertexI, vertexK);
                    distances[key] = vertexI.Equals(vertexK) ? 0 : long.MaxValue;
                }
            }

            foreach (var edge in graph.Edges)
            {
                var key = new Tuple<Vertex<T>, Vertex<T>>(edge.StartVertex, edge.EndVertex);
                distances[key] = edge.Weight;
            }

            foreach (var vertexK in graph.Vertices)
            {
                foreach (var vertexI in graph.Vertices)
                {
                    foreach (var vertexJ in graph.Vertices)
                    {
                        var keyIJ = new Tuple<Vertex<T>, Vertex<T>>(vertexI, vertexJ);
                        var keyIK = new Tuple<Vertex<T>, Vertex<T>>(vertexI, vertexK);
                        var keyKJ = new Tuple<Vertex<T>, Vertex<T>>(vertexK, vertexJ);

                        if (distances[keyIJ] > distances[keyIK] + distances[keyKJ])
                        {
                            distances[keyIJ] = distances[keyIK] + distances[keyKJ];
                        }
                    }
                }
            }

            return new Roadmap<T>(distances);
        }

        #endregion

        /// <summary>
        /// Tarjans algorithm to topologically sort the graph
        /// </summary>
        /// <param name="graph"> <see cref="Graph"/> instance. </param>
        /// <param name="recursive"> Defines if Tarjan's algorithm should perform a recursive or stack-based search. </param>
        /// <returns> The topologically sorted strongly connected conponents. </returns>
        public static IEnumerable<Vertex<T>> Tarjan<T>(this DirectedGraph<T> graph, bool recursive = false) where T : IEquatable<T>
        {
            TarjanDepthFirstSearchDelegate<T> tarjanDfs = recursive
                ? new TarjanDepthFirstSearchDelegate<T>(TarjanDfsRecursive)
                : new TarjanDepthFirstSearchDelegate<T>(TarjanDfsStack);

            var topologicalOrderSet = new HashSet<Vertex<T>>();
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
        public static MinimumSpanTree<T> PrimsMinimumSpanningTree<T>(this DirectedGraph<T> graph) where T : IEquatable<T>
        {
            if (graph.Vertices.Count == 0 || graph.Edges.Count == 0)
                return new MinimumSpanTree<T>(Enumerable.Empty<Edge<T>>().ToList(), 0);

            var currentVertex = graph.Vertices.First();
            int minimumDistance = 0;
            var minimumSpanTree = new List<Edge<T>>();
            var edgesToVisit = new BinaryHeap<Edge<T>>(BinaryHeapType.MinimumHeap, currentVertex.OutboundEdges.Count, new EdgeComparer<T>());
            var verticesVisited = new HashSet<Vertex<T>>();

            while (minimumSpanTree.Count < graph.Vertices.Count - 1)
            {
                foreach (var edge in currentVertex.OutboundEdges)
                {
                    edgesToVisit.Add(edge);
                }

                verticesVisited.Add(currentVertex);
                Edge<T> minimumEdge = null;

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

            return new MinimumSpanTree<T>(minimumSpanTree, minimumDistance);
        }

        /// <summary>
        /// Kruskal's algorithm to find the minimum span tree on graph
        /// </summary>
        /// <param name="graph"> <see cref="Graph"/> instance. </param>
        /// <returns> Minimum span tree containing edges and minimum distance. </returns>
        public static MinimumSpanTree<T> KruskalsMinimumSpanningTree<T>(this DirectedGraph<T> graph) where T : IEquatable<T>
        {
            if (graph.Vertices.Count == 0 || graph.Edges.Count == 0)
                return new MinimumSpanTree<T>(Enumerable.Empty<Edge<T>>().ToList(), 0);

            var minimumSpanTree = new List<Edge<T>>();
            var minimumDistance = 0;
            var unionFind = new DisjointSet<Vertex<T>>(graph.Vertices);
            var edgesToVisit = new BinaryHeap<Edge<T>>(BinaryHeapType.MinimumHeap, graph.Edges.Count, new EdgeComparer<T>());

            foreach (var edge in graph.Edges)
            {
                edgesToVisit.Add(edge);
            }

            while (minimumSpanTree.Count < graph.Vertices.Count - 1)
            {
                Edge<T> minimumEdge = null;

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

            return new MinimumSpanTree<T>(minimumSpanTree, minimumDistance);
        }

        #endregion

        /// <summary>
        /// Recursive function for vertex traversal for Tarjans algorithm
        /// </summary>
        /// <param name="vertex"> <see cref="Vertex"/> instance to start traversing from. </param>
        /// <param name="topologicalOrder"> List of topologically sorted vertices so far. </param>
        /// <param name="visitedVertices"> Map of vertices status traversed so far. </param>
        /// <returns> Returns true if graph is a Directed Acyclic Graph. </returns>
        private static bool TarjanDfsRecursive<T>(
            Vertex<T> vertex,
            ICollection<Vertex<T>> topologicalOrder,
            Dictionary<Vertex<T>, TarjansVisitStatus> visitedVertices)
            where T : IEquatable<T>
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
        private static bool TarjanDfsStack<T>(
            Vertex<T> vertex,
            ICollection<Vertex<T>> topologicalOrderSet,
            Dictionary<Vertex<T>, TarjansVisitStatus> visitedVertices)
            where T : IEquatable<T>
        {
            var stack = new Stack<Vertex<T>>();
            stack.Push(vertex);

            while (stack.Count > 0)
            {
                var currentVertex = stack.Pop();
                visitedVertices[currentVertex] = TarjansVisitStatus.Visited;
                var nextVertices = new List<Vertex<T>>();

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

        private delegate bool TarjanDepthFirstSearchDelegate<T>(
            Vertex<T> vertex,
            ICollection<Vertex<T>> topologicalOrder,
            Dictionary<Vertex<T>, TarjansVisitStatus> visitedVertices)
            where T : IEquatable<T>;

        /// <summary>
        /// Vertex visit status for Tarjan's algorithm
        /// </summary>
        private enum TarjansVisitStatus
        {
            NotVisited = 0,
            Visited = 1,
            Resolved = 2
        }

        private class EdgeComparer<T> : IComparer<Edge<T>> where T : IEquatable<T>
        {
            public int Compare(Edge<T> x, Edge<T> y)
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

        private class VertexDistancePairComparer<T> : IComparer<VertexDistancePair<T>> where T : IEquatable<T>
        {
            public int Compare(VertexDistancePair<T> x, VertexDistancePair<T> y)
            {
                if (x.Distance < y.Distance)
                {
                    return -1;
                }

                return x.Distance > y.Distance ? 1 : 0;
            }
        }

        private class VertexDistancePair<T> where T : IEquatable<T>
        {
            public VertexDistancePair(Vertex<T> vertex, long distance)
            {
                Vertex = vertex;
                Distance = distance;
            }

            public Vertex<T> Vertex { get; set; }

            public long Distance { get; set; }
        }
    }
}