using Graph.Controls.Options;
using Graph.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graph.Controls
{
    public class GraphGrid : Grid 
    {
        public static readonly DependencyProperty OptionAlgorythmProperty = DependencyProperty.Register(
            "OptionAlgorythm",
            typeof(IOptionAlgorythm),
            typeof(GraphGrid));

        public IOptionAlgorythm OptionAlgorythm
        {
            get => (IOptionAlgorythm)GetValue(OptionAlgorythmProperty);
            set => SetValue(OptionAlgorythmProperty, value);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            OptionAlgorythm?.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            OptionAlgorythm?.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            OptionAlgorythm?.OnMouseMove(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            OptionAlgorythm?.OnMouseLeave(e);
        }
    }
}
