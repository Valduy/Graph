using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Graph.Converters
{
    [ValueConversion(typeof(int), typeof(int))]
    public class SizeToLastIndexConverter : IValueConverter
    {
        public static SizeToLastIndexConverter Instance { get; private set; }

        static SizeToLastIndexConverter() => Instance = new SizeToLastIndexConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Math.Max(0, (int)value - 1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
