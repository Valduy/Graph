using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Graph.ViewModels
{
    public abstract class GraphEdgeBase : GraphElement
    {
        private List<Point> _points;
        private Rect _area;

        public List<Point> Points
        {
            get => _points;
            protected set
            {
                if (_points == value) return;
                _points = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Points"));
            }
        }

        public Rect Area
        {
            get => _area;
            protected set
            {
                if (_area == value) return;
                _area = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Area"));
            }
        }
    }
}
