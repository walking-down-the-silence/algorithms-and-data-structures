using System;

namespace Silent.Collections
{
    public static class Arrays
    {
        public static void Swap<T>(T[] array, int i, int j)
        {
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }

        public static void Shuffle<T>(T[] array)
        {
            var random = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                int randomIndex = random.Next(array.Length);
                Swap(array, i, randomIndex);
            }
        }

        public static void SelectionSort<T>(T[] array) where T : IComparable<T>
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                int minIdex = i;

                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[j].CompareTo(array[minIdex]) == -1)
                    {
                        minIdex = j;
                    }
                }

                Swap(array, i, minIdex);
            }
        }

        public static void InsertionSort<T>(T[] array) where T : IComparable<T>
        {
            for (int i = 1; i < array.Length; i++)
            {
                for (int j = i; j > 0; j--)
                {
                    if (array[j].CompareTo(array[j - 1]) == -1)
                    {
                        Swap(array, j, j - 1);
                    }
                }
            }
        }

        public static void BubbleSort<T>(T[] array) where T : IComparable<T>
        {
            bool somethingSwapped = true;

            while (somethingSwapped)
            {
                somethingSwapped = false;

                for (int i = 0; i < array.Length - 1; i++)
                {
                    if (array[i + 1].CompareTo(array[i]) == -1)
                    {
                        Swap(array, i, i + 1);
                        somethingSwapped = true;
                    }
                }
            }
        }

        public static void MergeSort<T>(T[] array) where T : IComparable<T>
        {
            T[] mergeArray = new T[array.Length];
            InternalMergeSort(array, mergeArray, 0, array.Length - 1);
        }

        private static void InternalMergeSort<T>(T[] array, T[] mergeArray, int begin, int end) where T : IComparable<T>
        {
            if (begin < end)
            {
                int middle = (begin + end) / 2;
                InternalMergeSort(array, mergeArray, begin, middle);
                InternalMergeSort(array, mergeArray, middle + 1, end);
                Merge(array, mergeArray, begin, middle + 1, end);
            }
        }

        private static void Merge<T>(T[] array, T[] mergeArray, int begin, int middle, int end) where T : IComparable<T>
        {
            int firstBegin = begin;
            int firstEnd = middle - 1;
            int secondBegin = middle;
            int mergeIndex = begin;

            while (firstBegin <= firstEnd && secondBegin <= end)
            {
                if (array[firstBegin].CompareTo(array[secondBegin]) == -1)
                {
                    mergeArray[mergeIndex] = array[firstBegin];
                    firstBegin++;
                }
                else
                {
                    mergeArray[mergeIndex] = array[secondBegin];
                    secondBegin++;
                }

                mergeIndex++;
            }

            while (firstBegin <= firstEnd)
            {
                mergeArray[mergeIndex] = array[firstBegin];
                mergeIndex++;
                firstBegin++;
            }

            while (secondBegin <= end)
            {
                mergeArray[mergeIndex] = array[secondBegin];
                mergeIndex++;
                secondBegin++;
            }

            for (int i = begin; i <= end; i++)
            {
                array[i] = mergeArray[i];
            }
        }

        public static void QuickSort<T>(T[] array) where T : IComparable<T>
        {
            Shuffle(array);
            InternalQuickSort(array, 0, array.Length - 1);
        }

        private static void InternalQuickSort<T>(T[] array, int begin, int end) where T : IComparable<T>
        {
            if (begin >= end)
                return;

            int pivotIndex = Partition(array, begin, end);

            if (begin < pivotIndex - 1)
                InternalQuickSort(array, begin, pivotIndex - 1);

            if (pivotIndex < end)
                InternalQuickSort(array, pivotIndex + 1, end);
        }

        private static int Partition<T>(T[] array, int begin, int end) where T : IComparable<T>
        {
            T pivot = array[begin];
            int leftIndex = begin + 1;
            int rightIndex = end;

            while (true)
            {
                while (array[leftIndex].CompareTo(pivot) < 0 && leftIndex < end)
                    leftIndex++;

                while (array[rightIndex].CompareTo(pivot) >= 0 && rightIndex > begin)
                    rightIndex--;

                if (leftIndex >= rightIndex)
                    break;

                Swap(array, leftIndex, rightIndex);
            }

            Swap(array, rightIndex, begin);
            return rightIndex;
        }
    }
}