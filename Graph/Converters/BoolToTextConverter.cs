using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Graph.Converters
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class BoolToTextConverter : IValueConverter
    {
        public static BoolToTextConverter Instance { get; private set; }

        static BoolToTextConverter() => Instance = new BoolToTextConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((bool)value) ? 1 : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var text = (string)value;

            if (int.TryParse(text, out var result))
            {
                switch (result)
                {
                    case 0: return false;
                    case 1: return true;
                }
            }

            return false;
        }
    }
}
