using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utility.Triangulation;
using Assets.Scripts.Utility.Triangulation.DataStructures;
using Unity3d.PlaneTriangulator.TriangulationDataTypes;
using UnityEngine;

namespace Unity3d.PlaneTriangulator
{
    public class DelaunayTriangulationBuilder
    {
        public IEnumerable<int> Build(IEnumerable<Vector2> vertices)
        {
            return Build(vertices, new List<IEnumerable<int>>());
        }

        public IEnumerable<int> Build(IEnumerable<Vector2> vertices, IReadOnlyCollection<IEnumerable<int>> holeVertices)
        {
            var sortedVertices = vertices.Select((v, index) => new SortedVertex(index, v))
                .OrderBy(v => v.Vertex.x).ThenBy(v => v.Vertex.y).ToList();

            if (sortedVertices.Count < 3)
            {
                throw new ArgumentException($"Minimum vertices count for triangulation is 3, but was {sortedVertices.Count}");
            }

            var convexHull = new HullEdgesList(sortedVertices.Count);
            var graph = new TriangulationGraph();

            convexHull.Add(new HullEdge(1, 1));
            convexHull.Add(new HullEdge(0, 0));
            graph[new Edge(0, 1)].Insert(2);

            for (var i = 2; i < sortedVertices.Count; i++)
            {
                AddVertexToTriangulation(i, sortedVertices, convexHull, graph);
            }

            return DelaunayTriangulationConverter.ConvertToTriangulation(graph, sortedVertices, holeVertices);
        }

        #region private methods
        private static void AddVertexToTriangulation(int vertexId, List<SortedVertex> vertices, HullEdgesList convexHull,
            TriangulationGraph graph)
        {
            FixTriangulationIteratively(vertexId, vertices, convexHull, HullEdge.IterationDirections.Right,
                (v1, v2) => TriangulationHelper.CrossProduct(v1, v2) > -DelaunayTriangulationValidator.Eps,
                (left, right, outer) => FixTriangulation(left, right, outer, vertices, graph));

            var hullVertex = FixTriangulationIteratively(vertexId, vertices, convexHull, HullEdge.IterationDirections.Left,
                (v1, v2) => TriangulationHelper.CrossProduct(v1, v2) < DelaunayTriangulationValidator.Eps,
                (left, right, outer) => FixTriangulation(right, left, outer, vertices, graph));

            convexHull[convexHull[vertexId].Right].Set(HullEdge.IterationDirections.Left, vertexId);
            convexHull[hullVertex].Set(HullEdge.IterationDirections.Right, vertexId);
        }

        private static int FixTriangulationIteratively(int vertexId, List<SortedVertex> vertices, HullEdgesList convexHull, HullEdge.IterationDirections direction,
            Func<Vector2, Vector2, bool> condition, Action<int, int, int> fixTriangulation)
        {
            var iterationsCount = 0;
            const int maxIterationsCount = 100;

            var hullVertex = vertexId - 1;

            var lastVector = vertices[hullVertex].Vertex - vertices[vertexId].Vertex;
            var nextHullVertex = convexHull[hullVertex].Get(direction);
            var newVector = vertices[nextHullVertex].Vertex - vertices[vertexId].Vertex;

            while (condition(lastVector, newVector) && iterationsCount < maxIterationsCount)
            {
                fixTriangulation(hullVertex, nextHullVertex, vertexId);

                hullVertex = nextHullVertex;
                lastVector = newVector;
                nextHullVertex = convexHull[hullVertex].Get(direction);
                newVector = vertices[nextHullVertex].Vertex - vertices[vertexId].Vertex;
                iterationsCount++;
            }
            convexHull[vertexId].Set(direction, hullVertex);

            return hullVertex;
        }

        private static void FixTriangulation(int left, int right, int outer, IReadOnlyList<SortedVertex> vertices,
            TriangulationGraph graph)
        {
            var recursionStack = new Edge[vertices.Count];

            recursionStack[0] = new Edge(left, right);
            var stackSize = 1;
            while (stackSize > 0)
            {
                left = recursionStack[stackSize - 1].Vertex1;
                right = recursionStack[stackSize - 1].Vertex2;
                --stackSize;

                var inner = graph[new Edge(Mathf.Min(left, right), Mathf.Max(left, right))].Min();

                if (DelaunayTriangulationValidator.Validate(left, right, outer, inner, vertices))
                {
                    graph[new Edge(right, outer)].Insert(left);
                    graph[new Edge(left, outer)].Insert(right);

                    if (right < left)
                    {
                        TriangulationHelper.Swap(ref left, ref right);
                    }
                    graph[new Edge(left, right)].Insert(outer);
                    continue;
                }

                graph[new Edge(right, outer)].Replace(left, inner);
                graph[new Edge(left, outer)].Replace(right, inner);

                graph[new Edge(Mathf.Min(inner, left), Mathf.Max(inner, left))].Replace(right, outer);
                graph[new Edge(Mathf.Min(inner, right), Mathf.Max(inner, right))].Replace(left, outer);

                graph.Remove(new Edge(Mathf.Min(left, right), Mathf.Max(left, right)));

                recursionStack[stackSize++] = new Edge(left, inner);
                recursionStack[stackSize++] = new Edge(inner, right);
            }
        }
        #endregion
    }
}
