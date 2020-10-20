using System;
using System.Collections.Generic;
using System.Linq;

namespace Silent.Collections
{
    public class Vertex<T> : IEquatable<Vertex<T>> where T : IEquatable<T>
    {
        private readonly List<Edge<T>> _inboundEdges = new List<Edge<T>>();
        private readonly List<Edge<T>> _outboundEdges = new List<Edge<T>>();

        public Vertex(T value)
        {
            Value = value;
        }

        public T Value { get; set; }

        public IReadOnlyCollection<Edge<T>> InboundEdges => _inboundEdges;

        public IReadOnlyCollection<Edge<T>> OutboundEdges => _outboundEdges;

        public IEnumerable<Vertex<T>> Neighbors => OutboundEdges.Select(x => x.EndVertex);

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj) => Equals(obj as Vertex<T>);

        public void SetInboundEdge(Edge<T> edge) => _inboundEdges.Add(edge);

        public void SetOutboundEdge(Edge<T> edge) => _outboundEdges.Add(edge);

        public bool Equals(Vertex<T> other)
        {
            return other != null
                && Value.Equals(other.Value)
                && InboundEdges.Count == other.InboundEdges.Count
                && OutboundEdges.Count == other.OutboundEdges.Count;
        }
    }
}