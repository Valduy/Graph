using Graph.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Graph.Helpers
{
    public static class EdgePointHelper
    {
        public static Point GetPointToConnection(GraphVertex source, GraphVertex distanation)
        {
            Point sourceCenter = FindCenterOfNode(source);
            Point distanationCenter = FindCenterOfNode(distanation);

            Point direction = FindVector(sourceCenter, distanationCenter).Unit();
            double R = Magnitude(source.Diametr, source.Diametr) / 2;

            return new Point(sourceCenter.X + direction.X * R, sourceCenter.Y + direction.Y * R);
        }

        public static Point GetPointToConnection(GraphVertex source, Point distanation)
        {
            Point sourceCenter = FindCenterOfNode(source);

            Point direction = FindVector(sourceCenter, distanation).Unit();
            double R = Magnitude(source.Diametr, source.Diametr) / 2;

            return new Point(sourceCenter.X + direction.X * R, sourceCenter.Y + direction.Y * R);
        }

        public static double Magnitude(this Point p) => Math.Sqrt(Math.Pow(p.X, 2) + Math.Pow(p.Y, 2));
        public static double Magnitude(double x, double y) => Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        public static bool PointInsideNode(GraphVertex v, Point p)
            => v.X < p.X && v.X + v.Width > p.X && v.Y < p.Y && v.Y + v.Height > p.Y;
        public static Point FindVector(Point a, Point b) => new Point(b.X - a.X, b.Y - a.Y);
        public static Point Unit(this Point v) => new Point(v.X / v.Magnitude(), v.Y / v.Magnitude());
        public static Point FindCenterOfNode(GraphVertex v) => new Point(v.X + v.Diametr / 2, v.Y + v.Diametr / 2);
    }
}
