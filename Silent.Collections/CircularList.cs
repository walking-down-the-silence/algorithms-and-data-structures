using System.Collections;
using System.Collections.Generic;

namespace Silent.Collections
{
    public class CircularList<T> : ICircularCollection<T>
    {
        private readonly List<T> _innerList;
        private int _currentIndex;

        public CircularList()
        {
            _innerList = new List<T>();
        }

        public CircularList(IEnumerable<T> list)
        {
            _innerList = new List<T>(list);
        }

        public T Current => _innerList[_currentIndex];

        public int Count => _innerList.Count;

        public bool IsReadOnly => false;

        public T Next()
        {
            _currentIndex = (_currentIndex + 1) % _innerList.Count;
            return _innerList[_currentIndex];
        }

        public T Previous()
        {
            _currentIndex = _currentIndex == 0 ? _innerList.Count - 1 : _currentIndex - 1;
            return _innerList[_currentIndex];
        }

        public T PeekNext()
        {
            int peekNextIndex = (_currentIndex + 1) % _innerList.Count;
            return _innerList[peekNextIndex];
        }

        public void Add(T item) => _innerList.Add(item);

        public void Clear() => _innerList.Clear();

        public bool Contains(T item) => _innerList.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _innerList.CopyTo(array, arrayIndex);

        public bool Remove(T item) => _innerList.Remove(item);

        public IEnumerator<T> GetEnumerator() => _innerList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
