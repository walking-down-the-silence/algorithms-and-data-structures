using System;
using System.Collections.Generic;
using System.Linq;

namespace Silent.Collections
{
    public class PathwayCollection<T> where T : IEquatable<T>
    {
        private readonly IDictionary<Vertex<T>, Vertex<T>> _roadmap;

        private readonly IDictionary<Vertex<T>, long> _distances;

        private readonly IDictionary<Vertex<T>, Pathway<T>> _pathways = new Dictionary<Vertex<T>, Pathway<T>>();

        public PathwayCollection(IDictionary<Vertex<T>, Vertex<T>> roadmap, IDictionary<Vertex<T>, long> distances, Vertex<T> startVertex)
        {
            _roadmap = roadmap;
            _distances = distances;
            StartVertex = startVertex;
        }

        public Pathway<T> this[Vertex<T> endVertex]
        {
            get
            {
                if (!_pathways.ContainsKey(endVertex))
                {
                    _pathways[endVertex] = ReconstructPath(endVertex);
                }

                return _pathways[endVertex];
            }
        }

        public Vertex<T> StartVertex { get; }

        private Pathway<T> ReconstructPath(Vertex<T> endVertex)
        {
            var predcessor = _roadmap[endVertex];
            var pathVertices = new Stack<Vertex<T>>();
            pathVertices.Push(endVertex);

            while (predcessor != null)
            {
                pathVertices.Push(predcessor);
                predcessor = _roadmap.ContainsKey(predcessor) ? _roadmap[predcessor] : null;
            }

            return new Pathway<T>(pathVertices.ToList(), _distances[endVertex], AlgorithmState.PathFound);
        }
    }
}