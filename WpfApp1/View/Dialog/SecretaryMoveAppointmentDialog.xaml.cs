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
using System.Windows.Shapes;
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.View.Converter;
using WpfApp1.View.Model;
using WpfApp1.View.Model.Patient;
using WpfApp1.View.Model.Secretary;
using static WpfApp1.Model.Appointment;
using static WpfApp1.Model.Doctor;

namespace WpfApp1.View.Dialog
{
    /// <summary>
    /// Interaction logic for SecretaryMoveAppointmentDialog.xaml
    /// </summary>
    ///         

    public partial class SecretaryMoveAppointmentDialog : Window
    {
        private AppointmentController _appointmentController;
        private DoctorController _doctorController;
        private UserController _userController;
        private PatientController _patientController;
        public ObservableCollection<User> Doctors { get; set; }
        public ObservableCollection<User> Patients { get; set; }
        public ObservableCollection<AppointmentView> AvailableAppointments { get; set; }
        private ObservableCollection<SecretaryAppointmentView> Appointments { get; set; }
        public SecretaryMoveAppointmentDialog(Appointment appointment)
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            _doctorController = app.DoctorController;
            _patientController = app.PatientController;
             
            int patientId = appointment.PatientId;
            int doctorId = appointment.DoctorId;
            int oldAppointmentId = appointment.Id;
            string priority = "Time";
            DateTime startOfInterval = appointment.Beginning.AddDays(1);
            DateTime endOfInterval = appointment.Beginning.AddDays(4).AddHours(2).AddMinutes(20);
            SpecType spec = _doctorController.GetById(appointment.DoctorId).Specialization;

            TB.Text = appointment.Id.ToString();

            AvailableAppointments = new ObservableCollection<AppointmentView>(_appointmentController.GetAvailableAppointmentOptions(
                priority, startOfInterval, endOfInterval, doctorId, spec, patientId, oldAppointmentId).ToList());

            tt.ItemsSource = AvailableAppointments;
            tt.Items.Refresh();
        }
        private void Choose_Appointment_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            _doctorController = app.DoctorController;
            _patientController = app.PatientController;
            int oldAppointmentId = int.Parse(TB.Text);
            DateTime appointmentBeginning = ((AppointmentView)tt.SelectedItem).Beginning;
            DateTime appointmentEnding= appointmentBeginning.AddHours(1);
            Appointment old = _appointmentController.GetById(oldAppointmentId);
            Doctor doctor = _doctorController.GetById(old.DoctorId);
            int patientId = old.PatientId;
            _appointmentController.Update(new Appointment(oldAppointmentId, appointmentBeginning, appointmentEnding, AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId));

            DataGrid SecretaryAppointmentsDataGrid = (DataGrid)app.Properties["SecretaryAppointmentsDataGrid"];

            Appointments = new ObservableCollection<SecretaryAppointmentView>(_appointmentController.GetSecretaryAppointmentViews().ToList());

            SecretaryAppointmentsDataGrid.ItemsSource = Appointments;
            SecretaryAppointmentsDataGrid.Items.Refresh();

            this.Close();

        }
    }
}
