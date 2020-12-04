using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace _1_minimum_flight_cost
{
    class Program
    {
        class VertexDistanceComparer : Comparer<Tuple<int, long>>
        {
            public override int Compare(Tuple<int, long> x, Tuple<int, long> y)
            {
                if (x == null || y == null)
                {
                    throw new Exception();
                }

                return Comparer<long>.Default.Compare(x.Item2, y.Item2);
            }
        }

        private static long Distance(List<List<int>> adj, List<List<int>> cost, int s, int t) {
            var distances = Enumerable.Range(0, adj.Count).Select(_ => long.MaxValue).ToList();
            var parents = Enumerable.Range(0, adj.Count).Select(_ => -1).ToList();
            var priorityQueue = new MinHeap<Tuple<int, long>>(new VertexDistanceComparer());

            var startVertex = s;
            distances[startVertex] = 0;
            priorityQueue.Add(new Tuple<int, long>(startVertex, distances[startVertex]));

            while (priorityQueue.Any())
            {
                var currentTuple = priorityQueue.ExtractDominating();
                var currentVertex = currentTuple.Item1;
                var currentVertexDistance = distances[currentVertex];
                var adjVertices = adj[currentVertex];
                var adjVerticesCosts = cost[currentVertex];

                for (var i = 0; i < adjVertices.Count; i++)
                {
                    var adjVertex = adjVertices[i];
                    var adjVertexWeight = adjVerticesCosts[i];
                    var distance = currentVertexDistance + adjVertexWeight;

                    if (distance < distances[adjVertex])
                    {
                        distances[adjVertex] = distance;
                        parents[adjVertex] = currentVertex;
                        priorityQueue.Add(new Tuple<int, long>(adjVertex, distance));
                    }
                }
            }

            return distances[t] < int.MaxValue ? distances[t] : -1;
        }

        private static long Init(string[] inputs)
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

            var result = Distance(adj, cost, s, t);
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
                    "1 2 1",
                    "4 1 2",
                    "2 3 2",
                    "1 3 5",
                    "1 3"
                };
                var actual = Init(inputs);
                var expected = 3;
                Console.WriteLine($"actual: {actual}, expected: {expected}, {actual == expected}");
            }
            {
                var inputs = new[] {
                    "5 9",
                    "1 2 4",
                    "1 3 2",
                    "2 3 2",
                    "3 2 1",
                    "2 4 2",
                    "3 5 4",
                    "5 4 1",
                    "2 5 3",
                    "3 4 4",
                    "1 5"
                };
                var actual = Init(inputs);
                var expected = 6;
                Console.WriteLine($"actual: {actual}, expected: {expected}, {actual == expected}");
            }
            {
                var inputs = new[] {
                    "3 3",
                    "1 2 7",
                    "1 3 5",
                    "2 3 2",
                    "3 2"
                };
                var actual = Init(inputs);
                var expected = -1;
                Console.WriteLine($"actual: {actual}, expected: {expected}, {actual == expected}");
            }
            {
                var inputs = new[] {
                    "6 11  ",
                    "1 2 3 ",
                    "1 3 10",
                    "2 4 3 ",
                    "2 5 5 ",
                    "2 3 8 ",
                    "3 2 2 ",
                    "3 5 5 ",
                    "4 3 3 ",
                    "4 5 1 ",
                    "4 6 2 ",
                    "5 6 0 ",
                    "1 6   "
                };
                var actual = Init(inputs);
                var expected = 7;
                Console.WriteLine($"actual: {actual}, expected: {expected}, {actual == expected}");
            }
            {
                var inputs = new[] {
                    "5 10",
                    "3 5 49438153",
                    "2 3 78072041",
                    "3 4 12409612",
                    "1 3 88526216",
                    "5 2 6876525",
                    "1 4 28703032",
                    "4 1 24629785",
                    "4 5 96300300",
                    "4 3 317823",
                    "2 5 22632987",
                    "4 2"
                };
                var actual = Init(inputs);
                var expected = 56632501;
                Console.WriteLine($"actual: {actual}, expected: {expected}, {actual == expected}");
            }
            #endif
        }
    }
}

public abstract class Heap<T> : IEnumerable<T>
{
    private const int InitialCapacity = 0;
    private const int GrowFactor = 2;
    private const int MinGrow = 1;

    private int _capacity = InitialCapacity;
    private T[] _heap = new T[InitialCapacity];
    private int _tail = 0;

    public int Count { get { return _tail; } }
    public int Capacity { get { return _capacity; } }

    protected Comparer<T> Comparer { get; private set; }
    protected abstract bool Dominates(T x, T y);

    protected Heap() : this(Comparer<T>.Default)
    {
    }

    protected Heap(Comparer<T> comparer) : this(Enumerable.Empty<T>(), comparer)
    {
    }

    protected Heap(IEnumerable<T> collection)
        : this(collection, Comparer<T>.Default)
    {
    }

    protected Heap(IEnumerable<T> collection, Comparer<T> comparer)
    {
        if (collection == null) throw new ArgumentNullException("collection");
        if (comparer == null) throw new ArgumentNullException("comparer");

        Comparer = comparer;

        foreach (var item in collection)
        {
            if (Count == Capacity)
                Grow();

            _heap[_tail++] = item;
        }

        for (int i = Parent(_tail - 1); i >= 0; i--)
            BubbleDown(i);
    }

    public void Add(T item)
    {
        if (Count == Capacity)
            Grow();

        _heap[_tail++] = item;
        BubbleUp(_tail - 1);
    }

    private void BubbleUp(int i)
    {
        if (i == 0 || Dominates(_heap[Parent(i)], _heap[i])) 
            return; //correct domination (or root)

        Swap(i, Parent(i));
        BubbleUp(Parent(i));
    }

    public T GetMin()
    {
        if (Count == 0) throw new InvalidOperationException("Heap is empty");
        return _heap[0];
    }

    public T ExtractDominating()
    {
        if (Count == 0) throw new InvalidOperationException("Heap is empty");
        T ret = _heap[0];
        _tail--;
        Swap(_tail, 0);
        BubbleDown(0);
        return ret;
    }

    private void BubbleDown(int i)
    {
        int dominatingNode = Dominating(i);
        if (dominatingNode == i) return;
        Swap(i, dominatingNode);
        BubbleDown(dominatingNode);
    }

    private int Dominating(int i)
    {
        int dominatingNode = i;
        dominatingNode = GetDominating(YoungChild(i), dominatingNode);
        dominatingNode = GetDominating(OldChild(i), dominatingNode);

        return dominatingNode;
    }

    private int GetDominating(int newNode, int dominatingNode)
    {
        if (newNode < _tail && !Dominates(_heap[dominatingNode], _heap[newNode]))
            return newNode;
        else
            return dominatingNode;
    }

    private void Swap(int i, int j)
    {
        T tmp = _heap[i];
        _heap[i] = _heap[j];
        _heap[j] = tmp;
    }

    private static int Parent(int i)
    {
        return (i + 1)/2 - 1;
    }

    private static int YoungChild(int i)
    {
        return (i + 1)*2 - 1;
    }

    private static int OldChild(int i)
    {
        return YoungChild(i) + 1;
    }

    private void Grow()
    {
        int newCapacity = _capacity*GrowFactor + MinGrow;
        var newHeap = new T[newCapacity];
        Array.Copy(_heap, newHeap, _capacity);
        _heap = newHeap;
        _capacity = newCapacity;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _heap.Take(Count).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class MinHeap<T> : Heap<T>
{
    public MinHeap()
        : this(Comparer<T>.Default)
    {
    }

    public MinHeap(Comparer<T> comparer)
        : base(comparer)
    {
    }

    public MinHeap(IEnumerable<T> collection) : base(collection)
    {
    }

    public MinHeap(IEnumerable<T> collection, Comparer<T> comparer)
        : base(collection, comparer)
    {
    }

    protected override bool Dominates(T x, T y)
    {
        return Comparer.Compare(x, y) <= 0;
    }
}
