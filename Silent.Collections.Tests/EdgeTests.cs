using Xunit;

namespace Silent.Collections.Tests
{
    public class EdgeTests
    {
        [Fact]
        public void Ctor_WithParameters_ShouldSetItselfAsParent_Test()
        {
            // Assign
            var vertexA = new Vertex<string>("a");
            var vertexB = new Vertex<string>("b");
            var edge = new Edge<string>(vertexA, vertexB, 0);

            // Act
            bool actual = vertexA.OutboundEdges.Contains(edge);
            actual &= vertexB.InboundEdges.Contains(edge);

            // Assert
            bool expected = true;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Equals_WithDifferentVertices_IsFalse_Test()
        {
            // Assign
            var vertexA = new Vertex<string>("a");
            var vertexB = new Vertex<string>("b");
            var edgeA = new Edge<string>(vertexA, vertexA, 0);
            var edgeB = new Edge<string>(vertexB, vertexB, 0);

            // Act
            bool actual = edgeA.Equals(edgeB);

            // Assert
            bool expected = false;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Equals_WithCrossDifferentVertices_IsFalse_Test()
        {
            // Assign
            var vertexA = new Vertex<string>("a");
            var vertexB = new Vertex<string>("b");
            var edgeA = new Edge<string>(vertexA, vertexB, 0);
            var edgeB = new Edge<string>(vertexB, vertexA, 0);

            // Act
            bool actual = edgeA.Equals(edgeB);

            // Assert
            bool expected = false;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Equals_WithDifferentWeight_IsFalse_Test()
        {
            // Assign
            var vertexA = new Vertex<string>("a");
            var vertexB = new Vertex<string>("b");
            var edgeA = new Edge<string>(vertexA, vertexB, 10);
            var edgeB = new Edge<string>(vertexA, vertexB, 20);

            // Act
            bool actual = edgeA.Equals(edgeB);

            // Assert
            bool expected = false;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Equals_WithSameVerticesAndWeight_IsTrue_Test()
        {
            // Assign
            var vertexA = new Vertex<string>("a");
            var vertexB = new Vertex<string>("b");
            var edgeA = new Edge<string>(vertexA, vertexB, 0);
            var edgeB = new Edge<string>(vertexA, vertexB, 0);

            // Act
            bool actual = edgeA.Equals(edgeB);

            // Assert
            bool expected = true;
            Assert.Equal(expected, actual);
        }
    }
}