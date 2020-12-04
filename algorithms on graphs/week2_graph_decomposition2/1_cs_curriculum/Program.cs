using System;
using System.Collections.Generic;
using System.Linq;

namespace _1_cs_curriculum
{
    class Program
    {
        private static int Acyclic(List<List<int>> adj)
        {
            var prePost = new Tuple<int, int>[adj.Count];
            var tick = 0;

            for (var vertex = 0; vertex < adj.Count; vertex++)
            {
                if (prePost[vertex] != null)
                {
                    continue;
                }

                var hasCycle = Dfs(adj, vertex, prePost, ref tick);
                if (hasCycle)
                {
                    return 1;
                }
            }

            return 0;
        }

        private static bool Dfs(List<List<int>> adj, int startVertex, Tuple<int, int>[] prePost, ref int tick)
        {
            var stack = new Stack<int>();
            stack.Push(startVertex);
            
            while (stack.Any())
            {
                var current = stack.Peek();
                var prePostTuple = prePost[current] != null ? prePost[current] : new Tuple<int, int>(++tick, -1);

                var adjVertices = adj[current];

                var cycle = adjVertices
                    .Where(adjVertex => prePost[adjVertex] != null && prePost[adjVertex].Item2 == -1)
                    .ToList();
                if (cycle.Any())
                {
                    return true;
                }

                var newAdjVertices = adjVertices.Where(adjVertex => prePost[adjVertex] == null).ToList();

                if (newAdjVertices.Any())
                {
                    newAdjVertices.ForEach(adjVertex => stack.Push(adjVertex));
                }
                else
                {
                    prePostTuple = new Tuple<int, int>(prePostTuple.Item1, ++tick);
                    stack.Pop();
                }
                prePost[current] = prePostTuple;
            }

            return false;
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
                var xy = inputs[i + 1].Trim().Split();
                var x = int.Parse(xy[0]);
                var y = int.Parse(xy[1]);
                adj[x - 1].Add(y - 1);
            }

            var result = Acyclic(adj);
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
                    "1 2",
                    "4 1",
                    "2 3",
                    "3 1"
                };
                var result = Init(inputs);
                Console.WriteLine($"{result}, {result == 1}");
            }
            {
                var inputs = new[] {
                    "5 7",
                    "1 2",
                    "2 3",
                    "1 3",
                    "3 4",
                    "1 4",
                    "2 5",
                    "3 5"
                };
                var result = Init(inputs);
                Console.WriteLine($"{result}, {result == 0}");
            }
            #endif
        }
    }
}