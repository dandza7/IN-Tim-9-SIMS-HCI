using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1.ViewModel.Commands.Executive
{
    internal class ConfirmMoving : ICommand
    {
        public MoveInventoryViewModel MoveInventoryViewModel { get; set; }


        public ConfirmMoving(MoveInventoryViewModel moveInventoryViewModel)
        {
            this.MoveInventoryViewModel = moveInventoryViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            MoveInventoryViewModel.ConfirmMovingF();
        }
    }
}
