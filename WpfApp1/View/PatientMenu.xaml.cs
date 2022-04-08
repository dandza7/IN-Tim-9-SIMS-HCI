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
using System.Windows.Shapes;
using WpfApp1.Model;

namespace WpfApp1.View
{
    /// <summary>
    /// Interaction logic for PatientMenu.xaml
    /// </summary>
    public partial class PatientMenu : Window, INotifyPropertyChanged
    {
        internal Patient Patient { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public PatientMenu()
        {
            InitializeComponent();
            this.Patient = new Patient
            {
                Appointments = new List<Appointment>()
                {
                    new Appointment
                    {
                        Id = 1,
                        Beginning = DateTime.Now,
                        Ending = DateTime.Now
                    },
                    new Appointment
                    {
                        Id = 2,
                        Beginning = DateTime.Now,
                        Ending = DateTime.Now
                    },
                    new Appointment
                    {
                        Id = 3,
                        Beginning = DateTime.Now,
                        Ending = DateTime.Now
                    },
                    new Appointment
                    {
                        Id = 4,
                        Beginning = DateTime.Now,
                        Ending = DateTime.Now
                    }
                }
            };
            this.DataContext = this.Patient;
        }

        private void ShowAppointmentForm(object sender, RoutedEventArgs e)
        {
            var s = new AppointmentForm();
            s.Show();
        }

        
    }
}
