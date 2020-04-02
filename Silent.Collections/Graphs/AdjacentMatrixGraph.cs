using System;
using System.Collections.Generic;
using System.Linq;

namespace Silent.Collections
{
    public class GridGraph<T> : IAdjacencyMatrix<T> where T : IEquatable<T>, IPosition
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

        private readonly Vertex<T>[,] _grid;
        private readonly int _width;
        private readonly int _height;

        public Edge<T> this[T startLabel, T endLabel] => throw new NotImplementedException();

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

        /// <summary>
        /// TODO: replace the wall checking with the removal of the edge between two vertices
        /// </summary>
        /// <param name="value"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public Vertex<T> Set(T value, int row, int column)
        {
            var vertex = new Vertex<T>(value);
            var outboundEdges = GetNeighbors(vertex)
                .Where(neighbor => neighbor != null)
                .Select(neighbor => new Edge<T>(vertex, neighbor, 1));
            vertex.OutboundEdges.AddRange(outboundEdges);
            _grid[row, column] = vertex;
            return vertex;
        }

        public Vertex<T> Get(int row, int column)
        {
            return _grid[row, column];
        }

        private IEnumerable<Vertex<T>> GetNeighbors(Vertex<T> node)
        {
            const bool includeDiagonals = true;
            List<(int column, int row)> currentShifts =
                includeDiagonals
                ? _neightborsDiagonalCoordinates
                : _neightborsDirectCoordinates;

            return currentShifts
                .Select(shift =>
                {
                    int row = node.Value.Y + shift.row;
                    int column = node.Value.X + shift.column;
                    return (row, column);
                })
                .Where(shift =>
                {
                    return shift.column >= 0
                        && shift.row >= 0
                        && shift.column < _width
                        && shift.row < _height;
                })
                .Select(shift => _grid[shift.row, shift.column]);
        }
    }
}
