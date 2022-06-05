using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.View.Model.Patient;

namespace WpfApp1.ViewModel
{
    public class PDFViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private User _patient;
        private ObservableCollection<AppointmentView> _appointments;
        private DateTime _from;
        private DateTime _to;

        public UserController userController { get; set; }

        public User Patient
        {
            get { return _patient; }
            set
            {
                if(value != _patient)
                {
                    _patient = value;
                    OnPropertyChanged("Patient");
                }
            }
        }

        public ObservableCollection<AppointmentView> Appointments
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

        public DateTime From
        {
            get
            {
                return _from;
            }
            set
            {
                if (value != _from)
                {
                    _from = value;
                    OnPropertyChanged("From");
                }
            }
        }

        public DateTime To
        {
            get
            {
                return _to;
            }
            set
            {
                if (value != _to)
                {
                    _to = value;
                    OnPropertyChanged("To");
                }
            }
        }

        public PDFViewModel()
        {
            LoadPatientInfo();
            LoadSearchInterval();
            LoadAppointmentReports();
        }

        private void LoadPatientInfo()
        {
            var app = Application.Current as App;
            userController = app.UserController;
            int patientId = (int)app.Properties["userId"];
            User user = userController.GetById(patientId);
            Patient = user;
        }

        private void LoadSearchInterval()
        {
            var app = Application.Current as App;
            From = (DateTime)app.Properties["from"];
            To = (DateTime)app.Properties["to"];
        }

        private void LoadAppointmentReports()
        {
            var app = Application.Current as App;
            Appointments = (ObservableCollection<AppointmentView>)app.Properties["Reports"];
        }
    }
}
