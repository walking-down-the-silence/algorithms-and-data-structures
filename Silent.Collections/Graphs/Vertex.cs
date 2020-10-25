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
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static Vertex<T> Empty { get; } = new Vertex<T>(default);

        public T Value { get; set; }

        public IReadOnlyCollection<Edge<T>> InboundEdges => _inboundEdges;

        public IReadOnlyCollection<Edge<T>> OutboundEdges => _outboundEdges;

        public IReadOnlyCollection<Vertex<T>> Neighbors => OutboundEdges.Select(x => x.EndVertex).ToList();

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj) => Equals(obj as Vertex<T>);

        public Edge<T> SetInboundEdge(Edge<T> edge)
        {
            if (edge != null)
            {
                _inboundEdges.Add(edge);
                return edge;
            }

            return null;
        }

        public Edge<T> SetOutboundEdge(Edge<T> edge)
        {
            if (edge != null)
            {
                _outboundEdges.Add(edge);
                return edge;
            }

            return null;
        }

        public Edge<T> SetInboundEdge(Vertex<T> sourceVertex, int weight)
        {
            if (sourceVertex != null)
            {
                var edge = new Edge<T>(sourceVertex, this, weight);
                _inboundEdges.Add(edge);
                return edge;
            }

            return null;
        }

        public Edge<T> SetOutboundEdge(Vertex<T> targetVertex, int weight)
        {
            if (targetVertex != null)
            {
                var edge = new Edge<T>(this, targetVertex, weight);
                _outboundEdges.Add(edge);
                return edge;
            }

            return null;
        }

        public bool RemoveInboundEdge(Edge<T> edge)
        {
            return edge != null && _inboundEdges.Remove(edge);
        }

        public bool RemoveOutboundEdge(Edge<T> edge)
        {
            return edge != null && _outboundEdges.Remove(edge);
        }

        public bool RemoveInboundEdge(Vertex<T> sourceVertex)
        {
            var edge = _inboundEdges.FirstOrDefault(x => x.StartVertex == sourceVertex);
            return edge != null && _inboundEdges.Remove(edge);
        }

        public bool RemoveOutboundEdge(Vertex<T> targetVertex)
        {
            var edge = _outboundEdges.FirstOrDefault(x => x.EndVertex == targetVertex);
            return edge != null && _outboundEdges.Remove(edge);
        }

        public bool Equals(Vertex<T> other)
        {
            return other != null
                && Value.Equals(other.Value)
                && InboundEdges.Count == other.InboundEdges.Count
                && OutboundEdges.Count == other.OutboundEdges.Count;
        }
    }
}