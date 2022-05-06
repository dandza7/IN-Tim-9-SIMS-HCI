using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.View.Converter;
using WpfApp1.View.Dialog.PatientDialog;

namespace WpfApp1.View.Model.Patient
{
    /// <summary>
    /// Interaction logic for PatientAppointmentsView.xaml
    /// </summary>
    public partial class PatientAppointmentsView : Page
    {
        private AppointmentController _appointmentController;
        private PatientController _patientController;

        public ObservableCollection<AppointmentView> Appointments { get; set; }

        public PatientAppointmentsView()
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;
            int patientId = (int)app.Properties["userId"];
            
            _appointmentController = app.AppointmentController;

            Appointments = new ObservableCollection<AppointmentView>(_appointmentController.GetPatientsAppointmentsView(patientId).ToList());
        }

        private void AddAppointment_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            app.Properties["DataView"] = PatientAppointmentsDataGrid;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new AddPatientAppointmentDialog();
        }

        private void OpenMoveAppointmentDialog_Click(object sender, RoutedEventArgs e)
        {
            int appointmentId = ((AppointmentView)PatientAppointmentsDataGrid.SelectedItem).Id;
            var app = Application.Current as App;

            _appointmentController = app.AppointmentController;

            Appointment oldAppointment = _appointmentController.GetById(appointmentId);
            if (DateTime.Now.AddDays(1) > oldAppointment.Beginning)
            {
                PatientErrorMessageBox.Show("ERROR: You cannot move the appointment if it's beginning in less than one day!");
                return;
            }

            app.Properties["appointmentId"] = appointmentId;
            app.Properties["DataView"] = PatientAppointmentsDataGrid;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new MovePatientAppointmentDialog();
        }

        private void RemoveAppointment_Click(object sender, RoutedEventArgs e)
        {
            int appointmentId = ((AppointmentView)PatientAppointmentsDataGrid.SelectedItem).Id;
            var app = Application.Current as App;
            int patientId = (int)app.Properties["userId"];

            _appointmentController = app.AppointmentController;
            _patientController = app.PatientController;

            _appointmentController.PatientsAppointmentDelete(patientId, appointmentId);
            PatientAppointmentsDataGrid.ItemsSource = null;
            PatientAppointmentsDataGrid.ItemsSource = _appointmentController.GetPatientsAppointmentsView(patientId);

            var patient = _patientController.GetById(patientId);
            PatientErrorMessageBox.Show("You have " + (4 - patient.NumberOfCancellations) + " cancellations left in this month");
        }
    }
}
