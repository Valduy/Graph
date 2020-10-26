using Graph.Helpers;
using System.ComponentModel;
using WebSimulation;

namespace Graph.ViewModels
{
    public class PacketWrapper : GraphElement
    {
        private GraphViewModel _vm;
        private GraphVertex _a;
        private GraphVertex _b;

        public Datagram Packet { get; }

        public PacketWrapper(Datagram packet, GraphViewModel vm)
        {
            Packet = packet;
            packet.PropertyChanged += OnPacketPropertyChanged;
            _vm = vm;
            _a = _vm.GetVertex((int)Packet.Previous);
            _b = _vm.GetVertex((int)Packet.Next);
            _a.PropertyChanged += OnPreviousChanged;
            _b.PropertyChanged += OnNextChanged;
            var from = EdgePointHelper.FindCenterOfNode(_a);
            X = from.X;
            Y = from.Y;
        }

        private void OnPacketPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Next")
            {
                _b.PropertyChanged -= OnNextChanged;
                _b = _vm.GetVertex((int)Packet.Next);
                _b.PropertyChanged += OnNextChanged;
            }
            if (e.PropertyName == "Previous")
            {
                _a.PropertyChanged -= OnPreviousChanged;
                _a = _vm.GetVertex((int)Packet.Previous);
                _a.PropertyChanged += OnPreviousChanged;
            }
            if (e.PropertyName == "Progress")
            {
                var from = EdgePointHelper.FindCenterOfNode(_a);
                var to = EdgePointHelper.FindCenterOfNode(_b);
                var vector = EdgePointHelper.FindVector(from, to);
                X = from.X + vector.X * Packet.Progress / 100;
                Y = from.Y + vector.Y * Packet.Progress / 100;
            }
        }

        private void OnPreviousChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == "X" || e.PropertyName == "Y") && Packet.Progress == 0)
            {
                var position = EdgePointHelper.FindCenterOfNode(_a);
                X = position.X;
                Y = position.Y;
            }
        }

        private void OnNextChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == "X" || e.PropertyName == "Y") && Packet.Progress == 100)
            {
                var position = EdgePointHelper.FindCenterOfNode(_b);
                X = position.X;
                Y = position.Y;
            }
        }
    }
}
