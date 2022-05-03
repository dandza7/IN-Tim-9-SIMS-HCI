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
        private int _medicalRecordId;
        private int _drugId;
        private float _frequency;
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

        public int MedicalRecordId
        {
            get
            {
                return _medicalRecordId;
            }
            set
            {
                if (value != _medicalRecordId)
                {
                    _medicalRecordId = value;
                    OnPropertyChanged("MedicalRecordId");
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

        public float Frequency
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

        public Therapy(int id, int medicalRecordId, int drugId, float frequency, int duration)
        {
            Id = id;
            MedicalRecordId = medicalRecordId;
            DrugId = drugId;
            Frequency = frequency;
            Duration = duration;
        }

        public Therapy(int medicalRecordId, int drugId, float frequency, int duration)
        {
            MedicalRecordId = medicalRecordId;
            DrugId = drugId;
            Frequency = frequency;
            Duration = duration;
        }
    }
}
