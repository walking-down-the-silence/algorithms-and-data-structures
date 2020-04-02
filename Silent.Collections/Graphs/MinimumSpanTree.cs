using System;
using System.Collections;
using System.Collections.Generic;

namespace Silent.Collections
{
    public class MinimumSpanTree<T> : IEnumerable<Edge<T>> where T : IEquatable<T>
    {
        public MinimumSpanTree(ICollection<Edge<T>> edges, int distance)
        {
            Edges = edges;
            Distance = distance;
        }

        public ICollection<Edge<T>> Edges { get; }

        public int Distance { get; set; }

        public IEnumerator<Edge<T>> GetEnumerator()
        {
            return Edges.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}