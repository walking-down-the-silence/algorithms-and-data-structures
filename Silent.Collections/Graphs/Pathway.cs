using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Silent.Collections
{
    public class Pathway<T> : IBidirectionalCollection<Vertex<T>> where T : IEquatable<T>
    {
        private readonly List<Vertex<T>> _vertices;
        private int _currentIndex;

        public Pathway(IEnumerable<Vertex<T>> vertices, long distance, AlgorithmState state)
        {
            _vertices = vertices.ToList();
            Distance = distance;
            State = state;
        }

        public long Distance { get; set; }

        public AlgorithmState State { get; }

        public Vertex<T> Current => GetCurrent();

        public int Count => _vertices.Count;

        public bool IsReadOnly => false;

        public void Add(Vertex<T> item) => _vertices.Add(item);

        public bool Remove(Vertex<T> item) => _vertices.Remove(item);

        public void Clear() => _vertices.Clear();

        public bool Contains(Vertex<T> item) => _vertices.Contains(item);

        public void CopyTo(Vertex<T>[] array, int arrayIndex) => _vertices.CopyTo(array, arrayIndex);

        public IEnumerator<Vertex<T>> GetEnumerator() => _vertices.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Vertex<T> Next()
        {
            _currentIndex = _currentIndex + 1;
            return GetCurrent();
        }

        public Vertex<T> Previous()
        {
            _currentIndex = _currentIndex - 1;
            return GetCurrent();
        }

        private Vertex<T> GetCurrent() => _currentIndex < _vertices.Count
            ? _vertices[_currentIndex] : default;
    }
}
