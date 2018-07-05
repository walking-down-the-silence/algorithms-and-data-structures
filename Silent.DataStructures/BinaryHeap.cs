using System;
using System.Collections;
using System.Collections.Generic;

namespace Silent.Collections
{
    public class BinaryHeap<TKey> : IEnumerable<TKey>
    {
        private readonly BinaryHeapType _heapType;
        private readonly IComparer<TKey> _comparer;
        private readonly List<TKey> _items;

        public BinaryHeap(BinaryHeapType heapType, int initialCapacity, IComparer<TKey> comparer)
        {
            _heapType = heapType;
            _items = new List<TKey>(initialCapacity);
            _comparer = comparer ?? Comparer<TKey>.Default;
        }

        public BinaryHeap(BinaryHeapType heapType, int initialCapacity) : this(heapType, initialCapacity, null)
        {
        }

        public int Count => _items.Count;

        public void Add(TKey item)
        {
            // add item to the end
            _items.Add(item);

            // then propagate it up till needed
            BubbleUp(Count - 1);
        }

        public TKey Peek()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Cannot peek when the heap is empty.");
            }

            return _items[0];
        }

        public TKey Remove()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Cannot remove when the heap is empty.");
            }

            TKey minimum = _items[0];
            Swap(0, Count - 1);
            _items.RemoveAt(_items.Count - 1);
            BubbleDown(0);

            return minimum;
        }

        public IEnumerator<TKey> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void BubbleUp(int childIndex)
        {
            // swap with parent till child is better
            while (SwapWithParentIfBetter(childIndex))
            {
                // update child index if it was swapped
                childIndex = GetParentIndex(childIndex);
            }
        }

        private void BubbleDown(int parentIndex)
        {
            // check if we can propagate further down
            while (HasChildren(parentIndex))
            {
                // get best from child to swap with
                int bestChildIndex = GetBestChildIndex(parentIndex);
                if (SwapWithParentIfBetter(bestChildIndex))
                {
                    parentIndex = bestChildIndex;
                }
                else
                {
                    break;
                }
            }
        }

        private bool SwapWithParentIfBetter(int childIndex)
        {
            if (childIndex > 0)
            {
                // get parent index to compare it
                int parentIndex = GetParentIndex(childIndex);

                // if child index is better then swap it
                if (GetBestIndex(parentIndex, childIndex) == childIndex)
                {
                    Swap(parentIndex, childIndex);
                    return true;
                }
            }

            return false;
        }

        private int GetBestChildIndex(int parentIndex)
        {
            int leftChildIndex = GetLeftChildIndex(parentIndex);
            int rightChildIndex = GetRightChildIndex(parentIndex);

            if (ItemExists(rightChildIndex))
            {
                return GetBestIndex(leftChildIndex, rightChildIndex);
            }

            return leftChildIndex;
        }

        private int GetBestIndex(int index1, int index2)
        {
            switch (_heapType)
            {
                case BinaryHeapType.MinimumHeap:
                    return _comparer.Compare(_items[index1], _items[index2]) < 0 ? index1 : index2;
                case BinaryHeapType.MaximumHeap:
                    return _comparer.Compare(_items[index1], _items[index2]) > 0 ? index1 : index2;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool HasChildren(int index)
        {
            int leftChildIndex = GetLeftChildIndex(index);
            return ItemExists(leftChildIndex);
        }

        private bool ItemExists(int index) => index < Count;

        private void Swap(int index1, int index2)
        {
            TKey temp = _items[index1];
            _items[index1] = _items[index2];
            _items[index2] = temp;
        }

        private int GetParentIndex(int childIndex) => (childIndex - 1) / 2;

        private int GetLeftChildIndex(int parentIndex) => 2 * parentIndex + 1;

        private int GetRightChildIndex(int parentIndex) => 2 * parentIndex + 2;
    }

    public enum BinaryHeapType
    {
        MinimumHeap,

        MaximumHeap
    }
}