using System;
using System.Collections.Generic;

namespace Silent.Collections
{
    public interface IDirectedGraph<T> : IAdjacencyMatrix<T> where T : IEquatable<T>
    {
        Vertex<T> this[T value] { get; }

        ICollection<Vertex<T>> Vertices { get; }

        ICollection<Edge<T>> Edges { get; }

        bool SetVertex(Vertex<T> vertex);

        bool SetEdge(Edge<T> edge);
    }
}
