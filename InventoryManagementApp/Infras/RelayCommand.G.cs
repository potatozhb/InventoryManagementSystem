using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InventoryManagementApp.Infras
{

    public class RelayCommandAsync<T> : ICommand
    {
        private readonly Func<T, Task> _execute;

        private readonly Predicate<T> _canExecute;
        private bool _isExecuting;

        public RelayCommandAsync(Func<T, Task> execute, Predicate<T>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }


        public event EventHandler CanExecuteChanged;


        public bool CanExecute(object parameter)
        {
            if (_isExecuting)
                return false;

            if (_canExecute == null)
                return true;

            if (parameter == null && typeof(T).IsValueType)
                return false;

            return _canExecute((T)parameter!);
        }


        public async void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            _isExecuting = true;
            RaiseCanExecuteChanged();

            try
            {
                await _execute((T)parameter!);
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public void RaiseCanExecuteChanged() =>
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
