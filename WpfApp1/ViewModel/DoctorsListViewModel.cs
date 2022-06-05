using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Controller;
using WpfApp1.Model.Preview;
using WpfApp1.View.Model.Executive.ExecutiveStatisticsDialogs;

namespace WpfApp1.ViewModel
{
    internal class DoctorsListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private DoctorPreview selectedDoctor;
        public DoctorPreview SelectedDoctor
        {
            get { return selectedDoctor; }
            set
            {
                if (value != selectedDoctor)
                {
                    selectedDoctor = value;
                    OnPropertyChanged("SelectedDoctor");
                    ShowStatistics();
                }
            }
        }
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public DoctorsList ParentPage { get; set; }
        private DoctorController doctorController;
        private SurveyController surveyController;
        public List<DoctorPreview> Doctors { get; set; }
        public DoctorsListViewModel(DoctorsList parentPage)
        {
            this.ParentPage = parentPage;
            var app = Application.Current as App;
            doctorController = app.DoctorController;
            surveyController = app.SurveyController;
            this.Doctors = doctorController.GetAllPreviews().ToList();
            if(Doctors.Count > 0)
                SelectedDoctor = Doctors[0];
        }
        public void ShowStatistics()
        {
            ParentPage.StatsFrame.Content = new DoctorStatistics(SelectedDoctor);
            ParentPage.sb1.Begin();
        }
    }
}
