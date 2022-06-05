using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.Model;
using WpfApp1.ViewModel.Secretary;

namespace WpfApp1.ViewModel.Commands.Secretary
{
    public class OpenLeaveRequestDialog : ICommand
    {
        public LeaveRequestsViewModel RequestsViewModel { get; set; }

        public OpenLeaveRequestDialog(LeaveRequestsViewModel requestsViewModel)
        {
            RequestsViewModel = requestsViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {

            Request request = (Request)parameter;
            int requestId = request.Id;
            RequestsViewModel.OpenRequestDialog(requestId);
        }
    }
}
