using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using WebSimulation.RoutingAlgorithms;

namespace WebSimulation
{
    [Flags]
    public enum RoutingType
    {
        Datagram,
        VirtualChannel,
    }

    public class WebSimulator : INPCBase
    {
        public const uint FramesPerSecond = 60;
        public const double DT = 1000d / FramesPerSecond;
        public const double PacketSpeed = 1d / 1000;
        public const int Cooldown = 600;

        private Thread _cicle;

        private uint _nodeCount;
        private uint _sourceNode;
        private uint _targetNode;
        private int _lifetime = 5;
        private bool _isWork;

        private RoutingType _routingType = RoutingType.Datagram;

        private ObservableCollection<(uint A, uint B)> _connections = new ObservableCollection<(uint A, uint B)>();
        private ObservableCollection<Datagram> _packets = new ObservableCollection<Datagram>();
        private ObservableCollection<RoutingCell> _routingTable = new ObservableCollection<RoutingCell>();        

        private ReadOnlyObservableCollection<(uint A, uint B)> Connections;
        public ReadOnlyObservableCollection<Datagram> Packets;
        public ReadOnlyObservableCollection<RoutingCell> RoutingTable;

        public uint NodeCount
        {
            get => _nodeCount;
            set
            {
                if (_nodeCount == value) return;                
                UpdateRoutingTable(value);
                ResetConnections();
                _nodeCount = value;
                SourceNode = TargetNode = 0;                
                OnPropertyChanged(new PropertyChangedEventArgs("NodeCount"));
            }
        }

        public uint SourceNode
        {
            get => _sourceNode;
            set
            {
                if (_sourceNode == value) return;
                if (_sourceNode >= _nodeCount) throw new ArgumentException();
                _sourceNode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SourceNode"));
            }
        }

        public uint TargetNode
        {
            get => _targetNode;
            set
            {
                if (_targetNode == value) return;
                if (_targetNode >= _nodeCount) throw new ArgumentException();
                _targetNode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TargetNode"));
            }
        }

        public int Lifetime
        {
            get => _lifetime;
            set
            {
                if (_lifetime == value) return;
                _lifetime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Lifetime"));
            }
        }

        public RoutingType RoutingType
        {
            get => _routingType;
            set
            {
                if (_routingType == value) return;
                _routingType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RoutingType"));
            }
        }

        public bool IsWork
        {
            get => _isWork;
            private set
            {
                if (_isWork == value) return;
                _isWork = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWork"));
            }
        }

        public WebSimulator()
        { 
            Connections = new ReadOnlyObservableCollection<(uint A, uint B)>(_connections);
            Packets = new ReadOnlyObservableCollection<Datagram>(_packets);
            RoutingTable = new ReadOnlyObservableCollection<RoutingCell>(_routingTable);
        }

        public void AddConnection(uint a, uint b) => AddConnection((a, b));

        public void AddConnection((uint A, uint B) connection)
        {
            if (connection.A >= _nodeCount || connection.B >= _nodeCount)
            {
                throw new ArgumentException();
            }

            if (!_connections.Contains(connection))
            {
                _connections.Add(connection);
                ResetRoutingCells();
            }
        }

        public void ResetConnections()
        {
            _connections.Clear();
        }

        public void RandomRouting(uint packetCount)
        {
            UpdatePackets(packetCount);
            Cicle(new RandomRoutingAlgorithm 
            { 
                PacketSpeed = PacketSpeed, 
                Cooldown = Cooldown,
            });
        }

        public void FloodingRouting(uint packetCount)
        {
            UpdatePackets(packetCount);
            Cicle(new FloodingRoutingAlgorithm 
            { 
                PacketSpeed = PacketSpeed, 
                Cooldown = Cooldown,
            });
        }

        public void ExpirienceRouting(uint packetCount)
        {
            UpdatePackets(packetCount);
            Cicle(new RandomRoutingAlgorithm
            {
                PacketSpeed = PacketSpeed,
                Cooldown = Cooldown,
                BuildTable = true,
            });
        }

        public void ResetRoutingCells()
        {
            foreach (var c in _routingTable)
            {
                c.Weight = null;
            }
        }

        public void Cancel()
        {
            foreach (var  p in _packets)
            {
                p.IsDelivered = true;
            }

            _packets.Clear();
        }

        internal RoutingCell GetRoutingCell(uint a, uint b) => _routingTable.First(o => o.A == a && o.B == b);

        private void UpdateRoutingTable(uint newCount)
        {
            ResetRoutingCells();

            if (_nodeCount < newCount)
            {
                for (uint i = 0; i < _nodeCount; i++)
                {
                    for (uint j = _nodeCount; j < newCount; j++)
                    {
                        _routingTable.Add(new RoutingCell(i, j));
                    }
                }

                for (uint i = _nodeCount; i < newCount; i++)
                {
                    for (uint j = 0; j < newCount; j++)
                    {
                        _routingTable.Add(new RoutingCell(i, j));
                    }
                }
            }
            else
            {
                var excluded = _routingTable.Where(o => o.A >= newCount || o.B >= newCount).ToList();

                foreach (var c in excluded)
                {
                    _routingTable.Remove(c);
                }
            }
        }

        private void UpdatePackets(uint packetCount)
        {
            _packets.Clear();

            for (uint i = 0; i < packetCount; i++)
            {
                _packets.Add(new Datagram(i, Lifetime, SourceNode, TargetNode));
            }
        }

        private void Cicle(RoutingAlgorithmBase algorithm)
        {
            _cicle = new Thread(() =>
            {
                IsWork = true;
                double accumulator = 0;
                algorithm.Register(this);
                algorithm.Begin(_packets, _connections);
                var timer = new Stopwatch();
                timer.Start();

                while (_packets.Any(o => !o.IsDelivered))
                {
                    accumulator += timer.ElapsedMilliseconds;
                    
                    if (accumulator >= DT)
                    {
                        timer.Restart();
                    }

                    if (accumulator > 3 * DT)
                    {
                        accumulator = 3 * DT;
                    }

                    while (accumulator > DT)
                    {
                        accumulator -= DT;
                        algorithm.Update(_packets, _connections, DT);
                    }                    
                }

                IsWork = false;
            });

            _cicle.Start();
        }
    }
}
