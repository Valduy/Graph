using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Graph.Controls.Options;
using Graph.Helpers;
using Graph.ViewModels;

namespace Graph.Controls
{
    /// <summary>
    /// Interaction logic for GraphControl.xaml
    /// </summary>
    public partial class GraphControl : UserControl
    {
        public static DependencyProperty IsEditableProperty = DependencyProperty.Register(
            "IsEditable",
            typeof(bool),
            typeof(GraphControl),
            new FrameworkPropertyMetadata(true, new PropertyChangedCallback(OnIsEditablePropertyChanged)));

        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }

        public GraphControl()
        {
            InitializeComponent();
            Loaded += (o, e) => Move.IsChecked = true;
        }

        private void Move_Checked(object sender, RoutedEventArgs e)
        {
            ItemsContainer.Cursor = Cursors.SizeAll;
            var grid = GetGraphGrid();
            grid.OptionAlgorythm = new MoveOptionAlgorythm(grid);
        }

        private void Add_Checked(object sender, RoutedEventArgs e)
        {
            ItemsContainer.Cursor = Cursors.Hand;
            var grid = GetGraphGrid();
            grid.OptionAlgorythm = new AddOptionAlgorythm(grid, (GraphViewModel)DataContext);
        }

        private void Delete_Checked(object sender, RoutedEventArgs e)
        {
            ItemsContainer.Cursor = Cursors.Hand;
            var grid = GetGraphGrid();
            grid.OptionAlgorythm = new DeleteOptionAlgorythm(grid, (GraphViewModel)DataContext);
        }

        private void Connect_Checked(object sender, RoutedEventArgs e)
        {
            ItemsContainer.Cursor = Cursors.Cross;
            var grid = GetGraphGrid();
            grid.OptionAlgorythm = new ConnectOptionAlgorythm(grid, (GraphViewModel) DataContext);
        }

        private void Edit_Checked(object sender, RoutedEventArgs e)
        {
            ItemsContainer.Cursor = Cursors.Pen;
            var grid = GetGraphGrid();
            grid.OptionAlgorythm = new EditOptionAlgorythm(grid, (GraphViewModel)DataContext);
        }

        private void Direction_Checked(object sender, RoutedEventArgs e)
        {
            ItemsContainer.Cursor = Cursors.Arrow;
            var grid = GetGraphGrid();
            grid.OptionAlgorythm = new DirectionOptionAlgorythm(grid, (GraphViewModel)DataContext);
        }

        private GraphGrid GetGraphGrid() 
        {
            var itemsPresenter = ItemsContainer.GetVisualChild<ItemsPresenter>();
            return VisualTreeHelper.GetChild(itemsPresenter, 0) as GraphGrid;
        }

        private static void OnIsEditablePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var graph = (GraphControl)sender;
            graph.ItemsContainer.Cursor = Cursors.Arrow;
            var grid = graph.GetGraphGrid();
            grid.OptionAlgorythm = null;
        }
    }
}
