using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1.View.Model.Doctor
{
    /// <summary>
    /// Interaction logic for DoctorAppointmentView.xaml
    /// </summary>
    public partial class DoctorAppointmentView : UserControl, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public DoctorAppointmentView()
        {
            InitializeComponent();
            DataContext = this;
        }
        private int _id;
        private string _patient;
        private string _doctor;
        private int _patientId;
        private int _doctorId;
        private DateTime _beginning;
        private DateTime _ending;
        private string _type;
        private string _urgent;

        public int DoctorId
        {
            get { return _doctorId; }
            set
            {
                if (value != _doctorId)
                {
                    _doctorId = value;
                    OnPropertyChanged("DoctorId");
                }
            }
        }
        public int PatientId
        {
            get { return _patientId; }
            set
            {
                if (value != _patientId)
                {
                    _patientId = value;
                    OnPropertyChanged("PatientId");
                }
            }
        }
        public string Doctor
        {
            get { return _doctor; }
            set
            {
                if (value != _doctor)
                {
                    _doctor = value;
                    OnPropertyChanged("Doctor");
                }
            }
        }
        public string Urgent
        {
            get { return _urgent; }
            set
            {
                if (value != _urgent)
                {
                    _urgent = value;
                    OnPropertyChanged("Urgent");
                }
            }
        }
        public string Type
        {
            get { return _type; }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    OnPropertyChanged("Type");
                }
            }
        }


        public DateTime Beginning
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

        public DateTime Ending
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

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public string Patient
        {
            get { return _patient; }
            set
            {
                if (value != _patient)
                {
                    _patient = value;
                    OnPropertyChanged("Patient");
                }
            }
        }



    }

}
