﻿
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unity3d.PlaneTriangulator
{
    public class DelaunayTriangulationBuilder
    {
        public const float Eps = 1e-9f;

        private struct ListNode
        {
            public ListNode(int left, int right)
            {
                Left = left;
                Right = right;
            }
            public int Left { get; private set; }
            public int Right { get; private set; }

            public void SetRight(int value)
            {
                Right = value;
            }

            public void SetLeft(int value)
            {
                Left = value;
            }
        }

        private class Edge
        {
            public Edge(int vertex1, int vertex2)
            {
                Vertex1 = vertex1;
                Vertex2 = vertex2;
            }

            public int Vertex1 { get; }
            public int Vertex2 { get; }

            public override bool Equals(object obj)
            {
                var edge = obj as Edge;
                if (edge == null)
                {
                    return false;
                }
                return edge.Vertex1 == Vertex1 && edge.Vertex2 == Vertex2;
            }

            public override int GetHashCode()
            {
                var hash1 = Vertex1.GetHashCode();
                var hash2 = Vertex2.GetHashCode();
                const uint offset = 0x9e3779b9;
                return (int)(hash1 ^ (hash2 + offset + (hash1 << 6) + (hash1 >> 2)));
            }
        }

        private class TwoVertices
        {
            public TwoVertices(int vertex1)
            {
                Vertex1 = vertex1;
            }

            public int Vertex1 { get; private set; } = -1;
            public int Vertex2 { get; private set; } = -1;

            public void Insert(int vertex)
            {
                if (Vertex1 == vertex || Vertex2 == vertex)
                    return;
                if (Vertex1 < 0)
                {
                    Vertex1 = vertex;
                }
                else
                {
                    Vertex2 = vertex;
                }
            }

            public void Replace(int u, int v)
            {
                if (Vertex1 == u)
                {
                    Vertex1 = v;
                }
                else if (Vertex2 == u)
                {
                    Vertex2 = v;
                }
                else
                {
                    Insert(v);
                }
            }

            public int Max()
            {
                return Mathf.Max(Vertex1, Vertex2);
            }

            public int Min()
            {
                if (Vertex1 < 0 || Vertex2 < 0)
                {
                    return Max();
                }
                return Mathf.Min(Vertex1, Vertex2);
            }
        }

        private static float CrossProduct(Vector2 leftVector, Vector2 rightVector)
        {
            return leftVector.x * rightVector.y - leftVector.y * rightVector.x;
        }
        private static Edge[] _recursionStack;

        private static bool CheckDelaunayCondition(int left, int right, int outer, int inner, List<Vector2> vertices)
        {
            var l = vertices[left];
            var r = vertices[right];
            var t = vertices[outer];
            var b = vertices[inner];

            // Проверка на то, что подан четырехугольник
            if (outer == inner)
            {
                return true;
            }

            // Проверка на выпуклость
            if (CrossProduct(l - t, b - t) < 0 || CrossProduct(r - t, b - t) > 0)
            {
                return true;
            }

            // Проверка условия Делоне, как в книге из статьи
            var sa = (t.x - r.x) * (t.x - l.x) + (t.y - r.y) * (t.y - l.y);
            var sb = (b.x - r.x) * (b.x - l.x) + (b.y - r.y) * (b.y - l.y);
            if (sa > -Eps && sb > -Eps)
            {
                return true;
            }

            if (!(sa < 0 && sb < 0))
            {
                var sc = CrossProduct(t - r, t - l);
                var sd = CrossProduct(b - r, b - l);
                if (sc < 0)
                    sc = -sc;
                if (sd < 0)
                    sd = -sd;
                if (sc * sb + sa * sd > -Eps)
                {
                    return true;
                }
            }
            return false;
        }

        private static void Insert(Edge key, int value, Dictionary<Edge, TwoVertices> graph)
        {
            if (!graph.ContainsKey(key))
            {
                graph.Add(key, new TwoVertices(-1));
            }
            graph[key].Insert(value);
        }

        private static void Replace(Edge key, int value1, int value2, Dictionary<Edge, TwoVertices> graph)
        {
            if (!graph.ContainsKey(key))
            {
                graph.Add(key, new TwoVertices(-1));
            }

            graph[key].Replace(value1, value2);
        }

        private static ListNode GetOrAdd(List<ListNode> convexHull, int index)
        {
            if (convexHull.Count <= index)
            {
                convexHull.Add(new ListNode());
            }
            return convexHull[index];
        }

        private static void FixTriangulation(int left, int right, int outer, List<Vector2> vertices, Dictionary<Edge, TwoVertices> graph)
        {
            _recursionStack[0] = new Edge(left, right);
            int stack_size = 1;
            while (stack_size > 0)
            {
                left = _recursionStack[stack_size - 1].Vertex1;
                right = _recursionStack[stack_size - 1].Vertex2;
                --stack_size;

                int inner =
                    graph[new Edge(Mathf.Min(left, right), Mathf.Max(left, right))].Min();
                if (CheckDelaunayCondition(left, right, outer, inner, vertices))
                {
                    // Если менять ничего в четырехугольнике не надо,
                    // просто добавляем недостающие ребра и выходим
                    Insert(new Edge(right, outer), left, graph);
                    Insert(new Edge(left, outer), right, graph);

                    Insert(new Edge(right < left ? right : left,
                        right < left ? left : right), outer, graph);
                    continue;
                }

                // Иначе перестраиваем триангуляцию в четырехугольнике
                Replace(new Edge(right, outer), left, inner, graph);
                Replace(new Edge(left, outer), right, inner, graph);

                Replace(new Edge(Mathf.Min(inner, left), Mathf.Max(inner, left)), right, outer, graph);
                Replace(new Edge(Mathf.Min(inner, right), Mathf.Max(inner, right)), left, outer, graph);

                graph.Remove(new Edge(Mathf.Min(left, right), Mathf.Max(left, right)));

                // И добавляем 2 новых рекурсивных вызова
                _recursionStack[stack_size++] = new Edge(left, inner);
                _recursionStack[stack_size++] = new Edge(inner, right);
            }
        }

        private static void AddVertexToTriangulation(int vertexId, List<Vector2> vertices, List<ListNode> convexHull, Dictionary<Edge, TwoVertices> graph)
        {
            int hullVertex = vertexId - 1;

            var lastVector = vertices[hullVertex] - vertices[vertexId];
            var nextHullVertex = convexHull[hullVertex].Right;
            var newVector = vertices[nextHullVertex] - vertices[vertexId];

            while (CrossProduct(lastVector, newVector) > -Eps)
            {
                FixTriangulation(hullVertex, nextHullVertex, vertexId, vertices, graph);

                hullVertex = nextHullVertex;
                lastVector = newVector;
                nextHullVertex = convexHull[hullVertex].Right;
                newVector = vertices[nextHullVertex] - vertices[vertexId];
            }
            GetOrAdd(convexHull, vertexId).SetRight(hullVertex);

            GetOrAdd(convexHull, GetOrAdd(convexHull, vertexId).Right).SetLeft(vertexId);
            GetOrAdd(convexHull, hullVertex).SetRight(vertexId);
        }

        public static IEnumerable<int> Build(IEnumerable<Vector2> vertices)
        {
            var sortedVertices = vertices.OrderBy(v => v.x).ToList();
            if (sortedVertices.Count == 4)
            {
                return new List<int>();
            }
            var convexHull = new List<ListNode>(sortedVertices.Count);
            var graph = new Dictionary<Edge, TwoVertices>();

            _recursionStack = new Edge[sortedVertices.Count];

            convexHull.Add(new ListNode(1, 1));
            convexHull.Add(new ListNode(0, 0));
            graph.Add(new Edge(0, 1), new TwoVertices(2));

            for (var i = 2; i < sortedVertices.Count; i++)
            {
                AddVertexToTriangulation(i, sortedVertices, convexHull, graph);
            }

            return new List<int>();
        }
    }
}