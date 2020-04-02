using System;

namespace Silent.Collections
{
    public interface IShortestPathStrategy<T> where T : IEquatable<T>
    {
        Pathway<T> FindPath(IAdjacencyMatrix<T> graph, Vertex<T> start, Vertex<T> target);
    }
}
