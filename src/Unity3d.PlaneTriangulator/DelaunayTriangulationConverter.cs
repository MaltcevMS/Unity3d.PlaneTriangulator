using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utility.Triangulation.DataStructures;
using Unity3d.PlaneTriangulator.TriangulationDataTypes;

namespace Assets.Scripts.Utility.Triangulation
{
    public class DelaunayTriangulationConverter
    {
        public static IEnumerable<int> ConvertToTriangulation(TriangulationGraph graph,
            IReadOnlyList<SortedVertex> vertices, IReadOnlyCollection<IEnumerable<int>> holeVertices)
        {
            var triangles = new HashSet<Triangle>();

            foreach (var graphItem in graph.Items)
            {
                var edge = graphItem.Key;
                var twoVertices = graphItem.Value;

                if (twoVertices.Vertex2 < 0)
                {
                    continue;
                }

                var edgeV1 = vertices[edge.Vertex1];
                var edgeV2 = vertices[edge.Vertex2];
                var v1 = vertices[twoVertices.Vertex1];
                var v2 = vertices[twoVertices.Vertex2];

                AddQuad(edgeV1, edgeV2, v1, v2, triangles, holeVertices);
            }

            return triangles.SelectMany(t => t.Vertices);
        }

        private static void AddTriangleIfNeeded(ISet<Triangle> existingTriangles, Triangle triangle,
            IEnumerable<IEnumerable<int>> holeVertices)
        {
            if (holeVertices.Any(triangle.IsInside) || existingTriangles.Contains(triangle))
            {
                return;
            }

            existingTriangles.Add(triangle);
        }

        private static Triangle CreateTriangle(SortedVertex v1, SortedVertex v2, SortedVertex v3)
        {
            FixWindingOrder(ref v1, ref v2, v3);
            var triangle = new Triangle(v1.InitialIndex, v2.InitialIndex, v3.InitialIndex);
            return triangle;
        }

        private static void AddTriangle(SortedVertex v1, SortedVertex v2, SortedVertex v3, ISet<Triangle> existingTriangles,
            IEnumerable<IEnumerable<int>> holeVertices)
        {
            var triangle = CreateTriangle(v1, v2, v3);
            if (triangle != null)
            {
                AddTriangleIfNeeded(existingTriangles, triangle, holeVertices);
            }
        }

        private static void AddQuad(SortedVertex v1, SortedVertex v2, SortedVertex v3, SortedVertex v4,
            ISet<Triangle> existingTriangles, IReadOnlyCollection<IEnumerable<int>> holeVertices)
        {
            AddTriangle(v1, v2, v3, existingTriangles, holeVertices);
            AddTriangle(v2, v1, v4, existingTriangles, holeVertices);
        }

        private static void FixWindingOrder(ref SortedVertex edgeV1, ref SortedVertex edgeV2, SortedVertex v1)
        {
            var crossProduct = TriangulationHelper.CrossProduct(edgeV2.Vertex - edgeV1.Vertex, v1.Vertex - edgeV2.Vertex);
            if (crossProduct > 0)
            {
                TriangulationHelper.Swap(ref edgeV1, ref edgeV2);
            }
        }
    }
}
