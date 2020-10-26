using Graph.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Graph.ViewModels
{
    public class GraphHalfEdge : GraphEdgeBase
    {
        private bool _isPointer;
        private Point _dragPoint;
        private Point _sourcePoint;
        private double _destanationX;
        private double _destanationY;

        public bool IsPointer 
        {
            get => _isPointer;
            set 
            {
                if (_isPointer == value) return;
                _isPointer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPointer"));
            }
        }

        public Point DragPoint 
        {
            get => _dragPoint;
            set 
            {
                if (_dragPoint == value) return;
                _dragPoint = value;
                Update();
                OnPropertyChanged(new PropertyChangedEventArgs("DragPoint"));
            }
        }

        public double DestanationX 
        {
            get => _destanationX;
            set 
            {
                if (_destanationX == value) return;
                _destanationX = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DestanationX"));
            }
        }

        public double DestanationY
        {
            get => _destanationY;
            set
            {
                if (_destanationY == value) return;
                _destanationY = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DestanationY"));
            }
        }

        public GraphVertex Source { get; }

        public GraphHalfEdge(GraphVertex source, Point point)
        {
            Source = source;
            _dragPoint = point;
            _destanationX = point.X;
            _destanationY = point.Y;
            Update();
        }

        private void Update() 
        {
            UpdateArea();
            UpdatePath();
            UpdateDestanation();
        }

        private void UpdateArea() 
        {
            if (EdgePointHelper.PointInsideNode(Source, _dragPoint))
            {
                Area = new Rect(0, 0, 0, 0);
            }
            else 
            {
                _sourcePoint = EdgePointHelper.GetPointToConnection(Source, _dragPoint);
                Area = new Rect(_sourcePoint, _dragPoint);
            }            
        }

        private void UpdatePath()
        {
            Points = new List<Point>
            {
                new Point(_sourcePoint.X  <  _dragPoint.X ? 0 : Area.Width, _sourcePoint.Y  <  _dragPoint.Y ? 0 : Area.Height),
                new Point(_sourcePoint.X  >  _dragPoint.X ? 0 : Area.Width, _sourcePoint.Y  >  _dragPoint.Y ? 0 : Area.Height),
            };
        }

        private void UpdateDestanation()
        {
            DestanationX = (Source.X > DragPoint.X) ? 0 : Area.Width;
            DestanationY = (Source.Y > DragPoint.Y) ? 0 : Area.Height;
        }
    }
}
