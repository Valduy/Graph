using Graph.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Graph.Serialization
{
    public class GraphSerializationInfo
    {
        public bool[] Matrix { get; set; }
        public Point[] VertexesCoordinates { get; set; }
        public int Size { get; set; }
    }
}
