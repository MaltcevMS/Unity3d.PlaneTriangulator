using System.Collections.Generic;
using System.Linq;

namespace Unity3d.PlaneTriangulator.TriangulationDataTypes
{
    public abstract class EdgeBase
    {
        public int[] Vertices { get; }

        protected EdgeBase(int v1, int v2, int v3)
        {
            Vertices = new[] { v1, v2, v3 };
        }

        protected EdgeBase(int v1, int v2)
        {
            Vertices = new[] { v1, v2 };
        }

        public bool IsInside(IEnumerable<int> shapeVertices)
        {
            return !Vertices.Except(shapeVertices).Any();
        }

        public override bool Equals(object obj)
        {
            var triangle = obj as EdgeBase;
            if (triangle == null)
            {
                return false;
            }
            return IsInside(triangle.Vertices);
        }

        public override int GetHashCode()
        {
            var result = Vertices[0];
            for (var i = 1; i < Vertices.Length; i++)
            {
                result ^= Vertices[i];
            }
            return result;
        }
    }
}
