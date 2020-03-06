namespace Unity3d.PlaneTriangulator.TriangulationDataTypes
{
    public class Triangle : EdgeBase
    {
        public Triangle(int v1, int v2, int v3) : base(v1, v2, v3)
        {
        }

        public Edge[] GetEdges()
        {
            return new[]
            {
                new Edge(Vertices[0], Vertices[1]),
                new Edge(Vertices[1], Vertices[2]),
                new Edge(Vertices[2], Vertices[0])
            };
        }
    }
}
