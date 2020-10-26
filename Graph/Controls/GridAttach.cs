using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Graph.Controls
{
    public static class GridAttach
    {
        #region ColumnCountProperty
        public static readonly DependencyProperty ColumnCountProperty = DependencyProperty.RegisterAttached(
            "ColumnCount",
            typeof(int),
            typeof(GridAttach),
            new PropertyMetadata(OnColumnCountChanged));

        public static void SetColumnCount(DependencyObject obj, int count) => obj.SetValue(ColumnCountProperty, count);
        public static int GetColumnCount(DependencyObject obj) => (int)obj.GetValue(ColumnCountProperty);

        private static void OnColumnCountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) 
        {
            if (!(obj is Grid grid)) return;            
            int count = Math.Max((int)e.NewValue, 0);
            grid.ColumnDefinitions.Clear();

            for (int i = 0; i < count; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }
        #endregion

        #region RowCountProperty
        public static readonly DependencyProperty RowCountProperty = DependencyProperty.RegisterAttached(
            "RowCount",
            typeof(int),
            typeof(GridAttach),
            new PropertyMetadata(OnRowCountChanged));

        public static void SetRowCount(DependencyObject obj, int count) => obj.SetValue(RowCountProperty, count);
        public static int GetRowCount(DependencyObject obj) => (int)obj.GetValue(RowCountProperty);

        private static void OnRowCountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) 
        {
            if (!(obj is Grid grid)) return;
            int count = Math.Max((int)e.NewValue, 0);
            grid.RowDefinitions.Clear();

            for (int i = 0; i < count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
        }
        #endregion
    }
}
