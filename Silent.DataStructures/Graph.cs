using System.Collections.Generic;
using System.Linq;

namespace Silent.Collections
{
    public class Graph
    {
        private readonly Dictionary<string, Vertex> _vertices;
        private readonly Dictionary<(string, string), Edge> _edges;

        private ICollection<Vertex> _cachedVertices;
        private ICollection<Edge> _cachedEdges;

        public Graph()
        {
            _vertices = new Dictionary<string, Vertex>();
            _edges = new Dictionary<(string, string), Edge>();
        }

        public Graph(IEnumerable<Vertex> vertices, IEnumerable<Edge> edges)
        {
            _vertices = new Dictionary<string, Vertex>();
            _edges = new Dictionary<(string, string), Edge>();

            AddVertices(vertices);
            AddEdges(edges);
        }

        public Vertex this[string label] => _vertices[label];

        public Edge this[string startLabel, string endLabel] => _edges[(startLabel, endLabel)];

        public Edge this[Vertex strartVertex, Vertex endVertex] => this[strartVertex.Label, endVertex.Label];

        public ICollection<Vertex> Vertices => _cachedVertices ?? (_cachedVertices = _vertices.Values);

        public ICollection<Edge> Edges => _cachedEdges ?? (_cachedEdges = _edges.Values);

        public bool SetVertex(Vertex vertex)
        {
            if (vertex == null)
                return false;

            _vertices[vertex.Label] = vertex;
            _cachedVertices = null;

            return true;
        }

        public bool SetEdge(Edge edge)
        {
            if (edge == null)
                return false;

            var key = (edge.StartVertex.Label, edge.EndVertex.Label);
            _edges[key] = edge;
            _cachedEdges = null;

            return true;
        }

        public bool ContainsVertex(string label) => _vertices.ContainsKey(label);

        public bool ContainsEdge(string startLabel, string endLabel) => _edges.ContainsKey((startLabel, endLabel));

        private bool AddVertices(IEnumerable<Vertex> vertices) => vertices.Aggregate(true, (current, vertex) => current & SetVertex(vertex));

        private bool AddEdges(IEnumerable<Edge> edges) => edges.Aggregate(true, (current, edge) => current & SetEdge(edge));
    }
}