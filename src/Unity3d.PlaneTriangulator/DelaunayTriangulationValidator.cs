using System.Collections.Generic;
using Unity3d.PlaneTriangulator.TriangulationDataTypes;

namespace Assets.Scripts.Utility.Triangulation
{
    public class DelaunayTriangulationValidator
    {
        public const float Eps = 1e-9f;

        public static bool Validate(int leftVertexIndex, int rightVertexIndex, int outerVertexIndex, int innerVertexIndex, 
            IReadOnlyList<SortedVertex> vertices)
        {

            var leftVertex = vertices[leftVertexIndex].Vertex;
            var rightVertex = vertices[rightVertexIndex].Vertex;
            var outerVertex = vertices[outerVertexIndex].Vertex;
            var innerVertex = vertices[innerVertexIndex].Vertex;

            if (outerVertexIndex == innerVertexIndex)
            {
                return true;
            }

            if (TriangulationHelper.CrossProduct(leftVertex - outerVertex, innerVertex - outerVertex) < 0 || 
                TriangulationHelper.CrossProduct(rightVertex - outerVertex, innerVertex - outerVertex) > 0)
            {
                return true;
            }

            var sa = (outerVertex.x - rightVertex.x) * (outerVertex.x - leftVertex.x) + 
                     (outerVertex.y - rightVertex.y) * (outerVertex.y - leftVertex.y);
            var sb = (innerVertex.x - rightVertex.x) * (innerVertex.x - leftVertex.x) + 
                     (innerVertex.y - rightVertex.y) * (innerVertex.y - leftVertex.y);

            if (sa > -Eps && sb > -Eps)
            {
                return true;
            }

            if (sa < 0 && sb < 0)
            {
                return false;
            }

            var sc = TriangulationHelper.CrossProduct(outerVertex - rightVertex, outerVertex - leftVertex);
            var sd = TriangulationHelper.CrossProduct(innerVertex - rightVertex, innerVertex - leftVertex);
            if (sc < 0)
                sc = -sc;
            if (sd < 0)
                sd = -sd;

            return sc * sb + sa * sd > -Eps;
        }
    }
}
