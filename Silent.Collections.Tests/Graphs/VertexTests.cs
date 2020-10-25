using System;
using Xunit;

namespace Silent.Collections.Tests
{
    public class VertexTests
    {
        [Fact]
        public void Vertex_Empty_ShouldNotBeNull()
        {
            // Assert
            Assert.NotNull(Vertex<int>.Empty);
        }

        [Fact]
        public void Vertex_Empty_ShouldBeSameInstance()
        {
            // Arrange
            var empty1 = Vertex<int>.Empty;
            var empty2 = Vertex<int>.Empty;

            // Assert
            Assert.Same(empty1, empty2);
        }

        [Fact]
        public void Vertex_WithNullValue_ShouldThrowArgumentNullException()
        {
            // Arrange
            var action = new Action(() => new Vertex<string>(null));

            // Act, Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void Vertex_Equals_IsFalse_Test()
        {
            // Assign
            var vertexA = new Vertex<string>("a");
            var vertexB = new Vertex<string>("b");

            // Act
            bool actual = vertexA.Equals(vertexB);

            // Assert
            bool expected = false;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Vertex_Equals_IsTrue_Test()
        {
            // Assign
            var vertexA = new Vertex<string>("a");
            var vertexB = new Vertex<string>("a");

            // Act
            bool actual = vertexA.Equals(vertexB);

            // Assert
            bool expected = true;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetOutboundEdge_WithNull_ShouldReturnNull()
        {
            // Arrange
            var sourceVertex = new Vertex<int>(1);

            // Act
            var edge = sourceVertex.SetOutboundEdge(null, 0);

            // Assert
            Assert.Null(edge);
        }

        [Fact]
        public void SetOutboundEdge_WithTargetNotNullVertex_ShouldReturnNotNullEdge()
        {
            // Arrange
            var sourceVertex = new Vertex<int>(1);
            var targetVertex = new Vertex<int>(1);

            // Act
            var edge = sourceVertex.SetOutboundEdge(targetVertex, 0);

            // Assert
            Assert.NotNull(edge);
            Assert.Same(sourceVertex, edge.StartVertex);
            Assert.Same(targetVertex, edge.EndVertex);
        }

        [Fact]
        public void SetInboundEdge_WithNull_ShouldReturnNull()
        {
            // Arrange
            var sourceVertex = new Vertex<int>(1);

            // Act
            var edge = sourceVertex.SetInboundEdge(null, 0);

            // Assert
            Assert.Null(edge);
        }

        [Fact]
        public void SetInboundEdge_WithTargetNotNullVertex_ShouldReturnNotNullEdge()
        {
            // Arrange
            var sourceVertex = new Vertex<int>(1);
            var targetVertex = new Vertex<int>(1);

            // Act
            var edge = sourceVertex.SetInboundEdge(targetVertex, 0);

            // Assert
            Assert.NotNull(edge);
            Assert.Same(sourceVertex, edge.EndVertex);
            Assert.Same(targetVertex, edge.StartVertex);
        }
    }
}
