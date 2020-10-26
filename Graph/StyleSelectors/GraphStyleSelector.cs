using Graph.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WebSimulation;

namespace Graph.StyleSelectors
{
    public class GraphStyleSelector : StyleSelector
    {
        public static GraphStyleSelector Instance
        {
            get;
            private set;
        }

        static GraphStyleSelector()
        {
            Instance = new GraphStyleSelector();
        }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(container) 
                ?? throw new InvalidOperationException("DesignerItemsControlItemStyleSelector : Could not find ItemsControl");

            if (item is GraphVertex)
            {
                return (Style)itemsControl.FindResource("Vertex");
            }

            if (item is GraphEdge)
            {
                return (Style)itemsControl.FindResource("Edge");
            }

            if (item is GraphHalfEdge) 
            {
                return (Style)itemsControl.FindResource("HalfEdge");
            }

            if (item is PacketWrapper wrapper)
            {
                return (Style)itemsControl.FindResource("Datagram");
            }

            return null;
        }
    }
}
