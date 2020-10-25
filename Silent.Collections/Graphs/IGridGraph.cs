using System;
using System.Collections.Generic;

namespace Silent.Collections
{
    public interface IGridGraph<T> where T : IEquatable<T>
    {
        Vertex<T> this[int row, int column] { get; }

        IReadOnlyCollection<Vertex<T>> Vertices { get; }

        IReadOnlyCollection<Edge<T>> Edges { get; }

        Vertex<T> SetVertex(Position position, T value);

        Edge<T> SetEdge(Position sourcePosition, Position targetPosition, int weight);

        bool RemoveVertex(Position position);

        bool RemoveEdge(Position sourcePosition, Position targetPosition);
    }
}
