using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph.ViewModels
{
    class EdgeInfo : INPCBase
    {
        private bool _toA;
        private bool _toB;
        private int _vertexANumber;
        private int _vertexBNumber;

        public bool ToA 
        {
            get => _toA;
            set 
            {
                if (_toA == value) return;
                _toA = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WeightToA"));
            }
        } 

        public bool ToB 
        {
            get => _toB;
            set
            {
                if (_toB == value) return;
                _toB = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WeightToB"));
            }
        }

        public int VertexANumber 
        {
            get => _vertexANumber;
            set 
            {
                if (_vertexANumber == value) return;
                _vertexANumber = Math.Max(0, value);
                OnPropertyChanged(new PropertyChangedEventArgs("VertexANumber"));
            }
        }

        public int VertexBNumber
        {
            get => _vertexBNumber;
            set 
            {
                if (_vertexBNumber == value) return;
                _vertexBNumber = Math.Max(0, value);
                OnPropertyChanged(new PropertyChangedEventArgs("VertexBNumber"));
            }
        }

        public EdgeInfo(int a, int b, bool toA, bool toB) 
        {
            _vertexANumber = a;
            _vertexBNumber = b;
            _toA = toA;
            _toB = toB;
        }
    }
}
