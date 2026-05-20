using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SortingExperiment
{
    /// <summary>
    /// Writes experiment results to a CSV file.
    /// Each row represents one run of one algorithm on one input file.
    /// Results are appended if the file already exists, allowing multiple
    /// experiment sessions to accumulate into a single output file.
    /// </summary>
    public class ResultReporter
    {
        private readonly string _outputFilePath;

        public ResultReporter(string outputFilePath)
        {
            _outputFilePath = outputFilePath;
            EnsureHeaderExists();
        }

        public void Write(IEnumerable<SortResult> results)
        {
            StringBuilder sb = new StringBuilder();

            foreach (SortResult result in results)
            {
                sb.AppendLine(
                    $"{result.AlgorithmName}," +
                    $"{result.InputSize}," +
                    $"{result.OrderingType}," +
                    $"{result.RunNumber}," +
                    $"{result.ElapsedMilliseconds:F4}," +
                    $"{result.OperationCount}"
                );
            }

            File.AppendAllText(_outputFilePath, sb.ToString());
        }

        private void EnsureHeaderExists()
        {
            if (!File.Exists(_outputFilePath))
            {
                File.WriteAllText(_outputFilePath,
                    "Algorithm,InputSize,OrderingType,RunNumber,ElapsedMilliseconds,OperationCount\n");
            }
        }
    }
}