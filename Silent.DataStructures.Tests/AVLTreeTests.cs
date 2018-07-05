using System.Collections.Generic;
using Common;
using Xunit;

namespace CommonTests
{
    public class AvlTreeTests
    {
        [Fact]
        public void Insert_Integers_ShouldBeAdded()
        {
            // Arrange
            IComparer<int> comparerFake = CreateIntegerComparerFake();
            AvlTree<int> tree = new AvlTree<int>(comparerFake);

            // Act
            tree.Insert(5);
            tree.Insert(7);
            tree.Insert(3);
            tree.Insert(2);
            tree.Insert(8);
            tree.Insert(3);

            // Assert
            int expectedCount = 6;
            int expectedHeight = 3;
            Assert.Equal(expectedCount, tree.Count);
            Assert.Equal(expectedHeight, tree.Height);
        }

        [Fact]
        public void Insert_SortedIntegers_ShouldBeBallanced()
        {
            // Arrange
            IComparer<int> comparerFake = CreateIntegerComparerFake();
            AvlTree<int> tree = new AvlTree<int>(comparerFake);

            // Act
            tree.Insert(1);
            tree.Insert(2);
            tree.Insert(3);
            tree.Insert(4);
            tree.Insert(5);
            tree.Insert(6);

            // Assert
            int expectedCount = 6;
            int expectedHeight = 3;
            Assert.Equal(expectedCount, tree.Count);
            Assert.Equal(expectedHeight, tree.Height);
        }

        private IComparer<int> CreateIntegerComparerFake()
        {
            return Comparer<int>.Create((left, right) => left < right ? -1 : (left > right ? 1 : 0));
        }
    }
}
