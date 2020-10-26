using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Graph.Controls
{
    public class NotResizeTextBox : TextBox
    {
        protected override Size MeasureOverride(Size constraint) => new Size(0, 0);
    }
}
