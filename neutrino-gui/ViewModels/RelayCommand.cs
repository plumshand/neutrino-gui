using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neutrino_gui.ViewModels
{
    public class RelayCommand : System.Windows.Input.ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action _action;
        private Func<bool> _canExecute;

        public RelayCommand(Action action)
        {
            _action = action;
            _canExecute = () => true;
        }

        public RelayCommand(Action action, Func<bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        public void Execute(object parameter)
        {
            if (_canExecute())
            {
                _action();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
