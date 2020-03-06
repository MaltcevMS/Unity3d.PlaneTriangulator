using System.Collections.Generic;
using Unity3d.PlaneTriangulator.TriangulationDataTypes;

namespace Assets.Scripts.Utility.Triangulation.DataStructures
{
    public class TriangulationGraph
    {
        public IDictionary<Edge, TwoVertices> Items { get; }

        public TriangulationGraph()
        {
            Items = new Dictionary<Edge, TwoVertices>();
        }

        public TwoVertices this[Edge key]
        {
            get
            {
                TwoVertices twoVertices;

                if (!Items.TryGetValue(key, out twoVertices))
                {
                    twoVertices = new TwoVertices();
                    Items.Add(key, twoVertices);
                }

                return twoVertices;
            }
        }

        public void Remove(Edge key)
        {
            Items.Remove(key);
        }
    }
}
