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

namespace Graph.Controls
{
    /// <summary>
    /// Interaction logic for RoutingTableControl.xaml
    /// </summary>
    public partial class RoutingTableControl : UserControl
    {
        public static DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size",
            typeof(int),
            typeof(RoutingTableControl),
            new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnSizePropertyChanged), new CoerceValueCallback(SizePropertyCoerceValue)));

        public static DependencyProperty CellWidthProperty = DependencyProperty.Register(
            "CellWidth",
            typeof(double),
            typeof(RoutingTableControl));

        public static DependencyProperty CellHeightProperty = DependencyProperty.Register(
            "CellHeight",
            typeof(double),
            typeof(RoutingTableControl));

        public static DependencyProperty CellMinWidthProperty = DependencyProperty.Register(
            "CellMinWidth",
            typeof(double),
            typeof(RoutingTableControl),
            new FrameworkPropertyMetadata(40d));

        public static DependencyProperty CellMinHeightProperty = DependencyProperty.Register(
            "CellMinHeight",
            typeof(double),
            typeof(RoutingTableControl),
            new FrameworkPropertyMetadata(40d));

        public int Size
        {
            get => (int)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public double CellWidth
        {
            get => (double)GetValue(CellWidthProperty);
            set => SetValue(CellWidthProperty, value);
        }

        public double CellHeight
        {
            get => (double)GetValue(CellHeightProperty);
            set => SetValue(CellHeightProperty, value);
        }

        public double CellMinWidth
        {
            get => (double)GetValue(CellMinWidthProperty);
            set => SetValue(CellMinWidthProperty, value);
        }

        public double CellMinHeight
        {
            get => (double)GetValue(CellMinHeightProperty);
            set => SetValue(CellMinHeightProperty, value);
        }

        public RoutingTableControl()
        {
            InitializeComponent();
            SizeChanged += (o, e) => CalculateWidthAndHeight();
        }

        private void CalculateWidthAndHeight()
        {
            CellWidth = (Size == 0) ? 0 : Matrix.ActualWidth / Size;
            CellHeight = (Size == 0) ? 0 : Matrix.ActualHeight / Size;
        }

        private static void OnSizePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var source = (RoutingTableControl)sender;
            int newSize = (int)e.NewValue;

            UpdateTopHeader(source, newSize);
            UpdateLeftHeader(source, newSize);
            source.CalculateWidthAndHeight();
        }

        private static void UpdateTopHeader(RoutingTableControl source, int newSize)
        {
            source.TopHeader.ColumnDefinitions.Clear();
            source.TopHeader.Children.Clear();

            for (int i = 0; i < newSize; i++)
            {
                var text = new TextBlock
                {
                    Text = i.ToString(),
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    MinWidth = 40,
                };

                var column = new ColumnDefinition();

                var widthBinding = new Binding("CellWidth") { Source = source };
                column.SetBinding(Grid.WidthProperty, widthBinding);

                var minWidthBinding = new Binding("CellMinWidth") { Source = source };
                column.SetBinding(Grid.MinWidthProperty, minWidthBinding);

                source.TopHeader.ColumnDefinitions.Add(column);
                Grid.SetColumn(text, i);
                source.TopHeader.Children.Add(text);
            }
        }

        private static void UpdateLeftHeader(RoutingTableControl source, int newSize)
        {
            source.LeftHeader.RowDefinitions.Clear();
            source.LeftHeader.Children.Clear();

            for (int i = 0; i < newSize; i++)
            {
                var text = new TextBlock
                {
                    Text = i.ToString(),
                    VerticalAlignment = VerticalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    MinHeight = 40,
                };

                var row = new RowDefinition();

                var heightBinding = new Binding("CellHeight") { Source = source };
                row.SetBinding(Grid.HeightProperty, heightBinding);

                var minHeightBinding = new Binding("CellMinWidth") { Source = source };
                row.SetBinding(Grid.MinHeightProperty, minHeightBinding);

                source.LeftHeader.RowDefinitions.Add(row);
                Grid.SetRow(text, i);
                source.LeftHeader.Children.Add(text);
            }
        }

        private static object SizePropertyCoerceValue(DependencyObject sender, object baseValue)
        {
            var value = (int)baseValue;
            return (value >= 0) ? value : 0;
        }
    }
}
