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
    public class DeleteOptionAlgorythm : IOptionAlgorythm
    {
        private GraphViewModel _graph;
        private GraphGrid _grid;

        public DeleteOptionAlgorythm(GraphGrid grid, GraphViewModel graph)
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
                && e.OriginalSource is FrameworkElement element) 
            { 
                if (element.DataContext is GraphEdge edge) 
                {
                    _graph.RemoveEdgeCommand.Execute(edge);
                }
                else if (element.DataContext is GraphVertex vertex)
                {
                    _graph.RemoveVertexCommand.Execute(vertex);
                }
            }               
        }
    }
}
