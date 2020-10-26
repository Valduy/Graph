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
    public class MoveOptionAlgorythm : IOptionAlgorythm
    {
        //private GraphCanvas _canvas;
        private GraphGrid _grid;
        private GraphVertex _dragged;

        //public MoveOptionAlgorythm(GraphCanvas canvas) => _canvas = canvas;
        public MoveOptionAlgorythm(GraphGrid grid) => _grid = grid;

        public void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed
                && e.OriginalSource is FrameworkElement element
                && element.DataContext is GraphVertex vertex) 
            {
                _dragged = vertex;
            }
        }

        public void OnMouseLeave(MouseEventArgs e) => _dragged = null;

        public void OnMouseMove(MouseEventArgs e)
        {
            if (_dragged != null) 
            {
                var position = e.GetPosition(_grid);
                _dragged.X = position.X - _dragged.Width / 2;
                _dragged.Y = position.Y - _dragged.Height / 2;
            }
        }

        public void OnMouseUp(MouseButtonEventArgs e) => _dragged = null;
    }
}
