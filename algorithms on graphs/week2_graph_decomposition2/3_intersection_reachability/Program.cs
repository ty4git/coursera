using System;
using System.Collections.Generic;
using System.Linq;

namespace _3_intersection_reachability
{
    class Program
    {
        private static int NumberOfStronglyConnectedComponents(List<List<int>> adj) {
            var prePost = new Tuple<int, int>[adj.Count];
            var tick = 1;

            for (var vertex = 0; vertex < adj.Count; vertex++)
            {
                if (prePost[vertex] != null)
                {
                    continue;
                }

                var vertices = new List<int>();
                (tick, vertices) = Dfs(adj, vertex, prePost, tick);
            }

            var orderedVertices = prePost
                .Select((prePostTuple, vertex) => new { Vertex = vertex, PrePost = prePostTuple })
                .OrderByDescending(x => x.PrePost.Item2)
                .ToList();

            var reversedGraph = ReverseGraph(adj);
            var connectedComponentNumber = 0;
            var reversedPrePost = new Tuple<int, int>[adj.Count];
            var reversedTick = 1;

            for (var topVertexIndex = 0; topVertexIndex < orderedVertices.Count; topVertexIndex++)
            {
                var topVertex = orderedVertices[topVertexIndex].Vertex;
                if (reversedPrePost[topVertex] != null)
                {
                    continue;
                }

                var vertices = new List<int>();
                (reversedTick, vertices) = Dfs(reversedGraph, topVertex, reversedPrePost, reversedTick);
                connectedComponentNumber++;
            }
            
            return connectedComponentNumber;
        }

        private static List<List<int>> ReverseGraph(List<List<int>> adj)
        {
            var reversedGraph = Enumerable.Range(0, adj.Count).Select(_ => new List<int>()).ToList();

            for (var vertex = 0; vertex < adj.Count; vertex++)
            {
                var adjVertices = adj[vertex];
                for (var j = 0; j < adjVertices.Count; j++)
                {
                    var adjVertex = adjVertices[j];
                    reversedGraph[adjVertex].Add((int)vertex);
                }
            }

            return reversedGraph;
        }

        private static (int Tick, List<int> vertices) Dfs(List<List<int>> adj, int startVertex, Tuple<int, int>[] prePost, int tick)
        {
            var vertices = new HashSet<int>();
            var stack = new Stack<int>();
            stack.Push(startVertex);
            
            while (stack.Any())
            {
                var current = stack.Peek();
                vertices.Add(current);
                var prePostTuple = prePost[current] != null ? prePost[current] : new Tuple<int, int>(tick++, -1);
                if (prePostTuple.Item2 > -1)
                {
                    stack.Pop();
                    continue;
                }

                var adjVertices = adj[current];
                var newAdjVertices = adjVertices.Where(adjVertex => prePost[adjVertex] == null).ToList();

                if (newAdjVertices.Any())
                {
                    newAdjVertices.ForEach(adjVertex => stack.Push(adjVertex));
                }
                else
                {
                    prePostTuple = new Tuple<int, int>(prePostTuple.Item1, tick++);
                    stack.Pop();
                }
                prePost[current] = prePostTuple;
            }

            vertices.Remove(startVertex);
            return (tick, vertices.ToList());
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

            var result = NumberOfStronglyConnectedComponents(adj);
            return result;
        }

        public static void Main(string[] args)
        {
            #if !DEBUG
            {
                var inputs = new List<string>();
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
                var actual = Init(inputs);
                var expected = 2;
                Console.WriteLine($"actual: {actual}, expected: {expected}, {actual == expected}");
            }
            {
                var inputs = new[] {
                    "5 7",
                    "2 1",
                    "3 2",
                    "3 1",
                    "4 3",
                    "4 1",
                    "5 2",
                    "5 3"
                };
                var actual = Init(inputs);
                var expected = 5;
                Console.WriteLine($"actual: {actual}, expected: {expected}, {actual == expected}");
            }
            #endif
        }
    }
}