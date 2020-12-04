using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace _2_detecting_anomalies
{
    class Program
    {
        private static int NegativeCycle(List<List<int>> adj, List<List<int>> cost)
        {
            var distances = Enumerable.Range(0, adj.Count).Select(_ => 0).ToList();
            var parents = Enumerable.Range(0, adj.Count).Select(_ => (int?)null).ToList();
            
            var startVertex = 0;
            distances[startVertex] = 0;

            for (var i = 1; i < adj.Count; i++)
            {
                for (int currentVertex = 0; currentVertex < adj.Count; currentVertex++)
                {
                    var currentVertexDistance = distances[currentVertex];

                    var adjVertices = adj[currentVertex];
                    var adjVerticesCosts = cost[currentVertex];

                    for (int j = 0; j < adjVertices.Count; j++)
                    {
                        var adjVertex = adjVertices[j];
                        var adjVertexCost = adjVerticesCosts[j];

                        var distance = currentVertexDistance + adjVertexCost;
                        if (distance < distances[adjVertex])
                        {
                            distances[adjVertex] = distance;
                        }
                    }
                }
            }

            for (var i = 1; i < adj.Count; i++)
            {
                for (var currentVertex = 0; currentVertex < adj.Count; currentVertex++)
                {
                    var adjVertices = adj[currentVertex];
                    var adjVerticesCosts = cost[currentVertex];
                    
                    for (int j = 0; j < adjVertices.Count; j++)
                    {
                        var adjVertex = adjVertices[j];
                        var adjVertexCost = adjVerticesCosts[j];

                        if (distances[adjVertex] > distances[currentVertex] + adjVertexCost)
                        {
                            return 1;
                        }
                    }
                }
            }

            return 0;
        }

        private static int Init(string[] inputs)
        {
            var nm = inputs[0].Trim().Split();
            var n = int.Parse(nm[0]);
            var m = int.Parse(nm[1]);

            var adj = Enumerable.Range(0, n).Select(_ => new List<int>()).ToList();
            var cost = Enumerable.Range(0, n).Select(_ => new List<int>()).ToList();

            for (var i = 0; i < m; i++)
            {
                var xyw = inputs[i + 1].Trim().Split();
                var x = int.Parse(xyw[0]);
                var y = int.Parse(xyw[1]);
                var w = int.Parse(xyw[2]);
                adj[x - 1].Add(y - 1);
                cost[x - 1].Add(w);
            }
            var st = inputs[inputs.Length - 1].Trim().Split().Select(x => int.Parse(x) - 1).ToList();
            var s = st[0];
            var t = st[1];

            var result = NegativeCycle(adj, cost);
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
            #else
            {
                var inputs = new[] {
                    "4 4",
                    "1 2 -5",
                    "4 1 2",
                    "2 3 2",
                    "3 1 1"
                };
                var actual = Init(inputs);
                var expected = 1;
                Console.WriteLine($"actual: {actual}, expected: {expected}, {actual == expected}");
            }
            #endif
        }
    }
}
