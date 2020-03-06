# Unity3d.PlaneTriangulator
C# Plane triangulation library adopted for Unity3d mesh. Supports triangulation of a plane mesh with arbitrary holes. Accepts an array of vertices, returns an array of triangles as an array of integers.
Not really fast to use this algorithm in the Update or FixUpdate method, but could work for relatively simple tasks.

![Example of triangulation with holes](https://github.com/MaltcevMS/Unity3d.PlaneTriangulator/blob/master/examples/ex1.png)

Initial C++ implementation: https://github.com/Vemmy124/Delaunay-Triangulation-Algorithm
