using System.Collections.Generic;

namespace Unity3d.PlaneTriangulator.TriangulationDataTypes
{
    public class Edge : EdgeBase
    {
        public Edge(int v1, int v2) : base(v1, v2)
        {
        }

        public int Vertex1 => Vertices[0];
        public int Vertex2 => Vertices[1];

        public EdgeValidationResult Validate(IDictionary<Edge, int> existingEdges)
        {
            var firstVertexOfExistingEdge = -1;
            var edgeExists = existingEdges.TryGetValue(this, out firstVertexOfExistingEdge);
            var isValid = !edgeExists || firstVertexOfExistingEdge != Vertex1;
            return new EdgeValidationResult(edgeExists, isValid);
        }

        public struct EdgeValidationResult
        {
            public EdgeValidationResult(bool edgeExists, bool isValid)
            {
                EdgeExists = edgeExists;
                IsValid = isValid;

            }

            public bool EdgeExists { get; }
            public bool IsValid { get; }
        }
    }
}
