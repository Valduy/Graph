using Graph.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Graph.Controls.Options
{
    public class EditOptionAlgorythm : IOptionAlgorythm
    {
        private GraphViewModel _graph;
        private GraphGrid _grid;

        public EditOptionAlgorythm(GraphGrid grid, GraphViewModel graph)
        {
            _grid = grid;
            _graph = graph;
        }


        public void OnMouseDown(MouseButtonEventArgs e) { }
        public void OnMouseLeave(MouseEventArgs e) { }
        public void OnMouseMove(MouseEventArgs e) { }

        public void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released
                && e.OriginalSource is FrameworkElement element
                && element.DataContext is GraphEdge edge)
            {
                _graph.EditEdgeCommand.Execute(edge);
            }
        }
    }
}
