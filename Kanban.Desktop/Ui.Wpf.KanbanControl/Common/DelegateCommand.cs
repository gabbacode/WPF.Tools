using System;
using System.Windows.Input;

namespace Ui.Wpf.KanbanControl.Common
{
    internal class DelegateCommand : ICommand
    {
        private readonly Action<object> executeAction;

        private readonly Func<object, bool> canExecuteFunc;

        public DelegateCommand(Action<object> action)
        {
            executeAction = action;
        }

        public DelegateCommand(Action<object> action, Func<object, bool> canExecute)
        {
            executeAction = action;
            canExecuteFunc = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (canExecuteFunc == null)
                return true;

            return canExecuteFunc(parameter);
        }

        public void Execute(object parameter)
        {
            executeAction(parameter);
        }
        
        public void ChangeCanExecute()
        {
            var eventHandler = CanExecuteChanged;
            eventHandler?.Invoke(this, EventArgs.Empty);
        }        
    }
}