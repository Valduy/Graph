using System.ComponentModel;

namespace Graph.ViewModels
{
    public class GraphVertex : GraphElement
    {
        private double _diametr;
        private int _number;

        public int Number 
        { 
            get => _number;
            set 
            {
                if (_number == value) return;
                _number = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Number"));
            }
        }

        public double Diametr 
        {
            get => _diametr;
            set 
            {
                if (_diametr == value) return;
                _diametr = value;
                if (Height != value) Height = value;
                if (Width != value) Height = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Diametr"));
            }
        }

        public override double Width
        {
            get => base.Width;
            set
            {
                base.Width = value;
                if (Diametr != value) Diametr = value;
                if (Height != value) Height = value;
            }
        }

        public override double Height
        {
            get => base.Height;
            set
            {
                base.Height = value;
                if (Diametr != value) Diametr = value;
                if (Width != value) Width = value;
            }
        }

        public GraphVertex(int number)
        {
            Number = number;
            Diametr = 50;
        }
    }
}
