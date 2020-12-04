using System;
using System.Collections.Generic;
using System.Linq;

namespace _2_adding_exits_to_maze
{
    public class ConnectedComponents
    {
        private static void Union(List<int> component, List<int> adjComponent, List<int>[] components)
        {
            component.AddRange(adjComponent);
            foreach (var v in adjComponent)
            {
                components[v] = component;
            }
        }

        private static int NumberOfComponents(List<List<int>> adj)
        {
            var components = new List<int>[adj.Count];
            
            for (var i = 0; i < adj.Count; i++)
            {
                var vertex = i;
                var vertexComponent = components[vertex] ?? (components[vertex] = new List<int>() { vertex });
                var adjVertexes = adj[vertex];

                var adjVertexComponents = adjVertexes
                    .Select(adjVertex => components[adjVertex] ?? new List<int> { adjVertex })
                    .Where(adjVertexComponent => vertexComponent != adjVertexComponent)
                    .Distinct()
                    .ToList();

                adjVertexComponents.ForEach(adjVertexComponent => Union(vertexComponent, adjVertexComponent, components));
            }

            var result = components.Distinct().Count();
            return result;
        }

        private static int Init(string[] inputs)
        {
            var nm = inputs[0].Trim().Split();
            var n = int.Parse(nm[0]);
            var m = int.Parse(nm[1]);

            var adj = new List<List<int>>(n);
            for (var i = 0; i < n; i++)
            {
                adj.Add(new List<int>());
            }

            for (var i = 0; i < m; i++)
            {
                var xyRaw = inputs[i + 1];
                var xy = xyRaw.Trim().Split();
                var x = int.Parse(xy[0]); var y = int.Parse(xy[1]);
                adj[x - 1].Add(y - 1);
                adj[y - 1].Add(x - 1);
            }

            var result = NumberOfComponents(adj);
            return result;
        }

        public static void Main(string[] args)
        {
            var inputs = new List<string>();
            var line = "";
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                inputs.Add(line);
            }
            var result = Init(inputs.ToArray());
            Console.WriteLine(result);

            // {
            //     var inputs = new[] {
            //         "5 2",
            //         "1 2",
            //         "3 2"
            //     };
            //     var result = Init(inputs);
            //     Console.WriteLine($"{result}, {result == 3}");
            // }

            // {
            //     var inputs = new[] {
            //         "4 4",
            //         "1 2",
            //         "3 2",
            //         "4 3",
            //         "1 4"
            //     };
            //     var result = Init(inputs);
            //     Console.WriteLine($"{result}, {result == 1}");
            // }

            // {
            //     var inputs = new[] {
            //         "100 100",
            //         "27 96",
            //         "6 9",
            //         "81 98",
            //         "21 94",
            //         "22 68",
            //         "76 100",
            //         "8 50",
            //         "38 86",
            //         "71 75",
            //         "32 93",
            //         "16 50",
            //         "71 84",
            //         "6 72",
            //         "22 58",
            //         "7 19",
            //         "19 76",
            //         "44 75",
            //         "24 76",
            //         "31 35",
            //         "11 89",
            //         "42 98",
            //         "63 92",
            //         "37 38",
            //         "20 98",
            //         "45 91",
            //         "23 53",
            //         "37 91",
            //         "76 93",
            //         "67 90",
            //         "12 22",
            //         "43 52",
            //         "23 56",
            //         "67 68",
            //         "1 21",
            //         "17 83",
            //         "63 72",
            //         "30 32",
            //         "7 91",
            //         "50 69",
            //         "38 44",
            //         "55 89",
            //         "15 23",
            //         "11 72",
            //         "28 42",
            //         "22 69",
            //         "56 79",
            //         "53 58",
            //         "5 83",
            //         "13 72",
            //         "7 93",
            //         "20 54",
            //         "21 55",
            //         "66 89",
            //         "2 91",
            //         "18 88",
            //         "26 64",
            //         "11 61",
            //         "28 59",
            //         "12 86",
            //         "42 95",
            //         "17 82",
            //         "50 66",
            //         "66 99",
            //         "40 71",
            //         "20 40",
            //         "5 66",
            //         "92 95",
            //         "32 46",
            //         "7 36",
            //         "44 94",
            //         "6 31",
            //         "19 67",
            //         "26 57",
            //         "53 84",
            //         "10 68",
            //         "28 74",
            //         "34 94",
            //         "25 61",
            //         "71 88",
            //         "10 89",
            //         "28 52",
            //         "72 79",
            //         "39 73",
            //         "11 80",
            //         "44 79",
            //         "13 77",
            //         "30 96",
            //         "30 53",
            //         "10 39",
            //         "1 90",
            //         "40 91",
            //         "62 71",
            //         "44 54",
            //         "15 17",
            //         "69 74",
            //         "13 67",
            //         "24 69",
            //         "34 96",
            //         "21 50",
            //         "20 91"
            //     };

            //     var result = Init(inputs);
            //     Console.WriteLine($"{result}, {result == 19}");
            // }
        }
    }
}
