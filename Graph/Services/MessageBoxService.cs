using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Graph.Services
{
    public class MessageBoxService
    {
        public void ShowError(string message) => ShowMessage(message, "Error", MessageBoxImage.Stop);
        public void ShowError(string message, string caption) => ShowMessage(message, caption, MessageBoxImage.Stop);
        public void ShowInformation(string message) => ShowMessage(message, "Information", MessageBoxImage.Information);
        public void ShowInformation(string message, string caption) => ShowMessage(message, caption, MessageBoxImage.Information);
        public void ShowWarning(string message) => ShowMessage(message, "Warning", MessageBoxImage.Warning);
        public void ShowWarning(string message, string caption) => ShowMessage(message, caption, MessageBoxImage.Warning);
        private void ShowMessage(string message, string caption, MessageBoxImage icon) => MessageBox.Show(message, caption, MessageBoxButton.OK, icon);
    }
}
