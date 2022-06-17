using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.ViewModel.Secretary;

namespace WpfApp1.ViewModel.Commands.Secretary
{
    public class FindAppointments: ICommand
    {
        public AppointmentReportViewModel ReportViewModel { get; set; }
        public FindAppointments (AppointmentReportViewModel reportViewModel)
        {
            ReportViewModel = reportViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ReportViewModel.FindApps();
        }
    
    }
}
