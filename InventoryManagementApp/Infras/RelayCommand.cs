using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InventoryManagementApp.Infras
{
    public class RelayCommandAsync : ICommand
    {
        private readonly Func<Task> _execute;

        private readonly Func<bool>? _canExecute;
        private bool _isExecuting;

        public RelayCommandAsync(Func<Task> execute, Func<bool>? canExecute = null)
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

            //if (parameter == null)
            //    return false;

            return _canExecute();
        }


        public async void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            _isExecuting = true;
            RaiseCanExecuteChanged();

            try
            {
                await _execute();
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

