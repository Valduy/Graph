using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WebSimulation.RoutingAlgorithms
{
    internal class RandomRoutingAlgorithm : RoutingAlgorithmBase
    {
        private Dictionary<Datagram, uint> _virtualChannelRouting = new Dictionary<Datagram, uint>();
        private Random _random = new Random();

        public bool BuildTable { get; set; }

        public override void Begin(Collection<Datagram> packets, IEnumerable<(uint A, uint B)> connections)
        {
            base.Begin(packets, connections);

            switch (Simulator.RoutingType)
            {
                case RoutingType.Datagram:
                    PrepareToDatagrammMethod(packets, connections);
                    break;
                case RoutingType.VirtualChannel:
                    PrepareToVirtualChannelMethod(packets, connections);
                    break;
            }
        }

        public override void Update(Collection<Datagram> packets, IEnumerable<(uint A, uint B)> connections, double dt)
        {
            base.Update(packets, connections, dt);

            for (int i = 0; i < Moving.Count; i++)
            {
                Moving[i].Progress += PacketSpeed * dt;

                if (Moving[i].Progress >= MaxPercent)
                {
                    if (Moving[i].Next == Moving[i].Target)
                    {
                        if (BuildTable) FillRoutingCell(Moving[i], (uint)Simulator.Lifetime - (uint)Moving[i].Lifetime + 1);
                        Moving[i].IsDelivered = true;
                        Moving[i].Progress = MaxPercent;
                        Moving.Remove(Moving[i]);
                        i--;
                        continue;
                    }

                    Moving[i].Lifetime--;

                    if (Moving[i].Lifetime <= 0)
                    {
                        if (BuildTable)
                        {
                            FillRoutingCell(Moving[i], (uint)Simulator.Lifetime - (uint)Moving[i].Lifetime);
                        }

                        packets.Remove(Moving[i]);
                    }
                    else
                    {
                        try
                        {
                            if (BuildTable)
                            {
                                FillRoutingCell(Moving[i], (uint)Simulator.Lifetime - (uint)Moving[i].Lifetime);
                            }

                            if (Simulator.RoutingType == RoutingType.VirtualChannel)
                            {
                                if (_virtualChannelRouting.ContainsKey(Moving[i]))
                                {
                                    Moving[i].Previous = Moving[i].Next;
                                    Moving[i].Next = _virtualChannelRouting[Moving[i]];
                                    _virtualChannelRouting.Remove(Moving[i]);
                                }
                                else
                                {
                                    RoutPacket(Moving[i], connections);
                                }                                

                                if (Moving[i].NextPacket != null)
                                {
                                    _virtualChannelRouting[Moving[i].NextPacket] = Moving[i].Next;
                                }
                            }
                            else
                            {
                                RoutPacket(Moving[i], connections);
                            }

                            Moving[i].Progress = LowPercent;
                            Waiting[(Moving[i].Previous, Moving[i].Next)].Enqueue(new RoutingOrder(Moving[i], Cooldown));
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            packets.Remove(Moving[i]);
                        }
                    }

                    Moving.Remove(Moving[i]);
                    i--;
                }
            }
        }

        private void PrepareToDatagrammMethod(Collection<Datagram> packets, IEnumerable<(uint A, uint B)> connections)
        {
            for (int i = 0; i < packets.Count; i++)
            {
                try
                {
                    var next = GetNext(packets[i], connections);
                    packets[i].Next = next;
                    Waiting[(packets[i].Previous, packets[i].Next)].Enqueue(new RoutingOrder(packets[i], Cooldown));
                }
                catch (ArgumentOutOfRangeException)
                {
                    packets.RemoveAt(i);
                    i--;
                }
            }
        }

        private void PrepareToVirtualChannelMethod(Collection<Datagram> packets, IEnumerable<(uint A, uint B)> connections)
        {
            for (int i = 0; i < packets.Count - 1; i++)
            {
                packets[i].NextPacket = packets[i + 1];
            }

            try
            {
                var next = GetNext(packets.First(), connections);

                foreach (var p in packets)
                {
                    p.Next = next;
                    Waiting[(p.Previous, p.Next)].Enqueue(new RoutingOrder(p, Cooldown));
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                packets.Clear();
                return;
            }
        }

        private void RoutPacket(Datagram packet, IEnumerable<(uint A, uint B)> connections)
        {
            var next = GetNext(packet, connections);
            packet.Previous = packet.Next;
            packet.Next = next;
        }

        private uint GetNext(Datagram packet, IEnumerable<(uint A, uint B)> connections)
        {
            var potencial = connections.Where(o => o.A == packet.Next && o.B != packet.Previous);
            return potencial.ElementAt(_random.Next(0, potencial.Count())).B;
        }

        private void FillRoutingCell(Datagram packet, uint passed)
        {
            var cell = Simulator.GetRoutingCell(packet.Previous, packet.Next);
            
            if (cell.Weight.HasValue)
            {
                cell.Weight = Math.Min(passed, cell.Weight.Value);
            }
            else
            {
                cell.Weight = passed;
            }
        }
    }
}
