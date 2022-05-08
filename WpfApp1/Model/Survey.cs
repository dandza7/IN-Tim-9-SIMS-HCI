using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class Survey: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private int _id;
        private int _patientId;
        private int _doctorId;
        private int _appointmentId;
        private List<int> _grades;

        public int Id
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged("Id");
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

        public int AppointmentId
        {
            get { return _appointmentId; }
            set
            {
                if (value != _appointmentId)
                {
                    _appointmentId = value;
                    OnPropertyChanged("AppointmentId");
                }
            }
        }

        public List<int> Grades
        {
            get { return _grades; }
            set
            {
                if (value != _grades)
                {
                    _grades = value;
                    OnPropertyChanged("Grades");
                }
            }
        }

        public Survey(int id, int patientId, int doctorId, int appointmentId, List<int> grades)
        {
            Id = id;
            PatientId = patientId;
            DoctorId = doctorId;
            AppointmentId = appointmentId;
            Grades = grades;
        }

        public Survey(int patientId, int doctorId, int appointmentId, List<int> grades)
        {
            PatientId = patientId;
            DoctorId = doctorId;
            AppointmentId = appointmentId;
            Grades = grades;
        }
    }
}
