using System.Linq;
using Xunit;

namespace Silent.Collections.Tests
{
    public class GraphTests
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
            Assert.Equal(3, graph.Vertices.Count());
            Assert.Equal(3, graph.Edges.Count());
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
            Assert.Equal(3, graph.Vertices.Count());
            Assert.Equal(3, graph.Edges.Count());
        }
    }
}