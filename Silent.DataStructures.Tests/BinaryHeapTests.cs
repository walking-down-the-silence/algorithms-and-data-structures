using System.Collections.Generic;
using Xunit;

namespace Silent.Collections.Tests
{
    public class BinaryHeapTests
    {
        [Fact]
        public void Peek_ReturnsMinimumElement_Test()
        {
            // Assign
            var binaryMinimumHeap = CreateBinaryHeap(BinaryHeapType.MinimumHeap);
            var expected = 1;

            // Act
            var actual = binaryMinimumHeap.Peek();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Peek_ReturnsMaximumElement_Test()
        {
            // Assign
            var binaryMinimumHeap = CreateBinaryHeap(BinaryHeapType.MaximumHeap);
            var expected = 10;

            // Act
            var actual = binaryMinimumHeap.Peek();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Remove_ReturnsAndRemovesMinimumElement_Test()
        {
            // Assign
            var binaryMinimumHeap = CreateBinaryHeap(BinaryHeapType.MinimumHeap);
            var expected = 1;
            var expectedCount = binaryMinimumHeap.Count - 1;

            // Act
            var actual = binaryMinimumHeap.Remove();
            var actualCount = binaryMinimumHeap.Count;

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void Remove_ReturnsAndRemovesMaximumElement_Test()
        {
            // Assign
            var binaryMinimumHeap = CreateBinaryHeap(BinaryHeapType.MaximumHeap);
            var expected = 10;
            var expectedCount = binaryMinimumHeap.Count - 1;

            // Act
            var actual = binaryMinimumHeap.Remove();
            var actualCount = binaryMinimumHeap.Count;

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void HeapSort_ReturnsSortedAscending_Test()
        {
            // Assign
            var binaryMinimumHeap = CreateBinaryHeap(BinaryHeapType.MinimumHeap);
            var expected = true;

            // Act
            var result = ExtractToList(binaryMinimumHeap);
            var actual = IsSorted(result, true);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HeapSort_ReturnsSortedDescending_Test()
        {
            // Assign
            var binaryMinimumHeap = CreateBinaryHeap(BinaryHeapType.MaximumHeap);
            var expected = true;

            // Act
            var result = ExtractToList(binaryMinimumHeap);
            var actual = IsSorted(result, false);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Peek_WithCustomComparerConstructorParameter_ReturnsMinimumElement_Test()
        {
            // Assign
            var binaryHeap = CreateCustomTypeBinaryHeap(BinaryHeapType.MinimumHeap);
            var expected = 1;

            // Act
            var actual = binaryHeap.Peek().Value;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Peek_WithCustomComparerConstructorParameter_ReturnsMaximumElement_Test()
        {
            // Assign
            var binaryHeap = CreateCustomTypeBinaryHeap(BinaryHeapType.MaximumHeap);
            var expected = 10;

            // Act
            var actual = binaryHeap.Peek().Value;

            // Assert
            Assert.Equal(expected, actual);
        }

        private BinaryHeap<int> CreateBinaryHeap(BinaryHeapType heapType)
        {
            var array = CreateIntegerArray();
            var binaryHeap = new BinaryHeap<int>(heapType, array.Length);

            foreach (var number in array)
            {
                binaryHeap.Add(number);
            }

            return binaryHeap;
        }

        private BinaryHeap<ValueWrapper> CreateCustomTypeBinaryHeap(BinaryHeapType heapType)
        {
            var array = CreateIntegerArray();
            var binaryHeap = new BinaryHeap<ValueWrapper>(heapType, array.Length, new ValueWrapperComparer());

            foreach (var number in array)
            {
                binaryHeap.Add(new ValueWrapper(number));
            }

            return binaryHeap;
        }

        private List<int> ExtractToList(BinaryHeap<int> binaryHeap)
        {
            var result = new List<int>(binaryHeap.Count);
            var count = binaryHeap.Count;

            for (int i = 0; i < count; i++)
            {
                result.Add(binaryHeap.Remove());
            }

            return result;
        }

        private bool IsSorted(List<int> integers, bool ascending)
        {
            for (int i = 1; i < integers.Count; i++)
            {
                int leftValue = ascending ? integers[i - 1] : integers[i];
                int rightValue = ascending ? integers[i] : integers[i - 1];

                if (leftValue > rightValue)
                    return false;
            }

            return true;
        }

        private int[] CreateIntegerArray()
        {
            return new[] { 1, 9, 6, 8, 2, 3, 10, 5, 4, 7 };
        }

        private class ValueWrapper
        {
            public ValueWrapper(int value)
            {
                Value = value;
            }

            public int Value { get; }
        }

        private class ValueWrapperComparer : IComparer<ValueWrapper>
        {
            public int Compare(ValueWrapper x, ValueWrapper y)
            {
                if (x.Value < y.Value)
                {
                    return -1;
                }
                if (x.Value > y.Value)
                {
                    return 1;
                }
                return 0;
            }
        }
    }
}