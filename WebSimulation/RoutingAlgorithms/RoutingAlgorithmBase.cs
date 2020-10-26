using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WebSimulation.RoutingAlgorithms
{
    internal abstract class RoutingAlgorithmBase
    {
        internal class RoutingOrder
        {
            public Datagram Packet { get; }
            public int Cooldown { get; set; }

            public RoutingOrder(Datagram packet, int cooldown)
            {
                Packet = packet;
                Cooldown = cooldown;
            }
        }

        internal const uint LowPercent = 0;
        internal const uint MaxPercent = 100;

        public double PacketSpeed { get; set; }
        public int Cooldown { get; set; }

        internal WebSimulator Simulator { get; private set; }
        internal Dictionary<(uint A, uint B), Queue<RoutingOrder>> Waiting { get; } = new Dictionary<(uint A, uint B), Queue<RoutingOrder>>();
        internal List<Datagram> Moving { get; } = new List<Datagram>();

        public virtual void Register(WebSimulator simulator) => Simulator = simulator;

        public virtual void Begin(Collection<Datagram> packets, IEnumerable<(uint A, uint B)> connections)
        {
            if (Simulator.Lifetime <= 0)
            {
                packets.Clear();
                return;
            }

            Waiting.Clear();
            Moving.Clear();
            InitializeQueues(connections);            
        }

        public virtual void Update(Collection<Datagram> packets, IEnumerable<(uint A, uint B)> connections, double dt) 
            => PushQueues();

        private void InitializeQueues(IEnumerable<(uint A, uint B)> connections)
        {
            foreach (var c in connections)
            {
                Waiting[c] = new Queue<RoutingOrder>();
            }
        }

        private void PushQueues()
        {
            foreach (var q in Waiting)
            {
                if (!q.Value.Any()) continue;
                var order = q.Value.Peek();
                order.Cooldown--;

                if (order.Cooldown <= 0)
                {
                    Moving.Add(order.Packet);
                    q.Value.Dequeue();
                }
            }
        }
    }
}
