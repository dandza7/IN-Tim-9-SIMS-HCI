﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class Allergy : INotifyPropertyChanged
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
        private string _allergyName;

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

        public string AllergyName
        {
            get
            {
                return _allergyName;
            }
            set
            {
                if (value != _allergyName)
                {
                    _allergyName = value;
                    OnPropertyChanged("AllergyName");
                }
            }
        }
        public Allergy(int id, int medicalRecordId, string name)
        {
            Id = id;
            MedicalRecordId = medicalRecordId;
            AllergyName = name;
        }
    }
}
