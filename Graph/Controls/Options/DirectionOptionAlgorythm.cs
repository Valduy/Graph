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
    class DirectionOptionAlgorythm : IOptionAlgorythm
    {
        private GraphGrid _grid;
        private GraphHalfEdge _halfEdge;
        private GraphVertex _source;
        private GraphViewModel _graph;

        public DirectionOptionAlgorythm(GraphGrid grid, GraphViewModel graph)
        {
            _graph = graph;
            _grid = grid;
        }

        public void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed
                && e.OriginalSource is FrameworkElement element
                && element.DataContext is GraphVertex vertex)
            {
                _source = vertex;
                _halfEdge = new GraphHalfEdge(vertex, e.GetPosition(_grid));
                _halfEdge.IsPointer = true;
                _graph.AddHalfEdgeCommand.Execute(_halfEdge);
                _source = vertex;
            }
        }

        public void OnMouseLeave(MouseEventArgs e)
        {
            if (_halfEdge != null)
            {
                Reset();
            }
        }

        public void OnMouseMove(MouseEventArgs e)
        {
            if (_halfEdge != null)
            {
                _halfEdge.DragPoint = e.GetPosition(_grid);
            }
        }

        public void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released
                && _halfEdge != null)
            {
                if (_source != null
                    && e.OriginalSource is FrameworkElement element
                    && element.DataContext is GraphVertex vertex)
                {
                    _graph.SetPathCommand.Execute(new object[] { (uint)_source.Number, (uint)vertex.Number });
                }

                Reset();
            }
        }

        private void Reset()
        {
            _graph.RemoveHalfEdgeCommand.Execute(_halfEdge);
            _halfEdge = null;
            _source = null;
        }
    }
}
