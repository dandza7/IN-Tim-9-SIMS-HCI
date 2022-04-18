using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class Therapy: INotifyPropertyChanged
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
        private int _drugId;
        private double _frequency;
        private int _duration;

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

        public int DrugId
        {
            get
            {
                return _drugId;
            }
            set
            {
                if (value != _drugId)
                {
                    _drugId = value;
                    OnPropertyChanged("DrugId");
                }
            }
        }

        public double Frequency
        {
            get
            {
                return _frequency;
            }
            set
            {
                if (value != _frequency)
                {
                    _frequency = value;
                    OnPropertyChanged("Frequency");
                }
            }
        }

        public int Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                if (value != _duration)
                {
                    _duration = value;
                    OnPropertyChanged("Duration");
                }
            }
        }

        public Therapy(int id, int patientId, int drugId, double frequency, int duration)
        {
            Id = id;
            PatientId = patientId;
            DrugId = drugId;
            Frequency = frequency;
            Duration = duration;
        }

        public Therapy(int patientId, int drugId, double frequency, int duration)
        {
            PatientId = patientId;
            DrugId = drugId;
            Frequency = frequency;
            Duration = duration;
        }
    }
}
