using System.Collections.Generic;

namespace Silent.Collections
{
    /// <summary>
    /// Extension methods to make the System.Collections.Generic.SortedList easier to use.
    /// </summary>
    public class PriorityQueue<TKey, TValue>
    {
        private readonly SortedList<TKey, TValue> _prioritizedValues;

        public PriorityQueue(IComparer<TKey> priorityComparer)
        {
            _prioritizedValues = new SortedList<TKey, TValue>(priorityComparer);
        }

        /// <summary>
        /// Checks if the SortedList is empty.
        /// </summary>
        /// <returns>True if sortedList is empty, false if it still has elements.</returns>
        public bool IsEmpty()
        {
            return _prioritizedValues.Count == 0;
        }

        /// <summary>
        /// Gets if the queue has an item with specified priority.
        /// </summary>
        /// <param name="priority"> Priority to look for. </param>
        public bool ContainsKey(TKey priority)
        {
            return _prioritizedValues.ContainsKey(priority);
        }

        /// <summary>
        /// Gets if the queue has an item with specified value.
        /// </summary>
        /// <param name="value"> Value to look for. </param>
        public bool ContainsValue(TValue value)
        {
            return _prioritizedValues.ContainsValue(value);
        }

        /// <summary>
        /// Adds a GridNode to the SortedList.
        /// </summary>
        /// <param name="value">Node to add to the sortedList.</param>
        public void Enqueue(TKey priority, TValue value)
        {
            _prioritizedValues.Add(priority, value);
        }

        /// <summary>
        /// Removes the node from the sorted list with the smallest TotalCost and returns that node.
        /// </summary>
        /// <returns>Node with the smallest TotalCost.</returns>
        public TValue Dequeue()
        {
            var value = _prioritizedValues.Values[0];
            _prioritizedValues.RemoveAt(0);
            return value;
        }
    }
}
