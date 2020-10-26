using System.ComponentModel;

namespace Graph.ViewModels
{
    public abstract class GraphElement : INPCBase
    {
        private double _x;
        private double _y;
        private double _width = 40;
        private double _height = 40;
        private bool _highlighted;

        public double X
        {
            get => _x;
            set
            {
                if (_x == value) return;
                _x = value;
                OnPropertyChanged(new PropertyChangedEventArgs("X"));
            }
        }

        public double Y
        {
            get => _y;
            set
            {
                if (_y == value) return;
                _y = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Y"));
            }
        }

        public virtual double Width 
        { 
            get => _width;
            set
            {
                if (_width == value) return;
                _width = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Width"));
            }
        }

        public virtual double Height 
        { 
            get => _height; 
            set 
            {
                if (_height == value) return;
                _height = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Height"));
            }
        }

        public bool Highlighted
        {
            get => _highlighted;
            set
            {
                if (_highlighted == value) return;
                _highlighted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Highlighted"));
            }
        }
    }
}
