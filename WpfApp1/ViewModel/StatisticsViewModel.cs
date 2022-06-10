using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.View.Model.Executive;
using WpfApp1.View.Model.Executive.ExecutiveStatisticsDialogs;
using WpfApp1.ViewModel.Commands.Executive;

namespace WpfApp1.ViewModel
{
    internal class StatisticsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public ExecutiveStatisticsPages ParentPage { get; set; }
        public NavigateDoctorsList NavigateDoctorsList { get; set; }
        public NavigateHospitalStats NavigateHospitalStats { get; set; }
        public StatisticsViewModel(ExecutiveStatisticsPages parentPage)
        {
            this.ParentPage = parentPage;
            this.NavigateDoctorsList = new NavigateDoctorsList(this);
            this.NavigateHospitalStats = new NavigateHospitalStats(this);
            this.ParentPage.ListFrame.Content = new DoctorsList();
        }

        public void NavigateFrame(string path)
        {
            this.ParentPage.ListFrame.NavigationService.Navigate(new Uri(path, UriKind.Relative));
            this.ParentPage.sb1.Begin();
        }

    }

}
