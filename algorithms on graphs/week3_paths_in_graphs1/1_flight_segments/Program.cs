﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace _1_finding_exit_from_maze
{
    class Program
    {
        private static int Distance(List<List<int>> adj, int x, int y)
        {
            var parents = Enumerable.Range(0, adj.Count).Select(_ => -1).ToArray();
            var visited = new bool[adj.Count];
            var queue = new Queue<int>();
            queue.Enqueue(x);
            visited[x] = true;

            while (queue.Any())
            {
                var current = queue.Dequeue();
                var adjVertecis = adj[current];
                var newAdjVertices = adjVertecis.Where(v => !visited[v]).ToList();
                newAdjVertices.ForEach(v => { 
                    queue.Enqueue(v);
                    visited[v] = true;
                    parents[v] = current;
                });
            }

            var path = new Stack<int>();
            for (var current = y; current != -1 && current != parents[x];)
            {
                path.Push(current);
                current = parents[current];
            }

            return path.Peek() == x ? path.Count - 1 : -1;
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
                var x = int.Parse(xy[0]); var y = int.Parse(xy[1]);
                adj[x - 1].Add(y - 1);
                adj[y - 1].Add(x - 1);
            }

            var se = inputs[inputs.Length - 1].Trim().Split();
            var s = int.Parse(se[0]) - 1;
            var e = int.Parse(se[1]) - 1;

            var result = Distance(adj, s, e);
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
                    "3 1",
                    "2 4"
                };
                var actual = Init(inputs);
                var expected = 2;
                Console.WriteLine($"{actual}, {expected}, {actual == expected}");
            }
            {
                var inputs = new[] {
                    "5 4",
                    "5 2",
                    "1 3",
                    "3 4",
                    "1 4",
                    "3 5"
                };
                var actual = Init(inputs);
                var expected = -1;
                Console.WriteLine($"{actual}, {expected}, {actual == expected}");
            }
            #endif
        }
    }
}