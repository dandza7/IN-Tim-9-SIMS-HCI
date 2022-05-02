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

namespace WpfApp1.View.Model.Patient
{
    /// <summary>
    /// Interaction logic for TherapyView.xaml
    /// </summary>
    public partial class TherapyView : UserControl, INotifyPropertyChanged
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
        private string _frequency;
        private string _duration;
        private string _drugName;

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

        public string Frequency
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

        public string Duration
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

        public string DrugName
        {
            get
            {
                return _drugName;
            }
            set
            {
                if (value != _drugName)
                {
                    _drugName = value;
                    OnPropertyChanged("DrugName");
                }
            }
        }

        public TherapyView()
        {
            InitializeComponent();
            DataContext = this;
        }

        public TherapyView(int id, int medicalRecordId, int drugId, string frequency, string duration, string drugName)
        {
            Id = id;
            MedicalRecordId = medicalRecordId;
            DrugId = drugId;
            Frequency = frequency;
            Duration = duration;
            DrugName = drugName;
        }
    }
}
