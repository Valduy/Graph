using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Graph.Converters
{
    [ValueConversion(typeof(List<Point>), typeof(PathSegmentCollection))]
    public class EdgePathConverter : IValueConverter
    {
        public static EdgePathConverter Instance { get; private set; }

        static EdgePathConverter() => Instance = new EdgePathConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<Point> points = (List<Point>)value;
            PointCollection pointCollection = new PointCollection();

            foreach (Point point in points)
            {
                pointCollection.Add(point);
            }

            return pointCollection;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
