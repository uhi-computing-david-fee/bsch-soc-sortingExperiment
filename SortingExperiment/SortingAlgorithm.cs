namespace SortingExperiment
{
    /// <summary>
    /// Contains the merge sort implementation used as the supplied baseline algorithm.
    /// 
    /// An operation is counted each time two elements are compared during sorting.
    /// This definition is applied consistently across all algorithms in this experiment;
    /// your implementations of bubble sort and insertion sort should count comparisons
    /// in the same way.
    /// 
    /// To add a new algorithm, follow the same pattern as MergeSort below:
    /// accept the array and a ref operationCount parameter, sort in place, and
    /// increment operationCount each time a comparison between two elements is made.
    /// </summary>
    public static class SortingAlgorithm
    {
        public static void MergeSort(int[] array, ref long operationCount)
        {
            if (array.Length <= 1)
                return;

            MergeSortRecursive(array, 0, array.Length - 1, ref operationCount);
        }

        private static void MergeSortRecursive(int[] array, int left, int right, ref long operationCount)
        {
            if (left >= right)
                return;

            int mid = left + (right - left) / 2;

            MergeSortRecursive(array, left, mid, ref operationCount);
            MergeSortRecursive(array, mid + 1, right, ref operationCount);
            Merge(array, left, mid, right, ref operationCount);
        }

        private static void Merge(int[] array, int left, int mid, int right, ref long operationCount)
        {
            int leftSize = mid - left + 1;
            int rightSize = right - mid;

            int[] leftArray = new int[leftSize];
            int[] rightArray = new int[rightSize];

            for (int i = 0; i < leftSize; i++)
                leftArray[i] = array[left + i];

            for (int j = 0; j < rightSize; j++)
                rightArray[j] = array[mid + 1 + j];

            int leftIndex = 0;
            int rightIndex = 0;
            int mergeIndex = left;

            while (leftIndex < leftSize && rightIndex < rightSize)
            {
                // Each iteration of this loop performs one comparison between elements.
                operationCount++;

                if (leftArray[leftIndex] <= rightArray[rightIndex])
                {
                    array[mergeIndex] = leftArray[leftIndex];
                    leftIndex++;
                }
                else
                {
                    array[mergeIndex] = rightArray[rightIndex];
                    rightIndex++;
                }
                mergeIndex++;
            }

            while (leftIndex < leftSize)
            {
                array[mergeIndex] = leftArray[leftIndex];
                leftIndex++;
                mergeIndex++;
            }

            while (rightIndex < rightSize)
            {
                array[mergeIndex] = rightArray[rightIndex];
                rightIndex++;
                mergeIndex++;
            }
        }
    }
}