using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Graph.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BoolConverter : IValueConverter
    {
        public static BoolConverter Instance { get; private set; }

        static BoolConverter() => Instance = new BoolConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
