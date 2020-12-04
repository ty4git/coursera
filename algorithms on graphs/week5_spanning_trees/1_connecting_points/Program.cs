#define _1
#define _2

using System;
using System.Collections.Generic;
using System.Linq;

namespace _1_connecting_points
{
    class Program
    {
        private static double MinimumDistance(int[] xs, int[] ys) {
            var n = xs.Length;
            var edges = new List<Tuple<double, int, int>>();

            for (var i = 0; i < n; i++)
            {
                var xi = xs[i];
                var yi = ys[i];

                for (int j = i + 1; j < n; j++)
                {
                    var xj = xs[j];
                    var yj = ys[j];

                    var distance = Math.Sqrt(Math.Pow(xj - xi, 2) + Math.Pow(yj - yi, 2));
                    edges.Add(new Tuple<double, int, int>(distance, i, j));
                }
            }
            
            var sets = Enumerable.Range(0, n).Select(vertex => new HashSet<int>(new[] { vertex })).ToList();
            var orderedEdges = edges.OrderBy(edge => edge.Item1).ToList();
            var minimumSpanningTree = new List<Tuple<double, int, int>>();

            for (int i = 0; i < orderedEdges.Count; i++)
            {
                var edge = orderedEdges[i];
                var startSet = sets[edge.Item2];
                var endSet = sets[edge.Item3];
                
                if (startSet != endSet)
                {
                    Union(startSet, endSet, sets);
                    minimumSpanningTree.Add(edge);
                }
            }

            var result = minimumSpanningTree.Sum(edge => edge.Item1);
            return result;
        }

        private static void Union(HashSet<int> result, HashSet<int> source, List<HashSet<int>> sets)
        {
            foreach (var vertex in source)
            {
                result.Add(vertex);
                sets[vertex] = result;
            }
        }

        private static double Init(string[] inputs)
        {
            var n = int.Parse(inputs[0].Trim());

            var xs = new int[n];
            var ys = new int[n];

            for (var i = 0; i < n; i++)
            {
                var xyw = inputs[i + 1].Trim().Split();
                var x = int.Parse(xyw[0]);
                var y = int.Parse(xyw[1]);
                xs[i] = x;
                ys[i] = y;
            }
            
            var result = MinimumDistance(xs, ys);
            return result;
        }

        public static void Main(string[] args)
        {
            #if !DEBUG
            {
                var inputs = new List<string>();
                var line = "";
                while (!string.IsNullOrEmpty(line = Console.ReadLine()))
                {
                    inputs.Add(line);
                }
                var result = Init(inputs.ToArray());
                Console.WriteLine(result);
            }
            #endif

            #if DEBUG && _1
            {
                var inputs = new[] {
                    "4",
                    "0 0",
                    "0 1",
                    "1 0",
                    "1 1"
                };
                var actual = Init(inputs);
                var expected = 3.000000000;
                Console.WriteLine($"actual: {actual}, expected: {expected}, {actual == expected}");
            }
            #endif
            #if DEBUG && _2
            {
                var inputs = new[] {
                    "5",
                    "0 0",
                    "0 2",
                    "1 1",
                    "3 0",
                    "3 2"
                };
                var actual = Init(inputs);
                var roundedActual = Math.Round(actual, 9, MidpointRounding.AwayFromZero);
                var expected = 7.064495102;
                Console.WriteLine($"actual: {roundedActual}, expected: {expected}, {roundedActual == expected}");
            }
            #endif
        }
    }
}