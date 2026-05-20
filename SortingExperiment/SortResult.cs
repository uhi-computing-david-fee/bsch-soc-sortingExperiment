using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingExperiment
{
    /// <summary>
    /// Holds the result of a single sorting experiment run.
    /// Records the algorithm name, input characteristics, timing, and operation count.
    /// </summary>
    public class SortResult
    {
        public string AlgorithmName { get; set; }
        public int InputSize { get; set; }
        public string OrderingType { get; set; }
        public int RunNumber { get; set; }
        public double ElapsedMilliseconds { get; set; }
        public long OperationCount { get; set; }

        public SortResult(string algorithmName, int inputSize, string orderingType, int runNumber, double elapsedMilliseconds, long operationCount)
        {
            AlgorithmName = algorithmName;
            InputSize = inputSize;
            OrderingType = orderingType;
            RunNumber = runNumber;
            ElapsedMilliseconds = elapsedMilliseconds;
            OperationCount = operationCount;
        }
    }
}