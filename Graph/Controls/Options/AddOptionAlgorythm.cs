using Graph.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Graph.Controls.Options
{
    public class AddOptionAlgorythm : IOptionAlgorythm
    {
        private GraphViewModel _graph;
        private GraphGrid _grid;

        public AddOptionAlgorythm(GraphGrid grid, GraphViewModel graph) 
        {
            _grid = grid;
            _graph = graph;
        }

        public void OnMouseDown(MouseButtonEventArgs e) { }
        public void OnMouseLeave(MouseEventArgs e) { }
        public void OnMouseMove(MouseEventArgs e) { }

        public void OnMouseUp(MouseButtonEventArgs e) 
        {
            if (e.LeftButton == MouseButtonState.Released) 
            {
                _graph.AddVertexCommand.Execute(e.GetPosition(_grid));
            }
        }
    }
}
