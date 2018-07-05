using System;
using System.Collections.Generic;
using System.Linq;
using Common.Algorithms;
using Common.DataStructures;
using Xunit;

namespace CommonTests
{
    public class GraphExtensionsTests
    {
        [Fact]
        public void BreadthFirstSearch_Test()
        {
            // Assign
            var graph = CreateTestTreeGraph();

            // Act
            var vertices = graph.BreadthFirstSearch(graph.Vertices.First(), graph.Vertices.Last());
            var actual = vertices.Select(vectex => vectex.Label).Aggregate((current, label) => current + label);

            // Assert
            var expected = "abcdefg";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DepthFirstSearch_Path_Test()
        {
            // Assign
            var graph = CreateTestTreeGraph();

            // Act
            var vertices = graph.DepthFirstSearch(graph.Vertices.First(), graph.Vertices.Last());
            var actual = vertices.Select(vectex => vectex.Label).Aggregate((current, label) => current + label);

            // Assert
            var expected = "acgfbed";
            Assert.Equal(expected, actual);
        }

        #region Dijkstra Path Tests

        [Fact]
        public void Dijkstra_From_A_To_B_Path_Test()
        {
            DijkstraPathTest("A", "B", "AB");
        }

        [Fact]
        public void Dijkstra_From_A_To_C_Path_Test()
        {
            DijkstraPathTest("A", "C", "ABC");
        }

        [Fact]
        public void Dijkstra_From_A_To_D_Path_Test()
        {
            DijkstraPathTest("A", "D", "AFED");
        }

        [Fact]
        public void Dijkstra_From_A_To_E_Path_Test()
        {
            DijkstraPathTest("A", "E", "AFE");
        }

        [Fact]
        public void Dijkstra_From_A_To_F_Path_Test()
        {
            DijkstraPathTest("A", "F", "AF");
        }

        [Fact]
        public void Dijkstra_From_A_To_G_Path_Test()
        {
            DijkstraPathTest("A", "G", "AFG");
        }

        [Fact]
        public void Dijkstra_From_A_To_H_Path_Test()
        {
            DijkstraPathTest("A", "H", "AFEH");
        }

        [Fact]
        public void Dijkstra_From_A_To_I_Path_Test()
        {
            DijkstraPathTest("A", "I", "AFEI");
        }

        [Fact]
        public void Dijkstra_From_A_To_J_Path_Test()
        {
            DijkstraPathTest("A", "J", "AFEHJ");
        }

        #endregion

        #region Dijkstra Distance Path

        [Fact]
        public void Dijkstra_From_A_To_B_Distance_Test()
        {
            DijkstraDistanceTest("A", "B", 3);
        }

        [Fact]
        public void Dijkstra_From_A_To_C_Distance_Test()
        {
            DijkstraDistanceTest("A", "C", 20);
        }

        [Fact]
        public void Dijkstra_From_A_To_D_Distance_Test()
        {
            DijkstraDistanceTest("A", "D", 14);
        }

        [Fact]
        public void Dijkstra_From_A_To_E_Distance_Test()
        {
            DijkstraDistanceTest("A", "E", 3);
        }

        [Fact]
        public void Dijkstra_From_A_To_F_Distance_Test()
        {
            DijkstraDistanceTest("A", "F", 2);
        }

        [Fact]
        public void Dijkstra_From_A_To_G_Distance_Test()
        {
            DijkstraDistanceTest("A", "G", 9);
        }

        [Fact]
        public void Dijkstra_From_A_To_H_Distance_Test()
        {
            DijkstraDistanceTest("A", "H", 8);
        }

        [Fact]
        public void Dijkstra_From_A_To_I_Distance_Test()
        {
            DijkstraDistanceTest("A", "I", 13);
        }

        [Fact]
        public void Dijkstra_From_A_To_J_Distance_Test()
        {
            DijkstraDistanceTest("A", "J", 21);
        }

        #endregion

        [Fact]
        public void TarjansAlgorithmTest()
        {
            // Assign
            var graph = CreateTestDirectlyAcyclicGraph();

            // Act
            var result = graph.Tarjan();
            var actual = result.Select(vectex => vectex.Label).Aggregate((current, label) => current + label);

            // Assert
            var expected = "321054876";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PrimsMinimumSpanningTreeTest()
        {
            // Assign
            var graph = CreateTestWeightedGraph();
            var expectedEdges = new[]
                {
                    graph["F", "E"], graph["F", "A"], graph["A", "B"], graph["D", "I"],
                    graph["E", "H"], graph["E", "G"], graph["D", "C"], graph["I", "J"],
                    graph["E", "I"]
                };
            var expected = new MinimumSpanTree(expectedEdges, 48);

            // Act
            var actual = graph.PrimsMinimumSpanningTree();

            // Assert
            Assert.True(MinimumSpanningTreeEquals(expected, actual));
        }

        [Fact]
        public void KruskalsMinimumSpanningTreeTest()
        {
            // Assign
            var graph = CreateTestWeightedGraph();
            var expectedEdges = new[]
                {
                    graph["F", "E"], graph["F", "A"], graph["A", "B"], graph["D", "I"],
                    graph["E", "H"], graph["E", "G"], graph["D", "C"], graph["I", "J"],
                    graph["E", "I"]
                };
            var expected = new MinimumSpanTree(expectedEdges, 48);

            // Act
            var actual = graph.KruskalsMinimumSpanningTree();

            // Assert
            Assert.True(MinimumSpanningTreeEquals(expected, actual));
        }

        private void DijkstraPathTest(string startLabel, string endLabel, string expected)
        {
            // Assign
            var graph = CreateTestWeightedGraph();

            // Act
            var result = graph.Dijkstra(graph[startLabel], graph[endLabel]);
            var actual = result.Select(vectex => vectex.Label).Aggregate((current, label) => current + label);

            // Assert
            Assert.Equal(expected, actual);
        }

        private void DijkstraDistanceTest(string startLabel, string endLabel, int expected)
        {
            // Assign
            var graph = CreateTestWeightedGraph();

            // Act
            var result = graph.Dijkstra(graph[startLabel], graph[endLabel]);
            var actual = result.Distance;

            // Assert
            Assert.Equal(expected, actual);
        }

        #region Tests setup and checks

        private Graph CreateTestTreeGraph()
        {
            var vertexA = new Vertex("a");
            var vertexB = new Vertex("b");
            var vertexC = new Vertex("c");
            var vertexD = new Vertex("d");
            var vertexE = new Vertex("e");
            var vertexF = new Vertex("f");
            var vertexG = new Vertex("g");

            var graph = new Graph();
            graph.SetVertex(vertexA);
            graph.SetVertex(vertexB);
            graph.SetVertex(vertexC);
            graph.SetVertex(vertexD);
            graph.SetVertex(vertexE);
            graph.SetVertex(vertexF);

            graph.SetEdge(new Edge(vertexA, vertexB, 0));
            graph.SetEdge(new Edge(vertexA, vertexC, 0));
            graph.SetEdge(new Edge(vertexB, vertexD, 0));
            graph.SetEdge(new Edge(vertexB, vertexE, 0));
            graph.SetEdge(new Edge(vertexC, vertexF, 0));
            graph.SetEdge(new Edge(vertexC, vertexG, 0));

            return graph;
        }

        private Graph CreateTestWeightedGraph()
        {
            var vertexA = new Vertex("A");
            var vertexB = new Vertex("B");
            var vertexC = new Vertex("C");
            var vertexD = new Vertex("D");
            var vertexE = new Vertex("E");
            var vertexF = new Vertex("F");
            var vertexG = new Vertex("G");
            var vertexH = new Vertex("H");
            var vertexI = new Vertex("I");
            var vertexJ = new Vertex("J");

            var graph = new Graph();
            graph.SetVertex(vertexA);
            graph.SetVertex(vertexB);
            graph.SetVertex(vertexC);
            graph.SetVertex(vertexD);
            graph.SetVertex(vertexE);
            graph.SetVertex(vertexF);
            graph.SetVertex(vertexG);
            graph.SetVertex(vertexH);
            graph.SetVertex(vertexI);
            graph.SetVertex(vertexJ);

            graph.SetEdge(new Edge(vertexA, vertexB, 3));
            graph.SetEdge(new Edge(vertexA, vertexF, 2));
            graph.SetEdge(new Edge(vertexB, vertexC, 17));
            graph.SetEdge(new Edge(vertexB, vertexD, 16));
            graph.SetEdge(new Edge(vertexC, vertexD, 8));
            graph.SetEdge(new Edge(vertexC, vertexI, 18));
            graph.SetEdge(new Edge(vertexD, vertexE, 11));
            graph.SetEdge(new Edge(vertexD, vertexI, 4));
            graph.SetEdge(new Edge(vertexE, vertexF, 1));
            graph.SetEdge(new Edge(vertexE, vertexG, 6));
            graph.SetEdge(new Edge(vertexE, vertexH, 5));
            graph.SetEdge(new Edge(vertexE, vertexI, 10));
            graph.SetEdge(new Edge(vertexF, vertexG, 7));
            graph.SetEdge(new Edge(vertexG, vertexH, 15));
            graph.SetEdge(new Edge(vertexH, vertexI, 12));
            graph.SetEdge(new Edge(vertexH, vertexJ, 13));
            graph.SetEdge(new Edge(vertexI, vertexJ, 9));

            graph.SetEdge(new Edge(vertexB, vertexA, 3));
            graph.SetEdge(new Edge(vertexF, vertexA, 2));
            graph.SetEdge(new Edge(vertexC, vertexB, 17));
            graph.SetEdge(new Edge(vertexD, vertexB, 16));
            graph.SetEdge(new Edge(vertexD, vertexC, 8));
            graph.SetEdge(new Edge(vertexI, vertexC, 18));
            graph.SetEdge(new Edge(vertexE, vertexD, 11));
            graph.SetEdge(new Edge(vertexI, vertexD, 4));
            graph.SetEdge(new Edge(vertexF, vertexE, 1));
            graph.SetEdge(new Edge(vertexG, vertexE, 6));
            graph.SetEdge(new Edge(vertexH, vertexE, 5));
            graph.SetEdge(new Edge(vertexI, vertexE, 10));
            graph.SetEdge(new Edge(vertexG, vertexF, 7));
            graph.SetEdge(new Edge(vertexH, vertexG, 15));
            graph.SetEdge(new Edge(vertexI, vertexH, 12));
            graph.SetEdge(new Edge(vertexJ, vertexH, 13));
            graph.SetEdge(new Edge(vertexJ, vertexI, 9));

            return graph;
        }

        private Graph CreateTestDirectlyAcyclicGraph()
        {
            var graph = new Graph();

            for (int i = 0; i < 9; i++)
            {
                graph.SetVertex(new Vertex(i.ToString()));
            }

            graph.SetEdge(new Edge(graph["0"], graph["1"], 0));
            graph.SetEdge(new Edge(graph["1"], graph["2"], 0));
            graph.SetEdge(new Edge(graph["1"], graph["3"], 0));
            graph.SetEdge(new Edge(graph["2"], graph["3"], 0));
            graph.SetEdge(new Edge(graph["4"], graph["2"], 0));
            graph.SetEdge(new Edge(graph["4"], graph["5"], 0));
            graph.SetEdge(new Edge(graph["6"], graph["7"], 0));
            graph.SetEdge(new Edge(graph["7"], graph["8"], 0));
            graph.SetEdge(new Edge(graph["6"], graph["4"], 0));

            return graph;
        }

        private bool MinimumSpanningTreeEquals(MinimumSpanTree x, MinimumSpanTree y)
        {
            if (x == null || y == null || x.Edges.Count != y.Edges.Count || x.Distance != y.Distance)
                return false;

            var thisEdgesKeys = new HashSet<Tuple<string, string>>(x.Edges.Select(key => new Tuple<string, string>(key.StartVertex.Label, key.EndVertex.Label)));

            foreach (var edge in y.Edges)
            {
                var normalKey = new Tuple<string, string>(edge.StartVertex.Label, edge.EndVertex.Label);
                var invertedKey = new Tuple<string, string>(edge.EndVertex.Label, edge.StartVertex.Label);
                if (!thisEdgesKeys.Contains(normalKey) && !thisEdgesKeys.Contains(invertedKey))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}