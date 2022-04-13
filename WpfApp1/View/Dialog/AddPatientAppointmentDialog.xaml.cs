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
using System.Windows.Navigation;
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.View.Converter;
using WpfApp1.View.Model;

namespace WpfApp1.View.Dialog
{
    /// <summary>
    /// Interaction logic for AddPatientAppointmentDialog.xaml
    /// </summary>
    public partial class AddPatientAppointmentDialog : Window
    {

        //private readonly PatientAppointmentsView _appointmentsView;
        public AddPatientAppointmentDialog()
        {
            InitializeComponent();
            DataContext = this;
        }

        private AppointmentController _appointmentController;

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;

            if (BeginningDTP.Text == null || EndingDTP.Text == null) return;
            _appointmentController.Create(new Appointment(DateTime.Parse(BeginningDTP.Text), DateTime.Parse(EndingDTP.Text)));
            DataGrid dataView = (DataGrid)app.Properties["DataView"];
            dataView.ItemsSource = null;
            dataView.ItemsSource = _appointmentController.UpdateAppointments();
            Close();
        }
    }
}
