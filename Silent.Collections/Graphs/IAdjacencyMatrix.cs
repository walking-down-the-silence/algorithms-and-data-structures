using System;

namespace Silent.Collections
{
    public interface IAdjacencyMatrix<T> where T : IEquatable<T>
    {
        Edge<T> this[T startLabel, T endLabel] { get; }
    }
}
