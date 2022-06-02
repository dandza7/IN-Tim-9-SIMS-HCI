using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1.ViewModel.Commands.Executive
{
    internal class NavigateHospitalStats : ICommand
    {
        public StatisticsViewModel StatisticsViewModel { get; set; }


        public NavigateHospitalStats(StatisticsViewModel statisticsViewModel)
        {
            this.StatisticsViewModel = statisticsViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            StatisticsViewModel.NavigateFrame("View/Model/Executive/ExecutiveStatisticsDialogs/HospitalStatistics.xaml");
        }
    }
}
