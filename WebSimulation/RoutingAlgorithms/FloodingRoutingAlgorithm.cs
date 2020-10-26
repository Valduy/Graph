using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WebSimulation.RoutingAlgorithms
{
    internal class FloodingRoutingAlgorithm : RoutingAlgorithmBase
    {
        public override void Begin(Collection<Datagram> packets, IEnumerable<(uint A, uint B)> connections)
        {
            base.Begin(packets, connections);
            var dublicates = new List<Datagram>();

            while (packets.Count > 0)
            {
                var directions = GetAllDirections(packets.First(), connections);

                if (directions.Count() > 0)
                {
                    foreach (var d in directions)
                    {
                        var packet = (Datagram)packets.First().Clone();
                        packet.Next = d;
                        dublicates.Add(packet);
                    }
                }

                packets.Remove(packets.First());
            }

            UpdateDublicates(packets, dublicates);
        }

        public override void Update(Collection<Datagram> packets, IEnumerable<(uint A, uint B)> connections, double dt)
        {
            base.Update(packets, connections, dt);
            var dublicates = new List<Datagram>();

            for (int i = 0; i < Moving.Count; i++)
            {
                Moving[i].Progress += PacketSpeed * dt;

                if (Moving[i].Progress >= MaxPercent)
                {
                    if (Moving[i].Next == Moving[i].Target)
                    {
                        Moving[i].IsDelivered = true;
                        Moving[i].Progress = MaxPercent;
                        Moving.Remove(Moving[i]);
                        i--;
                        continue;
                    }

                    Moving[i].Lifetime--;

                    if (!(Moving[i].Lifetime <= 0))
                    {
                        var directions = GetAllDirections(Moving[i], connections);

                        if (directions.Count() > 0)
                        {
                            foreach (var d in directions)
                            {
                                var packet = (Datagram)Moving[i].Clone();
                                packet.Previous = packet.Next;
                                packet.Next = d;
                                dublicates.Add(packet);
                            }
                        }
                    }

                    packets.Remove(Moving[i]);
                    Moving.Remove(Moving[i]);
                    i--;
                }
            }

            UpdateDublicates(packets, dublicates);
        }

        private IEnumerable<uint> GetAllDirections(Datagram packet, IEnumerable<(uint A, uint B)> connections)
            => connections.Where(o => o.A == packet.Next && o.B != packet.Previous).Select(o => o.B);

        private void UpdateDublicates(Collection<Datagram> packets, IEnumerable<Datagram> dublicates)
        {
            foreach (var p in dublicates)
            {
                packets.Add(p);
                Waiting[(p.Previous, p.Next)].Enqueue(new RoutingOrder(p, Cooldown));
            }
        }
    }
}
