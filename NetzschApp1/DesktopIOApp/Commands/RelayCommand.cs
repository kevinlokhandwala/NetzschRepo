using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DesktopIOApp.Commands
{
    class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<object> _actionToExecute;
        private Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canexecute = null)
        {
            _actionToExecute = execute;
            _canExecute = canexecute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _actionToExecute(parameter);
            CanExecuteChanged?.Invoke(this, null);
        }
    }
}
