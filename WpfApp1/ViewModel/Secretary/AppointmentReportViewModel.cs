using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.View.Model.Secretary;
using WpfApp1.ViewModel.Commands.Secretary;

namespace WpfApp1.ViewModel.Secretary
{
    public class AppointmentReportViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private AppointmentController _appointmentController;
        private ObservableCollection<SecretaryAppointmentView> _appointments;
        private string _beginning;
        private string _ending;
        public FindAppointments Find { get; set; }
        public string Beginning
        {
            get
            {
                return _beginning;
            }
            set
            {
                if (value != _beginning)
                {
                    _beginning = value;
                    OnPropertyChanged("Beginning");
                }
            }
        }
        public string Ending
        {
            get
            {
                return _ending;
            }
            set
            {
                if (value != _ending)
                {
                    _ending = value;
                    OnPropertyChanged("Ending");
                }
            }
        }
        public AppointmentReportViewModel()
        {
 
            Find = new FindAppointments(this);
        }

        public ObservableCollection<SecretaryAppointmentView> Appointments
        {
            get
            {
                return _appointments;
            }
            set
            {
                if (value != _appointments)
                {
                    _appointments = value;
                    OnPropertyChanged("Appointments");
                }
            }
        }
        public void LoadApps()
        {

            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            int userId = (int)app.Properties["userId"];

        }
            public void FindApps()
        {
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            int userId = (int)app.Properties["userId"];
            Console.WriteLine(Beginning);
            Appointments = new ObservableCollection<SecretaryAppointmentView>(_appointmentController.GetSecretaryAppointmentViewsInTimeInterval(DateTime.Parse(Beginning), DateTime.Parse(Ending)));
        }
    }
}
