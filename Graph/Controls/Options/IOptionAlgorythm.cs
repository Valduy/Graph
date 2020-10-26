using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Graph.Controls.Options
{
    public interface IOptionAlgorythm
    {
        void OnMouseDown(MouseButtonEventArgs e);
        void OnMouseUp(MouseButtonEventArgs e);
        void OnMouseMove(MouseEventArgs e);
        void OnMouseLeave(MouseEventArgs e);
    }
}
