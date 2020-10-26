using Graph.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Graph.Services
{
    public class DialogSirvice
    {
        public bool? ShowDialog(object dataContext)
        {
            var Window = new PopupWindow();
            Window.DataContext = dataContext;
            Window.Owner = Application.Current.MainWindow;
            
            if (Window != null)
            {
                return Window.ShowDialog();
            }

            return false;
        }
    }
}
