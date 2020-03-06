using UnityEngine;

namespace Unity3d.PlaneTriangulator.TriangulationDataTypes
{
    public class TwoVertices
    {
        public TwoVertices() : this(-1)
        { }

        public TwoVertices(int vertex1)
        {
            Vertex1 = vertex1;
            Vertex2 = -1;
        }

        public int Vertex1 { get; private set; }
        public int Vertex2 { get; private set; }
         
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
}
