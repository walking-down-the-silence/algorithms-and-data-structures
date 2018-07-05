using System;
using System.Collections.Generic;

namespace Common
{
    public class DisjointSet<TValue>
    {
        private readonly Dictionary<Guid, DisjointSetGroup<TValue>> _groups;
        private readonly Dictionary<TValue, DisjointSetGroup<TValue>> _items;

        public DisjointSet(IEnumerable<TValue> values)
        {
            _groups = new Dictionary<Guid, DisjointSetGroup<TValue>>();
            _items = new Dictionary<TValue, DisjointSetGroup<TValue>>();

            foreach (var value in values)
            {
                var group = new DisjointSetGroup<TValue>(value);
                _items.Add(value, group);
                _groups.Add(group.Id, group);
            }
        }

        public void Union(TValue leftValue, TValue rightValue)
        {
            var leftGroupId = Find(leftValue);
            var rightGroupId = Find(rightValue);

            if (leftGroupId == rightGroupId)
                return;

            var left = _groups[leftGroupId];
            var right = _groups[rightGroupId];

            if (left.Count < right.Count)
            {
                foreach (var value in left.Items)
                {
                    _items[value] = right;
                    right.Items.Add(value);
                }

                _groups.Remove(leftGroupId);
            }
            else
            {
                foreach (var value in right.Items)
                {
                    _items[value] = left;
                    left.Items.Add(value);
                }

                _groups.Remove(rightGroupId);
            }
        }

        public Guid Find(TValue value)
        {
            return _items[value].Id;
        }

        private class DisjointSetGroup<T>
        {
            public DisjointSetGroup(T value)
            {
                Id = Guid.NewGuid();
                Items = new List<T> { value };
            }

            public Guid Id { get; }

            public int Count => Items.Count;

            public List<T> Items { get; private set; }
        }
    }
}
