using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfApp1.View.Dialog;

namespace WpfApp1.View.Model
{
    /// <summary>
    /// Interaction logic for PatientAppointmentsView.xaml
    /// </summary>
    public partial class PatientAppointmentsView : Page
    {
        private AppointmentController _appointmentController;
        public ObservableCollection<Appointment> Appointments { get; set; }

        public PatientAppointmentsView()
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;

            Appointments = new ObservableCollection<Appointment>(_appointmentController.GetAll().ToList());
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
            int appointmentId = ((Appointment)PatientAppointmentsDataGrid.SelectedItem).Id;
            var app = Application.Current as App;
            app.Properties["appointmentId"] = appointmentId;
            app.Properties["DataView"] = PatientAppointmentsDataGrid;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new MovePatientAppointmentDialog();
        }

        private void RemoveAppointment_Click(object sender, RoutedEventArgs e)
        {
            int appointmentId = ((Appointment)PatientAppointmentsDataGrid.SelectedItem).Id;
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;

            _appointmentController.Delete(appointmentId);
            PatientAppointmentsDataGrid.ItemsSource = null;
            PatientAppointmentsDataGrid.ItemsSource = _appointmentController.UpdateAppointments();
        }
    }
}
