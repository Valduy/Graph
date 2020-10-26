using System;
using System.ComponentModel;

namespace WebSimulation
{
    public class Datagram : INPCBase, ICloneable
    {
        private int _lifetime;
        private uint _previous;
        private uint _next;        
        private uint _passedNodes;
        private double _progress;
        private bool _isDelivered;

        public Datagram NextPacket { get; set; }
        public uint Number { get; }
        public uint Source { get; }
        public uint Target { get; }

        public uint Previous
        {
            get => _previous;
            set
            {
                if (_previous == value) return;
                _previous = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Previous"));
            }
        }

        public uint Next
        {
            get => _next;
            set
            {
                if (_next == value) return;
                _next = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Next"));
            }
        }

        public uint PassedNodes
        {
            get => _passedNodes;
            internal set
            {
                if (_passedNodes == value) return;
                _passedNodes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PassedNodes"));
            }
        }

        public int Lifetime
        {
            get => _lifetime;
            internal set
            {
                if (_lifetime == value) return;
                _lifetime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Lifetime"));
            }
        }

        public double Progress
        {
            get => _progress;
            set
            {
                if (_progress == value) return;
                _progress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Progress"));
            }
        }

        public bool IsDelivered
        {
            get => _isDelivered;
            set
            {
                if (_isDelivered == value) return;
                _isDelivered = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDelivered"));
            }
        }

        public Datagram(uint number, int lifetime, uint source, uint target)
        {
            Number = number;
            _lifetime = lifetime;
            Source = source;
            Target = target;
            _next = _previous = source;
        }

        public object Clone() 
            => new Datagram(Number, Lifetime, Source, Target)
            {
                Previous = Previous,
                Next = Next,
            };
    }
}
