using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Graph.Attached
{
    public static class UIElementEnterAttach
    {
        public static readonly DependencyProperty AdvancesByEnterKeyProperty = DependencyProperty.RegisterAttached(
            "AdvancesByEnterKey",
            typeof(bool),
            typeof(UIElementEnterAttach),
            new UIPropertyMetadata(OnAdvancesByEnterKeyPropertyChanged));

        public static bool GetAdvancesByEnterKey(DependencyObject obj)
            => (bool)obj.GetValue(AdvancesByEnterKeyProperty);

        public static void SetAdvancesByEnterKey(DependencyObject obj, bool value)
            => obj.SetValue(AdvancesByEnterKeyProperty, value);

        private static void OnAdvancesByEnterKeyPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var element = obj as UIElement;
            if (element == null) return;

            if ((bool)e.NewValue)
            {
                element.KeyDown += KeyDown;
            }
            else
            {
                element.KeyDown -= KeyDown;
            }
        }

        private static void KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Key.Equals(Key.Enter)) return;
            var element = sender as UIElement;

            if (element != null)
            {
                element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }
    }
}
