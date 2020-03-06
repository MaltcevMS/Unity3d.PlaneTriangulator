using NUnit.Framework;
using Unity3d.PlaneTriangulator.TriangulationDataTypes;

namespace Unity3d.PlaneTriangulator.Tests.TriangulationInternals
{
    public class TriangleTests
    {
        [Test]
        public void Test_Triangle_GetHashCode_SimilarTriangles()
        {
            //Arrange
            var triangle1 = new Triangle(11, 5, 7);
            var triangle2 = new Triangle(5, 7, 11);
            var triangle3 = new Triangle(11, 7, 5);

            //Act
            var hash1 = triangle1.GetHashCode();
            var hash2 = triangle2.GetHashCode();
            var hash3 = triangle3.GetHashCode();

            //Assert
            Assert.AreEqual(hash1, hash2);
            Assert.AreEqual(hash2, hash3);
        }

        [Test]
        public void Test_Triangle_GetHashCode_DifferentTriangles()
        {
            //Arrange
            var triangle1 = new Triangle(0, 1, 3);
            var triangle2 = new Triangle(0, 1, 2);

            //Act
            var hash1 = triangle1.GetHashCode();
            var hash2 = triangle2.GetHashCode();

            //Assert
            Assert.AreNotEqual(hash1, hash2);
        }
    }
}
