using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SortingExperiment
{
    /// <summary>
    /// Runs a sorting algorithm a specified number of times on a given input file,
    /// timing each run and recording operation counts.
    /// 
    /// The input file is read once and the array is copied fresh before each run,
    /// ensuring that each run sorts an identical input and results are comparable.
    /// </summary>
    public class ExperimentRunner
    {
        private readonly string _inputFilePath;
        private readonly int[] _originalData;
        private readonly string _orderingType;

        public ExperimentRunner(string inputFilePath, string orderingType)
        {
            _inputFilePath = inputFilePath;
            _orderingType = orderingType;
            _originalData = LoadData(inputFilePath);
        }

        /// <summary>
        /// Runs the specified algorithm a given number of times and returns one SortResult per run.
        /// </summary>
        /// <param name="algorithmName">Display name for the algorithm, used in CSV output.</param>
        /// <param name="sortAction">A delegate matching the signature: (int[] array, ref long operationCount)</param>
        /// <param name="numberOfRuns">How many times to run the algorithm on this input.</param>
        public SortResult[] Run(string algorithmName, Action<int[], RefWrapper<long>> sortAction, int numberOfRuns)
        {
            SortResult[] results = new SortResult[numberOfRuns];

            for (int run = 1; run <= numberOfRuns; run++)
            {
                // Take a fresh copy of the original data for each run.
                int[] workingCopy = (int[])_originalData.Clone();
                long operationCount = 0;
                var opWrapper = new RefWrapper<long>(operationCount);

                Stopwatch stopwatch = Stopwatch.StartNew();
                sortAction(workingCopy, opWrapper);
                stopwatch.Stop();

                results[run - 1] = new SortResult(
                    algorithmName,
                    _originalData.Length,
                    _orderingType,
                    run,
                    stopwatch.Elapsed.TotalMilliseconds,
                    opWrapper.Value
                );

                Console.WriteLine($"  Run {run}/{numberOfRuns} complete. Time: {stopwatch.Elapsed.TotalMilliseconds:F2}ms, Operations: {opWrapper.Value:N0}");
            }

            return results;
        }

        private int[] LoadData(string filePath)
        {
            return File.ReadAllLines(filePath)
                       .Where(line => !string.IsNullOrWhiteSpace(line))
                       .Select(int.Parse)
                       .ToArray();
        }
    }

    /// <summary>
    /// A simple wrapper to allow passing a value type by reference through a delegate.
    /// This is a technical necessity in C# and is not something you need to modify.
    /// </summary>
    public class RefWrapper<T>
    {
        public T Value { get; set; }
        public RefWrapper(T value) { Value = value; }
    }
}