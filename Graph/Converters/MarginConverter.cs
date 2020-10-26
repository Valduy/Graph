using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Graph.Converters
{
    public class MarginConverter : IMultiValueConverter
    {
        public static MarginConverter Instance { get; private set; }

        static MarginConverter() => Instance = new MarginConverter();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double x = values[0] == DependencyProperty.UnsetValue ? 0 : (double)values[0];
            double y = values[1] == DependencyProperty.UnsetValue ? 0 : (double)values[1];
            return new Thickness(x, y, 0, 0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
