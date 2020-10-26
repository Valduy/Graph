using System.ComponentModel;

namespace WebSimulation
{
    public class RoutingCell : INPCBase
    {
        private uint? _weight;

        public uint A { get; }
        public uint B { get; }

        public uint? Weight
        {
            get => _weight;
            set
            {
                if (_weight == value && A != B) return;
                _weight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Weight"));
            }
        }

        public RoutingCell(uint a, uint b)
        {
            A = a;
            B = b;
        }
    }
}
