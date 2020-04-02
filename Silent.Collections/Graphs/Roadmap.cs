using System;
using System.Collections.Generic;

namespace Silent.Collections
{
    public class Roadmap<T> where T : IEquatable<T>
    {
        private readonly IDictionary<Tuple<Vertex<T>, Vertex<T>>, long> _roadmap;

        public Roadmap(IDictionary<Tuple<Vertex<T>, Vertex<T>>, long> roadmap)
        {
            _roadmap = roadmap;
        }

        public long this[Vertex<T> startVertex, Vertex<T> endVertex]
        {
            get
            {
                var key = new Tuple<Vertex<T>, Vertex<T>>(startVertex, endVertex);
                return _roadmap[key];
            }
        }
    }
}