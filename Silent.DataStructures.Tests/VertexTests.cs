using Xunit;
using Common.DataStructures;

namespace CommonTests
{
    public class VertexTests
    {
        [Fact]
        public void Vertex_Equals_IsFalse_Test()
        {
            // Assign
            var vertexA = new Vertex("a");
            var vertexB = new Vertex("b");

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
            var vertexA = new Vertex("a");
            var vertexB = new Vertex("a");

            // Act
            bool actual = vertexA.Equals(vertexB);

            // Assert
            bool expected = true;
            Assert.Equal(expected, actual);
        }
    }
}
