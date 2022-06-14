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
    public class AppointmentsMonthViewModel : INotifyPropertyChanged
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
        public string _month;
        public int _num;
        public string Month
        {
            get
            {
                return _month;
            }
            set
            {
                if (value != _month)
                {
                    _month = value;
                    OnPropertyChanged("Month");
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
        public AppointmentsMonthViewModel(string month, int num)
        {
            this.Month = month;
            this.Number = num;
        }
    
    }
}
