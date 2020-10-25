using System;
using System.Collections.Generic;
using System.Linq;

namespace Silent.Collections
{
    public class GridGraph<T> : IGridGraph<T> where T : IEquatable<T>
    {
        #region Static Fields

        private static (int column, int row) _left = (-1, 0);
        private static (int column, int row) _leftUp = (-1, -1);
        private static (int column, int row) _up = (0, -1);
        private static (int column, int row) _upRight = (1, -1);
        private static (int column, int row) _right = (1, 0);
        private static (int column, int row) _rightDown = (1, 1);
        private static (int column, int row) _down = (0, 1);
        private static (int column, int row) _downLeft = (-1, 1);

        private static readonly List<(int, int)> _neightborsDiagonalCoordinates = new List<(int, int)>
            {
                _left,
                _leftUp,
                _up,
                _upRight,
                _right,
                _rightDown,
                _down,
                _downLeft
            };
        private static readonly List<(int, int)> _neightborsDirectCoordinates = new List<(int, int)>
            {
                _left,
                _up,
                _right,
                _down
            };

        #endregion

        private readonly List<Vertex<T>> _vertices = new List<Vertex<T>>();
        private readonly List<Edge<T>> _edges = new List<Edge<T>>();
        private readonly Vertex<T>[,] _grid;
        private readonly int _width;
        private readonly int _height;

        private GridGraph(int width, int height)
        {
            _grid = new Vertex<T>[height, width];
            _width = width;
            _height = height;
        }

        public static GridGraph<T> FromSize(int width, int height)
        {
            return new GridGraph<T>(width, height);
        }

        public static GridGraph<T> Empty() => new GridGraph<T>(0, 0);

        public IReadOnlyCollection<Vertex<T>> Vertices => _vertices;

        public IReadOnlyCollection<Edge<T>> Edges => _edges;

        public Vertex<T> this[int row, int column] => InternalFindAndGet(new Position(row, column));

        public Vertex<T> SetVertex(Position position, T value)
        {
            // TODO: replace the wall checking with the removal of the edge between two vertices
            InternalRemoveVertex(position);
            return InternalSetVertex(position, value);
        }

        public Edge<T> SetEdge(Position sourcePosition, Position targetPosition, int weight)
        {
            var sourceVertex = InternalFindAndGet(sourcePosition);
            var targetVertex = InternalFindAndGet(targetPosition);
            return InternalSetEdge(sourceVertex, targetVertex, weight);
        }

        public bool RemoveVertex(Position position)
        {
            return InternalRemoveVertex(position);
        }

        public bool RemoveEdge(Position sourcePosition, Position targetPosition)
        {
            var sourceVertex = InternalFindAndGet(sourcePosition);
            var targetVertex = InternalFindAndGet(targetPosition);
            return InternalRemoveEdge(sourceVertex, targetVertex);
        }

        private IEnumerable<Vertex<T>> DetectNeighbors(int row, int column)
        {
            const bool includeDiagonals = true;
            List<(int column, int row)> currentShifts =
                includeDiagonals
                ? _neightborsDiagonalCoordinates
                : _neightborsDirectCoordinates;

            return currentShifts
                .Select(shift =>
                {
                    int offsetRow = row + shift.row;
                    int offsetColumn = column + shift.column;
                    return (offsetRow, offsetColumn);
                })
                .Where(shift =>
                {
                    return shift.offsetColumn >= 0
                        && shift.offsetRow >= 0
                        && shift.offsetColumn < _width
                        && shift.offsetRow < _height;
                })
                .Select(shift => _grid[shift.offsetRow, shift.offsetColumn]);
        }

        private Vertex<T> InternalFindAndGet(Position position)
        {
            // TODO: check if is in bounds
            return position == null ? null : _grid[position.Row, position.Column];
        }

        private Edge<T> InternalFindAndGet(Vertex<T> sourceVertex, Vertex<T> targetVertex)
        {
            return _edges.FirstOrDefault(x => x.StartVertex == sourceVertex && x.EndVertex == targetVertex);
        }

        private bool InternalRemoveVertex(Position position)
        {
            var sourceVertex = InternalFindAndGet(position);

            if (sourceVertex != null && position != null)
            {
                bool successfull = true;

                foreach (var targetVertex in sourceVertex.Neighbors)
                {
                    // NOTE: remove the edge from all neighbors
                    var edge1 = InternalFindAndGet(sourceVertex, targetVertex);
                    var edge2 = InternalFindAndGet(targetVertex, sourceVertex);

                    successfull &= InternalRemoveEdge(sourceVertex, targetVertex);
                    successfull &= InternalRemoveEdge(targetVertex, sourceVertex);
                    successfull &= _edges.Remove(edge1);
                    successfull &= _edges.Remove(edge2);
                }

                _vertices.Remove(sourceVertex);
                _grid[position.Row, position.Column] = null;
                return successfull;
            }

            return false;
        }

        private Vertex<T> InternalSetVertex(Position position, T value)
        {
            if (value != null && position != null)
            {
                var vertex = new Vertex<T>(value);
                var neighbours = DetectNeighbors(position.Row, position.Column).Where(neighbor => neighbor != null);

                foreach (var neighbor in neighbours)
                {
                    // NOTE: sets outbound edges for current vertex and all neighbors
                    // NOTE: sets inbound edges between for all neighbors to current vertex
                    _edges.Add(InternalSetEdge(vertex, neighbor, 1));
                    _edges.Add(InternalSetEdge(neighbor, vertex, 1));
                }

                _vertices.Add(vertex);
                _grid[position.Row, position.Column] = vertex;
                return vertex;
            }

            return null;
        }

        private Edge<T> InternalSetEdge(Vertex<T> sourceVertex, Vertex<T> targetVertex, int weight)
        {
            var edge = InternalFindAndGet(sourceVertex, targetVertex);
            if (edge == null && sourceVertex != null && targetVertex != null)
            {
                edge = new Edge<T>(sourceVertex, targetVertex, weight);
                sourceVertex.SetOutboundEdge(edge);
                targetVertex.SetInboundEdge(edge);
            }

            return edge;
        }

        private bool InternalRemoveEdge(Vertex<T> sourceVertex, Vertex<T> targetVertex)
        {
            return sourceVertex.RemoveOutboundEdge(targetVertex)
                && sourceVertex.RemoveInboundEdge(targetVertex);
        }
    }
}
