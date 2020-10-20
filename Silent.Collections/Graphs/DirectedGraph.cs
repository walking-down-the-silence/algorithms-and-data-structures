using System;
using System.Collections.Generic;
using System.Linq;

namespace Silent.Collections
{
    public class DirectedGraph<T> : IDirectedGraph<T> where T : IEquatable<T>
    {
        private readonly Dictionary<T, Vertex<T>> _vertices;
        private readonly Dictionary<(T, T), Edge<T>> _edges;

        private IReadOnlyCollection<Vertex<T>> _cachedVertices;
        private IReadOnlyCollection<Edge<T>> _cachedEdges;

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

        public Vertex<T> this[T value]
        {
            get
            {
                return _vertices.ContainsKey(value)
                    ? _vertices[value]
                    : default;
            }
        }

        public Edge<T> this[T startLabel, T endLabel]
        {
            get
            {
                return _edges.ContainsKey((startLabel, endLabel))
                    ? _edges[(startLabel, endLabel)]
                    : default;
            }
        }

        public IReadOnlyCollection<Vertex<T>> Vertices => _cachedVertices ?? (_cachedVertices = _vertices.Values);

        public IReadOnlyCollection<Edge<T>> Edges => _cachedEdges ?? (_cachedEdges = _edges.Values);

        public bool SetVertex(Vertex<T> vertex) => InternalSetVertex(vertex) != null;

        public bool SetEdge(Edge<T> edge) => InternalSetEdge(edge) != null;

        public bool RemoveVertex(Vertex<T> vertex)
        {
            return _vertices.Remove(vertex.Value)
                && vertex.InboundEdges.Aggregate(true, (successfull, edge) => successfull && InternalRemoveEdge(edge))
                && vertex.OutboundEdges.Aggregate(true, (successfull, edge) => successfull && InternalRemoveEdge(edge));
        }

        public bool RemoveEdge(Edge<T> edge) => InternalRemoveEdge(edge);

        public IAdjacencyMatrix<T> ToAdjacencyMatrix() => this;

        private Vertex<T> InternalSetVertex(Vertex<T> vertex)
        {
            if (vertex == null) return null;

            _cachedVertices = null;
            return (_vertices[vertex.Value] = vertex);
        }

        private Edge<T> InternalSetEdge(Edge<T> edge)
        {
            if (edge == null) return null;

            var key = (edge.StartVertex.Value, edge.EndVertex.Value);
            _cachedEdges = null;
            return (_edges[key] = edge);
        }

        private bool InternalRemoveEdge(Edge<T> edge)
        {
            if (edge == null) return false;

            var key = (edge.StartVertex.Value, edge.EndVertex.Value);
            return _edges.Remove(key);
        }

        private bool AddVertices(IEnumerable<Vertex<T>> vertices)
        {
            return vertices.Aggregate(true, (current, vertex) => current && SetVertex(vertex));
        }

        private bool AddEdges(IEnumerable<Edge<T>> edges)
        {
            return edges.Aggregate(true, (current, edge) => current && InternalSetEdge(edge) != null);
        }
    }
}