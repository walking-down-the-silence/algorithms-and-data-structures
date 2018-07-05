using System.Linq;
using Common.Algorithms;
using Xunit;

namespace CommonTests
{
    public class ArraysTests
    {
        private const string ArraysMismatchMessage = "Arrays mismatch!";

        private int[] GetShuffledArray()
        {
            return new[]
            {
                389, 100, 331, 340, 325, 331, 462,
                307, 457, 86, 398, 232, 97, 449, 14,
                236, 491, 126, 339, 187, 64, 142, 498,
                433, 437, 343, 401, 199, 138, 167,
                21, 306, 75, 105, 392, 430, 73, 271,
                479, 201, 252, 2, 101, 219, 472, 146,
                21, 40, 444, 436, 202, 479, 104, 227,
                130, 246, 102, 397, 403, 481, 429,
                263, 168, 290, 65, 91, 157, 491, 57,
                84, 162, 220, 147, 138, 435, 62, 41,
                278, 461, 290, 321, 361, 255, 202,
                464, 41, 201, 347, 475, 145, 327, 175,
                367, 497, 271, 489, 333, 311, 153, 380
            };
        }

        private int[] GetSortedArray()
        {
            return new[]
            {
                2, 14, 21, 21, 40, 41, 41, 57, 62, 64,
                65, 73, 75, 84, 86, 91, 97, 100, 101,
                102, 104, 105, 126, 130, 138, 138, 142,
                145, 146, 147, 153, 157, 162, 167, 168,
                175, 187, 199, 201, 201, 202, 202, 219,
                220, 227, 232, 236, 246, 252, 255, 263,
                271, 271, 278, 290, 290, 306, 307, 311,
                321, 325, 327, 331, 331, 333, 339, 340,
                343, 347, 361, 367, 380, 389, 392, 397,
                398, 401, 403, 429, 430, 433, 435, 436,
                437, 444, 449, 457, 461, 462, 464, 472,
                475, 479, 479, 481, 489, 491, 491, 497, 498
            };
        }

        [Fact]
        public void SelectionSort_WithInputShuffledArray_ShouldBeSorted()
        {
            // Assign
            var shuffled = GetShuffledArray();
            var sorted = GetSortedArray();

            // Act
            Arrays.SelectionSort(shuffled);

            // Assert
            Assert.True(shuffled.SequenceEqual(sorted), ArraysMismatchMessage);
        }

        [Fact]
        public void InsertionSort_WithInputShuffledArray_ShouldBeSorted()
        {
            // Assign
            var shuffled = GetShuffledArray();
            var sorted = GetSortedArray();

            // Act
            Arrays.InsertionSort(shuffled);

            // Assert
            Assert.True(shuffled.SequenceEqual(sorted), ArraysMismatchMessage);
        }

        [Fact]
        public void BubbleSort_WithInputShuffledArray_ShouldBeSorted()
        {
            // Assign
            var shuffled = GetShuffledArray();
            var sorted = GetSortedArray();

            // Act
            Arrays.BubbleSort(shuffled);

            // Assert
            Assert.True(shuffled.SequenceEqual(sorted), ArraysMismatchMessage);
        }

        [Fact]
        public void MergeSort_WithInputShuffledArray_ShouldBeSorted()
        {
            // Assign
            var shuffled = GetShuffledArray();
            var sorted = GetSortedArray();

            // Act
            Arrays.MergeSort(shuffled);

            // Assert
            Assert.True(shuffled.SequenceEqual(sorted), ArraysMismatchMessage);
        }

        [Fact]
        public void QuickSort_WithInputShuffledArray_ShouldBeSorted()
        {
            // Assign
            var shuffled = GetShuffledArray();
            var sorted = GetSortedArray();

            // Act
            Arrays.QuickSort(shuffled);

            // Assert
            Assert.True(shuffled.SequenceEqual(sorted), ArraysMismatchMessage);
        }
    }
}