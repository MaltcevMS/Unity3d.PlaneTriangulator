using System.Linq;

namespace Unity3d.PlaneTriangulator.TriangulationInternals
{
    public class Triangle
    {
        public int[] Vertices { get; }

        public Triangle(int v1, int v2, int v3)
        {
            Vertices = new [] {v1, v2, v3};
        }

        public override bool Equals(object obj)
        {
            var triangle = obj as Triangle;
            if (triangle == null)
            {
                return false;
            }
            return !Vertices.Except(triangle.Vertices).Any();
        }

        public override int GetHashCode()
        {
            return Vertices[0].GetHashCode() ^ Vertices[1].GetHashCode() ^ Vertices[2].GetHashCode();
        }
    }
}
