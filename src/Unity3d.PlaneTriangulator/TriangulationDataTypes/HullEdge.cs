using System;

namespace Unity3d.PlaneTriangulator.TriangulationDataTypes
{
    public class HullEdge : EdgeBase
    {
        public HullEdge(int v1, int v2) : base(v1, v2)
        {
        }
        public enum IterationDirections
        {
            Right = 0,
            Left = 1
        }

        public int Left => Vertices[0];
        public int Right => Vertices[1];

        public int Get(IterationDirections direction)
        {
            switch (direction)
            {
                case IterationDirections.Left: return Left;
                case IterationDirections.Right: return Right;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public void Set(IterationDirections direction, int value)
        {
            switch (direction)
            {
                case IterationDirections.Left:
                    Vertices[0] = value;
                    break;
                case IterationDirections.Right:
                    Vertices[1] = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}
