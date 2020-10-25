using Xunit;

namespace Silent.Collections.Tests.Graphs
{
    public class GridGraphTests
    {
        [Fact]
        public void Empty_ShouldReturnNotNullInstance()
        {
            // Arrange
            var graph = GridGraph<int>.Empty();

            // Act, Assert
            Assert.NotNull(graph);
        }

        [Fact]
        public void Empty_CalledTwice_ShouldReturnNotSameInstances()
        {
            // Arrange
            var graph1 = GridGraph<int>.Empty();
            var graph2 = GridGraph<int>.Empty();

            // Act, Assert
            Assert.NotSame(graph1, graph2);
        }

        [Fact]
        public void SetVertex_WithNullPosition_ShouldReturNullVertex()
        {
            // Arrange
            var graph = GridGraph<int>.FromSize(5, 5);

            // Act
            var vertex = graph.SetVertex(null, 1);

            // Assert
            Assert.Null(vertex);
            Assert.Empty(graph.Vertices);
        }

        [Fact]
        public void SetVertex_WithNotNullPosition_ShouldReturnNotNullVertex()
        {
            // Arrange
            var graph = GridGraph<int>.FromSize(5, 5);
            var position = new Position(1, 1);

            // Act
            var vertex = graph.SetVertex(position, 1);

            // Assert
            Assert.NotNull(vertex);
            Assert.NotNull(graph.Vertices);
            Assert.Equal(1, graph.Vertices.Count);
        }

        [Fact]
        public void SetVertex_AsNeighbors_ShouldSetEdgesAutomatically()
        {
            // Arrange
            var graph = GridGraph<int>.FromSize(5, 5);
            var sourcePosition = new Position(1, 1);
            var targetPosition = new Position(2, 2);

            // Act
            var sourceVertex = graph.SetVertex(sourcePosition, 1);
            var targetVertex = graph.SetVertex(targetPosition, 1);

            // Assert
            Assert.NotNull(graph.Edges);
            Assert.Equal(2, graph.Vertices.Count);
            Assert.NotNull(sourceVertex.OutboundEdges);
            Assert.Equal(1, sourceVertex.OutboundEdges.Count);
            Assert.Equal(1, sourceVertex.InboundEdges.Count);
            Assert.Equal(1, targetVertex.InboundEdges.Count);
            Assert.Equal(1, targetVertex.OutboundEdges.Count);
        }

        [Fact]
        public void SetEdge_WithNullPosition_ShouldReturNullVertex()
        {
            // Arrange
            var graph = GridGraph<int>.FromSize(5, 5);

            // Act
            var edge = graph.SetEdge(null, null, 1);

            // Assert
            Assert.Null(edge);
            Assert.Empty(graph.Edges);
        }

        [Fact]
        public void SetEdge_WithNotNullPosition_ShouldReturnNotNullEdge()
        {
            // Arrange
            var graph = GridGraph<int>.FromSize(5, 5);
            var sourcePosition = new Position(1, 1);
            var targetPosition = new Position(2, 2);

            // Act
            var sourceVertex = graph.SetVertex(sourcePosition, 1);
            var targetVertex = graph.SetVertex(targetPosition, 1);
            var edge = graph.SetEdge(sourcePosition, targetPosition, 1);

            // Assert
            Assert.NotNull(edge);
            Assert.NotNull(graph.Edges);
            Assert.Equal(2, graph.Vertices.Count);
            Assert.Same(sourceVertex, edge.StartVertex);
            Assert.Same(targetVertex, edge.EndVertex);
            Assert.Equal(1, edge.Weight);
            Assert.NotNull(sourceVertex.OutboundEdges);
            Assert.Equal(1, sourceVertex.OutboundEdges.Count);
            Assert.Equal(1, sourceVertex.InboundEdges.Count);
            Assert.Equal(1, targetVertex.InboundEdges.Count);
            Assert.Equal(1, targetVertex.OutboundEdges.Count);
        }
    }
}
