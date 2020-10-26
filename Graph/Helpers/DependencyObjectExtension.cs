using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Graph.Helpers
{
    public static class DependencyObjectExtension
    {
        public static T GetVisualChild<T>(this DependencyObject parent) where T : Visual
        {
            T child = default(T);

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T ?? GetVisualChild<T>(v);

                if (child != null)
                {
                    break;
                }
            }

            return child;
        }
    }
}
