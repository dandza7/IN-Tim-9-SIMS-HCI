using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public DoctorsListViewModel(DoctorsList parentPage, DoctorController docCon, SurveyController surCon)
        {
            this.ParentPage = parentPage;
            doctorController = docCon;
            surveyController = surCon;
            this.Doctors = doctorController.GetAllPreviews().ToList();
        }
        public void ShowStatistics()
        {
            ParentPage.StatsFrame.Content = new DoctorStatistics(SelectedDoctor, surveyController);
        }
    }
}
