using System;

namespace Silent.Collections
{
    public class Edge<T> : IEquatable<Edge<T>> where T : IEquatable<T>
    {
        public Edge(Vertex<T> startVertex, Vertex<T> endVertex, int weight)
        {
            StartVertex = startVertex;
            EndVertex = endVertex;
            Weight = weight;

            StartVertex.OutboundEdges.Add(this);
            EndVertex.InboundEdges.Add(this);
        }

        public Vertex<T> StartVertex { get; }

        public Vertex<T> EndVertex { get; }

        public int Weight { get; }

        public override int GetHashCode() => StartVertex.GetHashCode() ^ EndVertex.GetHashCode() ^ Weight;

        public override bool Equals(object obj) => Equals(obj as Edge<T>);

        public bool Equals(Edge<T> other)
        {
            return other != null
                   && StartVertex.Equals(other.StartVertex)
                   && EndVertex.Equals(other.EndVertex)
                   && Weight.Equals(other.Weight);
        }
    }
}