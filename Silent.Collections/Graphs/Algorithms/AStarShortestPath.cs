using System;
using System.Collections.Generic;

namespace Silent.Collections
{
    /// <summary>
    /// A* algorithm for serching the shortest distance path from source to target vertex.
    /// </summary>
    public class AStarShortestPath<T> : IShortestPathStrategy<T> where T : IEquatable<T>
    {
        private readonly IVertexDistanceMesureable<T> _distanceMesureable;

        public AStarShortestPath(IVertexDistanceMesureable<T> distanceMesureable)
        {
            _distanceMesureable = distanceMesureable ?? throw new ArgumentNullException(nameof(distanceMesureable));
        }

        /// <summary>
        /// A* algorithm that traverses the graph to find shortest path between set vertices
        /// </summary>
        /// <param name="graph"> <see cref="Graph"/> instance. </param>
        /// <param name="startVertex"> The start <see cref="Vertex"/> of path to calculate minimum distance for. </param>
        /// <param name="endVertex"> The end <see cref="Vertex"/> of path to calculate minimum distance for. </param>
        /// <returns> The shortest (minimum cost) path from starting point to ending point. </returns>
        public Pathway<T> FindPath(IAdjacencyMatrix<T> graph, Vertex<T> start, Vertex<T> target)
        {
            /// System.Collections.Generic.SortedList by default does not allow duplicate items.
            /// Since items are keyed by TotalCost there can be duplicate entries per key.
            var priorityComparer = Comparer<int>.Create((x, y) => (x <= y) ? -1 : 1);
            var opened = new PriorityQueue<int, Vertex<T>>(priorityComparer);
            var visited = new PriorityQueue<int, Vertex<T>>(priorityComparer);
            var path = new Dictionary<Vertex<T>, PathStep<T>>();

            // Resets the AStar algorithm with the newly specified start node and goal node.
            var current = start;
            int estimatedDistance = _distanceMesureable.MesureDistance(start, target);
            path[start] = new PathStep<T>(start, null, 0, estimatedDistance);
            opened.Enqueue(path[start].TotalCost, start);

            // Continue searching until either failure or the goal node has been found.
            AlgorithmState state = AlgorithmState.Searching;

            while (state == AlgorithmState.Searching)
            {
                current = GetNext(opened, visited);

                if (current == null)
                {
                    state = AlgorithmState.PathDoesNotExist;
                    break;
                }

                // Remove from the open list and place on the closed list 
                // since this node is now being searched.
                visited.Enqueue(path[current].TotalCost, current);

                // Found the goal, stop searching.
                if (current.Equals(target))
                {
                    state = AlgorithmState.PathFound;
                    break;
                }

                ExtendOpened(opened, visited, current, target, path);
            }

            ICollection<Vertex<T>> vertices = ReconstructPath(current, path);
            int totalDistance = current != null && path.ContainsKey(current) ? path[current].MovementCost : 0;

            return new Pathway<T>(vertices, totalDistance, state);
        }

        private void ExtendOpened(
            PriorityQueue<int, Vertex<T>> opened,
            PriorityQueue<int, Vertex<T>> visited,
            Vertex<T> current,
            Vertex<T> target,
            Dictionary<Vertex<T>, PathStep<T>> path)
        {
            foreach (var neighbor in current.Neighbors)
            {
                // If the child has already been visited (closed list) or is on
                // the open list to be searched then do not modify its movement cost
                // or estimated cost since they have already been set previously.
                if (!visited.ContainsValue(neighbor) && !opened.ContainsValue(neighbor))
                {
                    // Each child needs to have its movement cost set and estimated cost.
                    int estimatedDistance = _distanceMesureable.MesureDistance(neighbor, target);
                    path[neighbor] = new PathStep<T>(neighbor, current, path[current].MovementCost, estimatedDistance);
                    opened.Enqueue(path[neighbor].TotalCost, neighbor);
                }
            }
        }

        private static Vertex<T> GetNext(PriorityQueue<int, Vertex<T>> opened, PriorityQueue<int, Vertex<T>> visited)
        {
            while (!opened.IsEmpty())
            {
                // Check the next best node in the graph by TotalCost.
                var current = opened.Dequeue();

                // This node has already been searched, check the next one.
                if (!visited.ContainsValue(current))
                {
                    return current;
                }
            }

            return default;
        }

        /// <summary>
        /// Gets the path of the last solution of the AStar algorithm.
        /// Will return a partial path if the algorithm has not finished yet.
        /// </summary>
        /// <returns>Returns null if the algorithm has never been run.</returns>
        private ICollection<Vertex<T>> ReconstructPath(Vertex<T> current, Dictionary<Vertex<T>, PathStep<T>> path)
        {
            if (current != null)
            {
                var next = current;
                var vertices = new List<Vertex<T>>();

                while (next != null)
                {
                    vertices.Add(next);
                    next = path[next].Parent;
                }

                vertices.Reverse();
                return vertices;
            }

            return new Vertex<T>[0];
        }

        private class PathStep<T> where T : IEquatable<T>
        {
            public PathStep(Vertex<T> source, Vertex<T> parent, int parentCost, int estimatedCost)
            {
                Source = source;
                Parent = parent;
                // distance between grid cell is estimated at 1 point
                MovementCost = parentCost + 1;
                EstimatedCost = estimatedCost;
            }

            /// <summary>
            /// Gets of sets the source node for this info.
            /// </summary>
            public Vertex<T> Source { get; }

            /// <summary>
            /// Gets or sets the parent node to source node.
            /// </summary>
            public Vertex<T> Parent { get; }

            /// <summary>
            /// Gets the total cost for this node.
            /// f = g + h
            /// TotalCost = MovementCost + EstimatedCost
            /// </summary>
            public int TotalCost => MovementCost + EstimatedCost;

            /// <summary>
            /// Gets the movement cost for this node.
            /// This is the movement cost from this node to the starting node, or g.
            /// </summary>
            public int MovementCost { get; }

            /// <summary>
            /// Gets the estimated cost for this node.
            /// This is the heuristic from this node to the goal node, or h.
            /// </summary>
            public int EstimatedCost { get; }
        }
    }
}
