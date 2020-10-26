using Graph.Commands;
using Graph.Serialization;
using Graph.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Xaml;
using WebSimulation;

namespace Graph.ViewModels
{
    [Flags]
    public enum RoutingAlgorithm
    {
        Random,
        Flooding,
        Expirience,
    }

    public class GraphViewModel : INPCBase
    {
        public const int MaxSize = 10;

        private int _size;
        private bool _canIncrement = true;
        private bool _canDecrement = true;
        private RoutingAlgorithm _routingAlgorithm;
        private uint _packetCount = 1;
        
        private ObservableCollection<Cell> _cells = new ObservableCollection<Cell>();
        private ObservableCollection<GraphElement> _elements = new ObservableCollection<GraphElement>();
        private List<GraphVertex> _vertexes = new List<GraphVertex>();
        private List<GraphEdge> _edges = new List<GraphEdge>();
        private Cell[,] _matrix = new Cell[0, 0];

        private WebSimulator _webSimulator = new WebSimulator();
        private DialogSirvice _dialogSirvice = new DialogSirvice();
        private MessageBoxService _messageBoxService = new MessageBoxService();

        public ReadOnlyObservableCollection<Cell> Cells { get; }
        public ReadOnlyObservableCollection<GraphElement> Elements { get; }

        public WebSimulator WebSimulator => _webSimulator;

        public int Size
        {
            get => _size;
            set
            {
                value = Math.Max(0, value);
                if (_size == value) return;
                Update(value);
                _size = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Size"));               
            }
        }

        public ReadOnlyObservableCollection<RoutingCell> RoutingTable => _webSimulator.RoutingTable;

        public uint SourceNode
        {
            get => _webSimulator.SourceNode;
            set => _webSimulator.SourceNode = value;
        }

        public uint TargetNode
        {
            get => _webSimulator.TargetNode;
            set => _webSimulator.TargetNode = value;
        }

        public int Lifetime
        {
            get => _webSimulator.Lifetime;
            set => _webSimulator.Lifetime = value;
        }

        public RoutingType RoutingType
        {
            get => _webSimulator.RoutingType;
            set => _webSimulator.RoutingType = value;
        }

        public bool IsWork => _webSimulator.IsWork;
        
        public bool CanIncrement 
        {
            get => _canIncrement;
            private set 
            {
                if (_canIncrement == value) return;
                _canIncrement = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CanIncrement"));
            }
        }

        public bool CanDecrement 
        {
            get => _canDecrement;
            private set 
            {
                if (_canDecrement == value) return;
                _canDecrement = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CanDecrement"));
            }
        }

        public RoutingAlgorithm RoutingAlgorithm
        {
            get => _routingAlgorithm;
            set
            {
                if (_routingAlgorithm == value) return;
                _routingAlgorithm = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RoutingAlgorithm"));
            }
        }

        public uint PacketCount
        {
            get => _packetCount;
            set
            {
                if (_packetCount == value) return;
                _packetCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PacketCount"));
            }
        }

        public SimpleCommand IncrementSizeCommand { get; }
        public SimpleCommand DecrementSizeCommand { get; }
        public SimpleCommand AddVertexCommand { get; }
        public SimpleCommand RemoveVertexCommand { get; }
        public SimpleCommand AddEdgeCommand { get; }
        public SimpleCommand RemoveEdgeCommand { get; }
        public SimpleCommand AddHalfEdgeCommand { get; }
        public SimpleCommand RemoveHalfEdgeCommand { get; }
        public SimpleCommand EditEdgeCommand { get; }
        public SimpleCommand SaveGraphCommand { get; }
        public SimpleCommand LoadGraphCommand { get; }
        public SimpleCommand RoutingCommand { get; } 
        public SimpleCommand SetPathCommand { get; }
        public SimpleCommand ResetCommand { get; }
        public SimpleCommand ClearCommand { get; }
        public SimpleCommand ChangeAlgorithmCommand { get; }
        public SimpleCommand CancelCommand { get; }

        public GraphViewModel()
        {
            Cells = new ReadOnlyObservableCollection<Cell>(_cells);
            Elements = new ReadOnlyObservableCollection<GraphElement>(_elements);

            IncrementSizeCommand = new SimpleCommand(OnIncrementSizeCommand);
            DecrementSizeCommand = new SimpleCommand(OnDecrementSizeCommand);
            AddVertexCommand = new SimpleCommand(OnAddVertexCommand);
            RemoveVertexCommand = new SimpleCommand(OnRemoveVertexCommand);
            AddEdgeCommand = new SimpleCommand(OnAddEdgeCommand);
            RemoveEdgeCommand = new SimpleCommand(OnRemoveEdgeCommand);
            AddHalfEdgeCommand = new SimpleCommand(OnAddHalfEdgeCommand);
            RemoveHalfEdgeCommand = new SimpleCommand(OnRemoveHalfEdgeCommand);
            EditEdgeCommand = new SimpleCommand(OnEditEdgeCommand);
            SaveGraphCommand = new SimpleCommand(OnSaveGraphCommand);
            LoadGraphCommand = new SimpleCommand(OnLoadGraphCommand);
            RoutingCommand = new SimpleCommand(OnRoutingCommand);
            SetPathCommand = new SimpleCommand(OnSetPathCommand);
            ResetCommand = new SimpleCommand(OnResetCommand);
            ClearCommand = new SimpleCommand(OnClearCommand);
            ChangeAlgorithmCommand = new SimpleCommand(OnChangeAlgorithmCommand);
            CancelCommand = new SimpleCommand(OnCancelCommand);
            
            Update(_size);
            ((INotifyCollectionChanged)_webSimulator.Packets).CollectionChanged += OnPacketsChanged;
            _webSimulator.PropertyChanged += (o, e) => OnPropertyChanged(e); 
        }

        public bool this[int row, int column]
        {
            get => _matrix[row, column].Value;
            private set => _matrix[row, column].Value = value;
        }

        internal GraphEdge TryGetEdge(int a, int b)
            => _edges.FirstOrDefault(o => (o.A.Number == a && o.B.Number == b || o.A.Number == b && o.B.Number == a));

        internal GraphVertex GetVertex(int number) => _vertexes.First(o => o.Number == number);

        private void Update(int newSize)
        {
            CanDecrement = newSize > 0;
            CanIncrement = newSize < MaxSize;
            ResetRouting();
            UpdateMatrix(newSize);
            UpdateCells(newSize);
            UpdateGraphElements(newSize);
            _webSimulator.NodeCount = (uint)newSize;
            UpdateSimulator();
        }

        private void UpdateMatrix(int newSize) 
        {
            var newMatrix = new Cell[newSize, newSize];
            int minSize = Math.Min(_size, newSize);

            for (int i = 0; i < minSize; i++)
            {
                for (int j = 0; j < minSize; j++)
                {
                    newMatrix[i, j] = _matrix[i, j];
                }
            }

            for (int i = 0; i < _size; i++)
            {
                for (int j = newSize; j < _size; j++)
                {
                    _matrix[i, j].PropertyChanged -= OnCellChanged;
                }
            }

            for (int i = newSize; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    _matrix[i, j].PropertyChanged -= OnCellChanged;
                }
            }

            _matrix = newMatrix;
        }

        private void UpdateCells(int newSize) 
        { 
            if (newSize > _size) 
            {
                for (int i = 0; i < _size; i++)
                {
                    for (int j = _size; j < newSize; j++)
                    {
                        _matrix[i, j] = new Cell(i, j);
                        _cells.Add(_matrix[i, j]);
                        _matrix[i, j].PropertyChanged += OnCellChanged;
                    }
                }

                for (int i = _size; i < newSize; i++)
                {
                    for (int j = 0; j < newSize; j++)
                    {
                        _matrix[i, j] = new Cell(i, j);
                        _cells.Add(_matrix[i, j]);
                        _matrix[i, j].PropertyChanged += OnCellChanged;
                    }
                }
            }
            else 
            {
                var excessCells = _cells.Where(o => o.Column >= newSize || o.Row >= newSize);

                foreach (var c in excessCells.ToList()) 
                {
                    _cells.Remove(c);
                }
            }
        }

        private void UpdateGraphElements(int newSize) 
        {
            if (newSize > _size) 
            { 
                for (int i = _size; i < newSize; i++) 
                {
                    AddVertex(new GraphVertex(i));
                }
            }
            else 
            {
                var excessVertexes = _vertexes.Where(o => o.Number >= newSize).ToList();

                foreach (GraphVertex v in excessVertexes) 
                {
                    RemoveVertex(v);
                }

                var excessEdges = _edges.Where(o => (excessVertexes.Contains(o.A) || excessVertexes.Contains(o.B))).ToList();

                foreach (GraphEdge e in excessEdges)
                {
                    RemoveEdge(e);
                }
            }
        }

        private void UpdateSimulator()
        {
            //_webSimulator.ResetConnections();

            foreach (var c in _cells)
            {
                if (c.Value)
                {
                    _webSimulator.AddConnection((uint)c.Row, (uint)c.Column);
                }
            }
        }

        private void OnCellChanged(object sender, PropertyChangedEventArgs e) 
        { 
            if (e.PropertyName == "Value") 
            {
                var cell = (Cell)sender;
                var edge = TryGetEdge(cell.Column, cell.Row);
                      
                if (cell.Value) 
                { 
                    if (edge == null) 
                    {
                        edge = new GraphEdge(GetVertex(cell.Row), GetVertex(cell.Column), false, true);
                        AddEdge(edge);
                    }
                    else 
                    {
                        if (_matrix[cell.Column, cell.Row].Value)
                        {
                            edge.ToA = edge.ToB = true;
                        }
                    }
                }
                else if (edge != null)
                { 
                    if (_matrix[cell.Column, cell.Row].Value) 
                    {
                        if (cell.Column == edge.A.Number)
                        {
                            edge.ToA = false;
                            edge.ToB = true;                            
                        }
                        else
                        {
                            edge.ToA = true;
                            edge.ToB = false;
                        }
                    }
                    else 
                    {
                        RemoveEdge(edge);
                    }
                }

                edge.ToA = _matrix[edge.B.Number, edge.A.Number].Value;
                edge.ToB = _matrix[edge.A.Number, edge.B.Number].Value;

                ResetRouting();
                UpdateSimulator();
            }
        }

        private void RemoveVertex(int number) 
        {
            if (number < 0 && number >= Size) throw new ArgumentException();

            // Сжимаем матрицу.
            for (int i = 0; i < Size; i++)
            {
                for (int j = number; j < Size - 1; j++)
                {
                    _matrix[i, j].Value = _matrix[i, j + 1].Value;
                }
            }
                        
            for (int i = number; i < Size - 1; i++)
            {
                for (int j = 0; j < Size - 1; j++)
                {
                    _matrix[i, j].Value = _matrix[i + 1, j].Value;
                }
            }

            // Теперь необходимо удалить последнюю вершину. Отсоединяем ее ото всех.
            for (int i = 0; i < Size; i++)
            {
                _matrix[i, Size - 1].Value = false;
            }

            for (int j = 0; j < Size; j++)
            {
                _matrix[Size - 1, j].Value = false;
            }

            // Корректируем координаты
            var vertexes = _elements.OfType<GraphVertex>()
                                    .Where(o => o.Number >= number)
                                    .OrderBy(o => o.Number)
                                    .ToArray();

            for (int i = 0; i < vertexes.Length - 1; i++)
            {
                vertexes[i].X = vertexes[i + 1].X;
                vertexes[i].Y = vertexes[i + 1].Y;
            }

            Size--;
        }

        private void AddVertex(GraphVertex vertex) 
        {
            _elements.Add(vertex);
            _vertexes.Add(vertex);
        }

        private void AddEdge(GraphEdge edge) 
        {
            _elements.Add(edge);
            _edges.Add(edge);
        }

        private void RemoveVertex(GraphVertex vertex) 
        {
            _elements.Remove(vertex);
            _vertexes.Remove(vertex);
        }

        private void RemoveEdge(GraphEdge edge) 
        {
            _elements.Remove(edge);
            _edges.Remove(edge);
        }

        private void ResetRouting()
        {
            SourceNode = TargetNode = 0;
            _webSimulator.ResetRoutingCells();
        }

        private void OnIncrementSizeCommand(object o) 
        {
            if (Size < MaxSize)
            {
                Size++;

                if (Size == MaxSize) 
                {
                    CanIncrement = false;
                }

                CanDecrement = true;                
            }
        }

        private void OnDecrementSizeCommand(object o) 
        { 
            if (Size > 0) 
            {
                Size--;

                if (Size == 0) 
                {
                    CanDecrement = false;
                }

                CanIncrement = true;
            }
        }

        private void OnAddVertexCommand(object o) 
        {
            if (Size >= MaxSize) return;
            var position = (Point)o;
            OnIncrementSizeCommand(null);
            var addedVertex = GetVertex(Size - 1);
            addedVertex.X = position.X - addedVertex.Width / 2;
            addedVertex.Y = position.Y - addedVertex.Height / 2;
        }

        private void OnRemoveVertexCommand(object o) 
        {
            RemoveVertex(((GraphVertex)o).Number);
        }

        private void OnAddEdgeCommand(object o) 
        {
            var edge = (GraphEdge)o;
            if (edge.A == edge.B) return;
            _matrix[edge.A.Number, edge.B.Number].Value = true;
            _matrix[edge.B.Number, edge.A.Number].Value = true;
        }

        private void OnRemoveEdgeCommand(object o) 
        { 
            var edge = (GraphEdge)o;
            _matrix[edge.A.Number, edge.B.Number].Value = false;
            _matrix[edge.B.Number, edge.A.Number].Value = false;
        }

        private void OnAddHalfEdgeCommand(object o) => _elements.Add((GraphHalfEdge)o);
        private void OnRemoveHalfEdgeCommand(object o) => _elements.Remove((GraphHalfEdge)o);

        private void OnEditEdgeCommand(object o) 
        {
            var edge = (GraphEdge)o;
            var info = new EdgeInfo(
                edge.A.Number, 
                edge.B.Number, 
                _matrix[edge.B.Number, edge.A.Number].Value,
                _matrix[edge.A.Number, edge.B.Number].Value);
            
            if (_dialogSirvice.ShowDialog(info) == true) 
            {
                _matrix[edge.A.Number, edge.B.Number].Value = info.ToB;
                _matrix[edge.B.Number, edge.A.Number].Value = info.ToA;
            }
        }

        private void OnSaveGraphCommand(object o) 
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XAML file (*.xaml)|*.xaml";

            if (saveFileDialog.ShowDialog() == true)
            {
                var serializationInfo = new GraphSerializationInfo
                {
                    Matrix = _matrix.Cast<Cell>().Select(e => e.Value).ToArray(),
                    VertexesCoordinates = _vertexes.Select(e => new Point(e.X, e.Y)).ToArray(),
                    Size = Size,
                };
                XamlServices.Save(saveFileDialog.FileName, serializationInfo);                
            }
        }

        private void OnLoadGraphCommand(object o) 
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XAML file (*.xaml)|*.xaml";

            if (openFileDialog.ShowDialog() == true) 
            {
                try 
                {
                    var info = XamlServices.Load(openFileDialog.FileName) as GraphSerializationInfo;                    
                    Size = info.Size;

                    for (int i = 0; i < info.Matrix.Length; i++) 
                    {
                        _matrix[i / Size, i % Size].Value = info.Matrix[i];
                    }

                    for (int i = 0; i < Size; i++) 
                    {
                        _vertexes[i].X = info.VertexesCoordinates[i].X;
                        _vertexes[i].Y = info.VertexesCoordinates[i].Y;
                    }                   
                }
                catch 
                {
                    _messageBoxService.ShowWarning("Неверный формат файла!");
                }
            }
        }

        private void OnRoutingCommand(object o)
        {
            if (_vertexes.Count == 0) return;
            
            switch (RoutingAlgorithm)
            {
                case RoutingAlgorithm.Random:
                    _webSimulator.RandomRouting(_packetCount);
                    break;
                case RoutingAlgorithm.Flooding:
                    _webSimulator.FloodingRouting(_packetCount);
                    break;
                case RoutingAlgorithm.Expirience:
                    _webSimulator.ExpirienceRouting(_packetCount);
                    break;
            }
        }

        private void OnSetPathCommand(object o) 
        {
            var parametres = (object[])o;
            SourceNode = (uint)parametres[0];
            TargetNode = (uint)parametres[1];
        }

        private void OnResetCommand(object o) => ResetRouting();

        private void OnClearCommand(object o) 
        {
            CanDecrement = false;
            Size = 0;
            _elements.Clear();
        }

        private void OnChangeAlgorithmCommand(object o) 
            => RoutingAlgorithm = (RoutingAlgorithm)(((object[])o)[0]);

        private void OnCancelCommand(object o) => _webSimulator.Cancel();

        private void OnPacketsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Datagram p in e.NewItems)
                    {
                        Application.Current.Dispatcher.Invoke(() => _elements.Add(new PacketWrapper(p, this)));
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var p in e.OldItems)
                    {
                        var oldPacket = _elements.First(o => o is PacketWrapper wrapper && wrapper.Packet == p);
                        Application.Current.Dispatcher.Invoke(() => _elements.Remove(oldPacket));
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    var packets = _elements.Where(o => o is PacketWrapper);

                    foreach (var p in packets.ToList())
                    {
                        Application.Current.Dispatcher.Invoke(() => _elements.Remove(p));
                    }
                    break;               
            }
        }
    }
}
