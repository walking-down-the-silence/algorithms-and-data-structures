using System;

namespace Common.DataStructures
{
    public class Edge : IEquatable<Edge>
    {
        public Edge(Vertex startVertex, Vertex endVertex, int weight)
        {
            StartVertex = startVertex;
            EndVertex = endVertex;
            Weight = weight;

            StartVertex.OutboundEdges.Add(this);
            EndVertex.InboundEdges.Add(this);
        }

        public Vertex StartVertex { get; }

        public Vertex EndVertex { get; }

        public int Weight { get; }

        public bool Equals(Edge other)
        {
            return other != null
                   && StartVertex.Equals(other.StartVertex)
                   && EndVertex.Equals(other.EndVertex)
                   && Weight.Equals(other.Weight);
        }

        public override int GetHashCode() => StartVertex.GetHashCode() ^ EndVertex.GetHashCode() ^ Weight;

        public override bool Equals(object obj) => Equals(obj as Edge);
    }
}