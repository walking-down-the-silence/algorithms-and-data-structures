using Xunit;

namespace Silent.Collections.Tests.Graphs
{
    public class DirectedGraphTests
    {
        [Fact]
        public void Parse_WithoutWeight_ShouldParse_Test()
        {
            // Assign
            var lines = new[] { "1 2", "2 3", "3 1" };
            var graph = new DirectedGraph<string>();

            // Act
            graph.Parse(lines, true);

            // Assert
            Assert.Equal(3, graph.Vertices.Count);
            Assert.Equal(3, graph.Edges.Count);
        }

        [Fact]
        public void Parse_WithWeight_ShouldParse_Test()
        {
            // Assign
            var lines = new[] { "1 2 10", "2 3 15", "3 1 20" };
            var graph = new DirectedGraph<string>();

            // Act
            graph.Parse(lines, true);

            // Assert
            Assert.Equal(3, graph.Vertices.Count);
            Assert.Equal(3, graph.Edges.Count);
        }

        [Fact]
        public void SetVertex_WithVertex_ShouldHaveExactly1Vertex()
        {
            // Arrange
            var vertex = new Vertex<int>(1);
            var graph = new DirectedGraph<int>();

            // Act
            graph.SetVertex(vertex);

            // Assert
            Assert.NotNull(graph.Vertices);
            Assert.Equal(1, graph.Vertices.Count);
        }

        [Fact]
        public void RemoveVertex_AfterAddingSingleVertex_ShouldHaveNoVertices()
        {
            // Arrange
            var vertex = new Vertex<int>(1);
            var graph = new DirectedGraph<int>();

            // Act
            graph.SetVertex(vertex);
            graph.RemoveVertex(vertex);

            // Assert
            Assert.NotNull(graph.Vertices);
            Assert.Empty(graph.Vertices);
        }

        [Fact]
        public void SetEdge_WithSingleEdge_ShouldHaveExactly1Vertex()
        {
            // Arrange
            var sourceVertex = new Vertex<int>(1);
            var targetVertex = new Vertex<int>(2);
            var edge = new Edge<int>(sourceVertex, targetVertex, 1);
            var graph = new DirectedGraph<int>();

            // Act
            graph.SetVertex(sourceVertex);
            graph.SetVertex(targetVertex);
            graph.SetEdge(edge);

            // Assert
            Assert.NotNull(graph.Vertices);
            Assert.Equal(2, graph.Vertices.Count);
            Assert.NotNull(graph.Edges);
            Assert.Equal(1, graph.Edges.Count);
        }

        [Fact]
        public void RemoveEdge_WithSingleEdge_ShouldHaveExactly1Vertex()
        {
            // Arrange
            var sourceVertex = new Vertex<int>(1);
            var targetVertex = new Vertex<int>(2);
            var edge = new Edge<int>(sourceVertex, targetVertex, 1);
            var graph = new DirectedGraph<int>();

            // Act
            graph.SetVertex(sourceVertex);
            graph.SetVertex(targetVertex);
            graph.SetEdge(edge);
            graph.RemoveEdge(edge);

            // Assert
            Assert.NotNull(graph.Vertices);
            Assert.Equal(2, graph.Vertices.Count);
            Assert.NotNull(graph.Edges);
            Assert.Empty(graph.Edges);
        }
    }
}
