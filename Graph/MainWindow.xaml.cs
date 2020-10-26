using Graph.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Graph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GraphViewModel VM = new GraphViewModel();

        public MainWindow()
        {
            InitializeComponent();            
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e) => DataContext = VM;

        private void Random_Selected(object sender, RoutedEventArgs e) 
            => VM.RoutingAlgorithm = RoutingAlgorithm.Random;

        private void Flooding_Selected(object sender, RoutedEventArgs e)
             => VM.RoutingAlgorithm = RoutingAlgorithm.Flooding;

        private void Expirience_Selected(object sender, RoutedEventArgs e)
             => VM.RoutingAlgorithm = RoutingAlgorithm.Expirience;

        private void Datagram_Selected(object sender, RoutedEventArgs e)
            => VM.RoutingType = WebSimulation.RoutingType.Datagram;

        private void VirtualChannel_Selected(object sender, RoutedEventArgs e)
            => VM.RoutingType = WebSimulation.RoutingType.VirtualChannel;
    }
}
