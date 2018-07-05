using System;
using System.Collections.Generic;

namespace Silent.Collections
{
    public class Vertex : IEquatable<Vertex>
    {
        public Vertex(string label)
        {
            Label = label;
            InboundEdges = new List<Edge>();
            OutboundEdges = new List<Edge>();
        }

        public string Label { get; set; }

        public List<Edge> InboundEdges { get; }

        public List<Edge> OutboundEdges { get; }

        public bool Equals(Vertex other)
        {
            return other != null
                   && Label.Equals(other.Label)
                   && InboundEdges.Count.Equals(other.InboundEdges.Count)
                   && OutboundEdges.Count.Equals(other.OutboundEdges.Count);
        }

        public override int GetHashCode() => Label.GetHashCode();

        public override bool Equals(object obj) => Equals(obj as Vertex);
    }
}