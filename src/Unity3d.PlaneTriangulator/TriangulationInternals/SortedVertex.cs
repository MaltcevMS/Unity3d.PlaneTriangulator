using UnityEngine;

namespace Unity3d.PlaneTriangulator.TriangulationInternals
{
    internal class SortedVertex
    {
        public SortedVertex(int index, Vector2 vertex)
        {
            InitialIndex = index;
            Vertex = vertex;
        }
        public int InitialIndex { get; }
        public Vector2 Vertex { get; }
    }
}
