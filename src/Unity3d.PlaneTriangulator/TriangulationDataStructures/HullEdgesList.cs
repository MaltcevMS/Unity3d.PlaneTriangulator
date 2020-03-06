using System.Collections.Generic;
using Unity3d.PlaneTriangulator.TriangulationDataTypes;

namespace Assets.Scripts.Utility.Triangulation.DataStructures
{
    public class HullEdgesList
    {
        private readonly List<HullEdge> _edges;

        public HullEdgesList(int capacity)
        {
            _edges = new List<HullEdge>(capacity);
        }

        public HullEdge this[int index]
        {
            get
            {
                if (_edges.Count <= index)
                {
                    _edges.Add(new HullEdge(-1, -1));
                }
                return _edges[index];
            }
        }

        public void Add(HullEdge edge)
        {
            _edges.Add(edge);
        }
    }
}
