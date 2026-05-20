using System;
using System.Collections.Generic;
using System.IO;

namespace SortingExperiment
{
    /// <summary>
    /// Entry point for the sorting experiment application.
    /// Menu-driven interface; follow the prompts to generate input files
    /// or run sorting experiments.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nSorting Experiment");
            Console.WriteLine("==================");

            bool running = true;
            while (running)
            {
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("  1. Generate an input file");
                Console.WriteLine("  2. Run sorting experiments");
                Console.WriteLine("  3. Exit");
                Console.Write("\nEnter choice: ");

                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        HandleGenerate();
                        break;
                    case "2":
                        HandleRun();
                        break;
                    case "3":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                        break;
                }
            }
        }

        static void HandleGenerate()
        {
            Console.WriteLine("\n-- Generate Input File --");

            int size = PromptInt("Number of integers to generate: ", min: 1);

            Console.WriteLine("Ordering types: random, sorted, reverse, nearlysorted");
            string ordering = PromptString("Ordering type: ",  new[] { "random", "sorted", "reverse", "nearlysorted" });

            string outputFile = PromptPath("Output file path (e.g. data/random_10000.txt): ");

            Directory.CreateDirectory(Path.GetDirectoryName(outputFile) ?? ".");

            InputGenerator generator = new InputGenerator();
            generator.Generate(outputFile, size, ordering);
        }

        static void HandleRun()
        {
            Console.WriteLine("\n-- Run Sorting Experiments --");

            string inputFile = PromptExistingFile("Input file path: ");

            Console.WriteLine("Ordering types: random, sorted, reverse, nearlysorted");
            string ordering = PromptString("Ordering type of this file: ",
                new[] { "random", "sorted", "reverse", "nearlysorted" });

            int runs = PromptInt("Number of runs per algorithm: ", min: 1);

            string resultsFile = PromptPath("Results file path (e.g. results/results.csv): ");
            Directory.CreateDirectory(Path.GetDirectoryName(resultsFile) ?? ".");

            Console.WriteLine($"\nRunning experiments on: {inputFile}");
            Console.WriteLine($"Ordering: {ordering}, Runs per algorithm: {runs}\n");

            ExperimentRunner runner = new ExperimentRunner(inputFile, ordering);
            ResultReporter reporter = new ResultReporter(resultsFile);

            // --- Merge Sort ---
            Console.WriteLine("Running Merge Sort...");
            var mergeResults = runner.Run("MergeSort", (arr, ops) =>
            {
                long count = 0;
                SortingAlgorithm.MergeSort(arr, ref count);
                ops.Value = count;
            }, runs);
            reporter.Write(mergeResults);

            // --- Add your algorithms below this line ---
            // Follow the same pattern as MergeSort above.
            // Example structure (do not uncomment, implement your own):
            //
            // Console.WriteLine("Running [Your Algorithm]...");
            // var yourResults = runner.Run("[YourAlgorithmName]", (arr, ops) =>
            // {
            //     long count = 0;
            //     SortingAlgorithm.YourMethod(arr, ref count);
            //     ops.Value = count;
            // }, runs);
            // reporter.Write(yourResults);

            Console.WriteLine($"\nResults written to: {resultsFile}");
        }

        static int PromptInt(string message, int min = int.MinValue, int max = int.MaxValue)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine()?.Trim();
                if (int.TryParse(input, out int value) && value >= min && value <= max)
                    return value;
                Console.WriteLine($"Please enter a valid integer{(min != int.MinValue ? $" (minimum {min})" : "")}.");
            }
        }

        static string PromptString(string message, string[] validOptions)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine()?.Trim().ToLower();
                foreach (string option in validOptions)
                {
                    if (input == option)
                        return input;
                }
                Console.WriteLine($"Please enter one of: {string.Join(", ", validOptions)}");
            }
        }

        static string PromptPath(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine()?.Trim();
                if (!string.IsNullOrWhiteSpace(input))
                    return input;
                Console.WriteLine("Please enter a valid file path.");
            }
        }

        static string PromptExistingFile(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine()?.Trim();
                if (File.Exists(input))
                    return input;
                Console.WriteLine($"File not found: {input}. Please try again.");
            }
        }
    }
}