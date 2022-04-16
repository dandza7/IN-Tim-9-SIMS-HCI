﻿using System;
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

        private int _id;
        private DateTime _beginning;
        private DateTime _ending;
        private Doctor _doctor;
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
        public Doctor Doctor
        {
            get
            {
                return _doctor;
            }
            set
            {
                if (value != _doctor)
                {
                    _doctor = value;
                    OnPropertyChanged("Doctor");
                }
            }
        }

        public Appointment() {}
        public Appointment(DateTime beginning, DateTime ending, Doctor doctor)
        {
            Beginning = beginning;
            Ending = ending;
            Doctor = doctor;
        }
        public Appointment(int id, DateTime beginning, DateTime ending, Doctor doctor)
        {
            Id = id;
            Beginning = beginning;
            Ending = ending;
            Doctor = doctor;
        }

        
    }
}
