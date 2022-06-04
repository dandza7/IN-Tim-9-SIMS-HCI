using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1.ViewModel.Commands.Patient
{
    public class OpenAddAppointmentDialog : ICommand
    {
        public AppointmentViewModel AppointmentViewModel { get; set; }

        public OpenAddAppointmentDialog(AppointmentViewModel appointmentViewModel)
        {
            AppointmentViewModel = appointmentViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            AppointmentViewModel.OpenAddAppointmentDialog();
        }
    }
}
