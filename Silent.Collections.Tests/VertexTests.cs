using Xunit;

namespace Silent.Collections.Tests
{
    public class VertexTests
    {
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
    }
}
