using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1.ViewModel.Commands.Executive
{
    internal class InventoryFilter : ICommand
    {
        public InventoryViewModel InventoryViewModel { get; set; }


        public InventoryFilter(InventoryViewModel inventoryViewModel)
        {
            this.InventoryViewModel = inventoryViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            InventoryViewModel.FilterInventory();
        }
    }
}
