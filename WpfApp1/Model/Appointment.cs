using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class Appointment: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public enum AppointmentType
        {
            surgery,
            regular
        }

        private int _id;
        private DateTime _beginning;
        private DateTime _ending;
        private AppointmentType _type;
        private bool _isUrgent;

        private int _doctorId;
        private int _patientId;
        private int _roomId;

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

        public AppointmentType Type
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

        public bool IsUrgent
        {
            get
            {
                return _isUrgent;
            }
            set
            {
                if (value != _isUrgent)
                {
                    _isUrgent = value;
                    OnPropertyChanged("IsUrgent");
                }
            }
        }

        public int DoctorId
        {
            get
            {
                return _doctorId;
            }
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
            get
            {
                return _patientId;
            }
            set
            {
                if (value != _patientId)
                {
                    _patientId = value;
                    OnPropertyChanged("PatientId");
                }
            }
        }

        public int RoomId
        {
            get
            {
                return _roomId;
            }
            set
            {
                if (value != _roomId)
                {
                    _roomId = value;
                    OnPropertyChanged("RoomId");
                }
            }
        }

        public Appointment(DateTime beginning, DateTime ending, AppointmentType type, bool isUrgent, int doctorId, int patientId, int roomId)
        {
            Beginning = beginning;
            Ending = ending;
            Type = type;
            IsUrgent = isUrgent;
            DoctorId = doctorId;
            PatientId = patientId;
            RoomId = roomId;
        }
        public Appointment(int id, DateTime beginning, DateTime ending, AppointmentType type, bool isUrgent, int doctorId, int patientId, int roomId)
        {
            Id = id;
            Beginning = beginning;
            Ending = ending;
            Type = type;
            IsUrgent = isUrgent;
            DoctorId = doctorId;
            PatientId = patientId;
            RoomId = roomId;
        }
        public Appointment()
        {
        }

    }
}
