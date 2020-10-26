using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Graph.Converters
{
    [ValueConversion(typeof(uint?), typeof(string))]
    class NullableUintToTextConverter : IValueConverter
    {
        public static NullableUintToTextConverter Instance { get; private set; }

        static NullableUintToTextConverter() => Instance = new NullableUintToTextConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((uint?)value).ToString();
            //var number = (uint?)value;

            //return (number.HasValue) ? number.ToString() : "null";
            //if (number.HasValue)
            //{

            //}

            //return "null";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
