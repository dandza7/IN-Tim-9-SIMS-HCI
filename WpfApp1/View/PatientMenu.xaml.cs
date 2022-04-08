using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using WpfApp1.Controller;
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

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _id;
        private DateTime _beginning;
        private DateTime _ending;
        private readonly AppointmentController _appointmentController;

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }


        public DateTime Beginning
        {
            get => _beginning;
            set
            {
                if (_beginning != value)
                {
                    _beginning = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime Ending
        {
            get => _ending;
            set
            {
                if (_ending != value)
                {
                    _ending = value;
                    OnPropertyChanged();
                }
            }
        }
        public PatientMenu()
        {
            InitializeComponent();
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            this.Patient = new Patient
            {
                Appointments = _appointmentController.GetAll().ToList()
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
