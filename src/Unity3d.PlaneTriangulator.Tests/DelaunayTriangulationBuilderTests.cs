using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Unity3d.PlaneTriangulator.Tests
{
    public class Tests
    {
        private const int PointsPerTriangle = 3;

        private void TestPointsCount(Vector2[] vertices, int expectedTrianglesCount)
        {
            //Arrange
            var expectedTrianglePointsCount = expectedTrianglesCount * PointsPerTriangle;
            var builder = new DelaunayTriangulationBuilder();

            //Act
            var actualTriangulation = builder.Build(vertices).ToList();

            //Assert
            Assert.AreEqual(expectedTrianglePointsCount, actualTriangulation.Count,
                $"Expected points count: {expectedTrianglePointsCount}, but was \n {string.Join("\n ", actualTriangulation)}");
        }

        [Test]
        public void Test_BuildOn4Vertices()
        {
            //Arrange
            const int expectedTrianglesCount = 2;
            var vertices = new[]
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 0),
                new Vector2(1, 1)
            };

            //Act
            TestPointsCount(vertices, expectedTrianglesCount);
        }

        [Test]
        public void Test_BuildOn5Vertices()
        {
            //Arrange
            const int expectedTrianglesCount = 4;
            var vertices = new[]
            {
                new Vector2(-0.2f, 0.2f),
                new Vector2(0.1f, 1),
                new Vector2(0.5f, 0.5f),
                new Vector2(1.7f, 0),
                new Vector2(1.7f, 1.7f)
            };

            //Act
            TestPointsCount(vertices, expectedTrianglesCount);
        }
    }
}