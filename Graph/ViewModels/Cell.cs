using System;
using System.ComponentModel;

namespace Graph.ViewModels
{
    public class Cell : INPCBase
    {
        private bool _value;

        public bool Value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                _value = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Value"));                
            }
        }
        
        public int Column { get; }        
        public int Row { get; }
        public bool IsEnabled { get; }

        public Cell(int row, int column) 
        {
            Column = Math.Max(0, column);
            Row = Math.Max(0, row);
            IsEnabled = !(Column == Row);
        }
    }
}
