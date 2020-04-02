using System;
using System.Collections.Generic;
using System.Linq;

namespace Silent.Collections
{
    public class DirectedGraph<T> : IDirectedGraph<T> where T : IEquatable<T>
    {
        private readonly Dictionary<T, Vertex<T>> _vertices;
        private readonly Dictionary<(T, T), Edge<T>> _edges;

        private ICollection<Vertex<T>> _cachedVertices;
        private ICollection<Edge<T>> _cachedEdges;

        public DirectedGraph()
        {
            _vertices = new Dictionary<T, Vertex<T>>();
            _edges = new Dictionary<(T, T), Edge<T>>();
        }

        public DirectedGraph(IEnumerable<Vertex<T>> vertices, IEnumerable<Edge<T>> edges)
        {
            _vertices = new Dictionary<T, Vertex<T>>();
            _edges = new Dictionary<(T, T), Edge<T>>();

            AddVertices(vertices);
            AddEdges(edges);
        }

        public Vertex<T> this[T value] => _vertices.ContainsKey(value) ? _vertices[value] : default;

        public Edge<T> this[T startLabel, T endLabel] => _edges.ContainsKey((startLabel, endLabel)) ? _edges[(startLabel, endLabel)] : default;

        public ICollection<Vertex<T>> Vertices => _cachedVertices ?? (_cachedVertices = _vertices.Values);

        public ICollection<Edge<T>> Edges => _cachedEdges ?? (_cachedEdges = _edges.Values);

        public bool SetVertex(Vertex<T> vertex)
        {
            if (vertex == null)
                return false;

            _vertices[vertex.Value] = vertex;
            _cachedVertices = null;

            return true;
        }

        public bool SetEdge(Edge<T> edge)
        {
            if (edge == null)
                return false;

            var key = (edge.StartVertex.Value, edge.EndVertex.Value);
            _edges[key] = edge;
            _cachedEdges = null;

            return true;
        }

        public IAdjacencyMatrix<T> ToAdjacencyMatrix() => this;

        private bool AddVertices(IEnumerable<Vertex<T>> vertices) => vertices.Aggregate(true, (current, vertex) => current && SetVertex(vertex));

        private bool AddEdges(IEnumerable<Edge<T>> edges) => edges.Aggregate(true, (current, edge) => current && SetEdge(edge));
    }
}