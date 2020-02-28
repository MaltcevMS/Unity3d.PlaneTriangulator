using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Unity3d.PlaneTriangulator.Tests
{
    public class Tests
    {
        private const int PointsPerTriangle = 3;

        [Test]
        public void Test_BuildOn4Vertices()
        {
            //Arrange
            const int expectedTrianglesCount = 2;
            const int expectedTrianglePointsCount = expectedTrianglesCount * PointsPerTriangle;

            var vertices = new[]
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 0),
                new Vector2(1, 1)
            };
            var builder = new DelaunayTriangulationBuilder();

            //Act
            var actualTriangulation = builder.Build(vertices).ToList();

            //Assert
            Assert.AreEqual(expectedTrianglePointsCount, actualTriangulation.Count, 
                $"Expected points count: {expectedTrianglePointsCount}, but was \n {string.Join("\n ", actualTriangulation)}");
        }
    }
}