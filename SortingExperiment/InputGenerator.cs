using System;
using System.IO;
using System.Linq;

namespace SortingExperiment
{
    /// <summary>
    /// Generates integer input files for sorting experiments.
    /// Supports four ordering types: random, sorted, reverse sorted, and nearly sorted.
    /// Nearly sorted applies a small number of random swaps to an otherwise sorted sequence,
    /// simulating real-world data that is mostly ordered but not perfectly so.
    /// </summary>
    public class InputGenerator
    {
        private readonly Random _random;

        // Controls what proportion of elements are swapped to produce nearly sorted input.
        // At 0.05, approximately 5% of elements are displaced from their sorted positions.
        private const double NearlySortedSwapFraction = 0.05;

        public InputGenerator(int seed = 42)
        {
            _random = new Random(seed);
        }

        /// <summary>
        /// Generates an input file at the specified path.
        /// </summary>
        /// <param name="filePath">Destination file path.</param>
        /// <param name="size">Number of integers to generate.</param>
        /// <param name="orderingType">One of: random, sorted, reverse, nearlysorted</param>
        public void Generate(string filePath, int size, string orderingType)
        {
            int[] data = GenerateData(size, orderingType);
            try
            {
                File.WriteAllLines(filePath, data.Select(x => x.ToString()));
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Could not save file: " + ex.ToString());
            }

            Console.WriteLine($"Generated {size} integers ({orderingType}) -> {filePath}");
        }

        private int[] GenerateData(int size, string orderingType)
        {
            int[] data = Enumerable.Range(1, size).ToArray();

            switch (orderingType.ToLower())
            {
                case "random":
                    return Shuffle(data);

                case "sorted":
                    return data;

                case "reverse":
                    return data.Reverse().ToArray();

                case "nearlysorted":
                    return NearlySort(data);

                default:
                    throw new ArgumentException($"Unknown ordering type: {orderingType}. Use random, sorted, reverse, or nearlysorted.");
            }
        }

        private int[] Shuffle(int[] data)
        {
            int[] shuffled = (int[])data.Clone();
            for (int i = shuffled.Length - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
            }
            return shuffled;
        }

        private int[] NearlySort(int[] data)
        {
            int[] nearly = (int[])data.Clone();
            int swapCount = Math.Max(1, (int)(data.Length * NearlySortedSwapFraction));

            for (int i = 0; i < swapCount; i++)
            {
                int a = _random.Next(nearly.Length);
                int b = _random.Next(nearly.Length);
                (nearly[a], nearly[b]) = (nearly[b], nearly[a]);
            }
            return nearly;
        }
    }
}