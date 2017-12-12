using System;
using System.Windows.Input;

namespace CMK.Services
{
    internal class CommandHandlerWithParameter<T> : ICommand
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        public CommandHandlerWithParameter(Action<T> execute)
         : this(execute, null)
        {
            _execute = execute;
        }

        public CommandHandlerWithParameter(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
    }
}