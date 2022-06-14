using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Controller;
using WpfApp1.Model;

namespace WpfApp1.ViewModel.Secretary
{
    public class SpecializationCountViewModel : INotifyPropertyChanged
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
        private ObservableCollection<Appointment> _appointments;
        public string _spec;
        public int _num;
        public string Specialization
        {
            get
            {
                return _spec;
            }
            set
            {
                if (value != _spec)
                {
                    _spec = value;
                    OnPropertyChanged("Specialization");
                }
            }
        }

        public int Number
        {
            get
            {
                return _num;
            }
            set
            {
                _num = value;
                OnPropertyChanged("Number");

            }
        }
            public SpecializationCountViewModel(string spec, int num)
        {
            this.Specialization = spec;
            this.Number = num;
        }

        
    }
}
