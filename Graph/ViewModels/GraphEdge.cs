using Graph.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Graph.ViewModels
{
    public class GraphEdge : GraphEdgeBase
    {
        private static double ToDegree = (180 / Math.PI);
    
        private double _arrowToAX;
        private double _arrowToAY;
        private double _arrowToBX;
        private double _arrowToBY;
        private double _arrowAAngle; 
        private double _arrowBAngle;
        private bool _toA;
        private bool _toB;
        private bool _hideArrows;

        public GraphVertex A { get; }
        public GraphVertex B { get; }

        public double ArrowToAX 
        {
            get => _arrowToAX;
            set 
            {
                if (_arrowToAX == value) return;
                _arrowToAX = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArrowToAX"));
            }
        }

        public double ArrowToAY 
        {
            get => _arrowToAY;
            set
            {
                if (_arrowToAY == value) return;
                _arrowToAY = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArrowToAY"));
            }
        }

        public double ArrowToBX
        {
            get => _arrowToBX;
            set
            {
                if (_arrowToBX == value) return;
                _arrowToBX = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArrowToBX"));
            }
        }

        public double ArrowToBY
        {
            get => _arrowToBY;
            set
            {
                if (_arrowToBY == value) return;
                _arrowToBY = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArrowToBY"));
            }
        }

        public double ArrowAAngle
        {
            get => _arrowAAngle;
            set
            {
                if (_arrowAAngle == value) return;
                _arrowAAngle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArrowAAngle"));
            }
        }

        public double ArrowBAngle
        {
            get => _arrowBAngle;
            set
            {
                if (_arrowBAngle == value) return;
                _arrowBAngle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArrowBAngle"));
            }
        }

        public bool ToA 
        {
            get => _toA;
            set 
            {
                if (_toA == value) return;
                _toA = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToA"));
            }
        }

        public bool ToB
        {
            get => _toB;
            set
            {
                if (_toB == value) return;
                _toB = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToB"));
            }
        }

        public bool HideArrows 
        {
            get => _hideArrows;
            set 
            {
                if (_hideArrows == value) return;
                _hideArrows = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HideArrows"));
            }
        }

        public GraphEdge(GraphVertex a, GraphVertex b, bool toA = false, bool toB = false) 
        {            
            A = a;
            B = b;
            ToA = toA;
            ToB = toB;
            A.PropertyChanged += OnPositionChanged;
            B.PropertyChanged += OnPositionChanged;
            Update();
        }

        private void OnPositionChanged(object sender, PropertyChangedEventArgs e) 
        {
            if (e.PropertyName == "X" || e.PropertyName == "Y") 
            {
                Update();
            }
        }

        private void Update() 
        {
            UpdateArea();
            UpdatePath();
            UpdateArrow();                    
        }

        private void UpdateArea()
        {
            var aPoint = EdgePointHelper.GetPointToConnection(A, B);
            var bPoint = EdgePointHelper.GetPointToConnection(B, A);

            if (EdgePointHelper.PointInsideNode(A, bPoint) || EdgePointHelper.PointInsideNode(B, aPoint)) 
            {
                Area = new Rect(0, 0, 0, 0);
                HideArrows = true;
            }
            else 
            {
                Area = new Rect(aPoint, bPoint);
                HideArrows = false;
            }
        }

        private void UpdatePath() 
        {
            Points = new List<Point>
            {
                new Point( A.X  <  B.X ? 0 : Area.Width, A.Y  <  B.Y ? 0 : Area.Height ),
                new Point( A.X  >  B.X ? 0 : Area.Width, A.Y  >  B.Y ? 0 : Area.Height),
            };
        }

        private void UpdateArrow() 
        {
            if (A.X > B.X) 
            {
                ArrowToAX = Area.Width;
                ArrowToBX = 0;
            }
            else
            {
                ArrowToAX = 0;
                ArrowToBX = Area.Width;
            }

            if (A.Y > B.Y) 
            {
                ArrowToAY = Area.Height;
                ArrowToBY = 0;
            }
            else
            {
                ArrowToAY = 0;
                ArrowToBY = Area.Height;
            }

            if (A.X >= B.X && A.Y < B.Y)
            {
                ArrowBAngle = Math.Atan2(Area.Width, Area.Height) * ToDegree;
            }
            else if (A.X < B.X && A.Y < B.Y)
            {
                ArrowBAngle = Math.Atan2(-Area.Width, Area.Height) * ToDegree;
            }
            else if (A.X > B.X && A.Y >= B.Y)
            {
                ArrowBAngle = Math.Atan2(Area.Width, -Area.Height) * ToDegree;
            }
            else
            {
                ArrowBAngle = Math.Atan2(-Area.Width, -Area.Height) * ToDegree;
            }

            ArrowAAngle = ArrowBAngle + 180;
        }
    }
}
