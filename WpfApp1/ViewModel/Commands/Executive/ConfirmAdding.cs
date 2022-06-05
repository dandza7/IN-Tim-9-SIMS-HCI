using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1.ViewModel.Commands.Executive
{
    internal class ConfirmAdding : ICommand
    {
        public NewInventoryViewModel NewInventoryViewModel { get; set; }


        public ConfirmAdding(NewInventoryViewModel newInventoryViewModel)
        {
            this.NewInventoryViewModel = newInventoryViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            NewInventoryViewModel.ConfirmAddingF();
        }
    }
}
