using System;
using System.Collections.Generic;
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
using WpfApp1.Controller;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp1.View
{
    /// <summary>
    /// Interaction logic for AppointmentForm.xaml
    /// </summary>
    public partial class AppointmentForm : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private AppointmentController _appointmentController;

        /*private DateTime _beginning;
        private DateTime _ending;*/

        public DateTime Beginning;
        public DateTime Ending;
        //private const string APPOINTMENT_DATETIME_FORMAT = "dd.MM.yyyy. HH:mm:ss";
        /*public DateTime Beginning
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
        }*/

        public AppointmentForm()
        {

            InitializeComponent();
            this.DataContext = this;

        }

        private void CreateNewAppointment(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;

            if (BeginningDTP.Text == null || EndingDTP.Text == null) return;
            Beginning = DateTime.Parse(BeginningDTP.Text);
            Ending = DateTime.Parse(EndingDTP.Text);
            Appointment appointment = new Appointment(Beginning, Ending);
            _appointmentController.Create(appointment);
        }
    }

    


}
