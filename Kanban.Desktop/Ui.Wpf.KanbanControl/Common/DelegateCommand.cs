using System;
using System.Windows.Input;

namespace Ui.Wpf.KanbanControl.Common
{
    internal class DelegateCommand : ICommand
    {
        private Action<object> ExecuteAction;

        private Func<object, bool> CanExecuteFunc;

        public DelegateCommand(Action<object> action)
        {
            ExecuteAction = action;
        }

        public DelegateCommand(Action<object> action, Func<object, bool> canExecute)
        {
            ExecuteAction = action;
            CanExecuteFunc = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (CanExecuteFunc == null)
                return true;

            return CanExecuteFunc(parameter);
        }

        public void Execute(object parameter)
        {
            ExecuteAction(parameter);
        }
    }
}