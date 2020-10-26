using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Graph.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    public class VertexNumberConverter : IValueConverter
    {
        public static VertexNumberConverter Instance { get; private set; }

        static VertexNumberConverter() => Instance = new VertexNumberConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"To vertex {value}:";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
