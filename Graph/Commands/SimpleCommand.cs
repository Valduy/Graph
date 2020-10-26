using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Graph.Commands
{
    public class SimpleCommand : ICommand
    {
        public Predicate<object> CanExecuteDelegate { get; set; }
        public Action<object> ExecuteDelegate { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public SimpleCommand(Predicate<object> canExecuteDelegate, Action<object> executeDelegate)
        {
            CanExecuteDelegate = canExecuteDelegate;
            ExecuteDelegate = executeDelegate;
        }

        public SimpleCommand(Action<object> executeDelegate) => ExecuteDelegate = executeDelegate;

        public bool CanExecute(object parameter) => CanExecuteDelegate == null || CanExecuteDelegate(parameter);
        public void Execute(object parameter) => ExecuteDelegate?.Invoke(parameter);
    }
}
