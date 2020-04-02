using System;
using System.Collections.Generic;
using System.Linq;

namespace Silent.Collections
{
    public class Vertex<T> : IEquatable<Vertex<T>> where T: IEquatable<T>
    {
        public Vertex(T value)
        {
            Value = value;
            InboundEdges = new List<Edge<T>>();
            OutboundEdges = new List<Edge<T>>();
        }

        public T Value { get; set; }

        public List<Edge<T>> InboundEdges { get; }

        public List<Edge<T>> OutboundEdges { get; }

        public IEnumerable<Vertex<T>> Neighbors => OutboundEdges.Select(x => x.EndVertex);

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj) => Equals(obj as Vertex<T>);

        public bool Equals(Vertex<T> other)
        {
            return other != null
                && Value.Equals(other.Value)
                && InboundEdges.Count == other.InboundEdges.Count
                && OutboundEdges.Count == other.OutboundEdges.Count;
        }
    }
}