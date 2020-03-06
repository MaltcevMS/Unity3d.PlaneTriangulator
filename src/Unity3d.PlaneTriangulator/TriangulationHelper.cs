using Unity3d.PlaneTriangulator.TriangulationDataTypes;
using UnityEngine;

namespace Assets.Scripts.Utility.Triangulation
{
    public static class TriangulationHelper
    {
        public static void Swap(ref int left, ref int right)
        {
            var temp = left;
            left = right;
            right = temp;
        }

        public static void Swap(ref SortedVertex v1, ref SortedVertex v2)
        {
            var temp = v1;
            v1 = v2;
            v2 = temp;
        }

        public static float CrossProduct(Vector2 leftVector, Vector2 rightVector)
        {
            return leftVector.x * rightVector.y - leftVector.y * rightVector.x;
        }
    }
}
