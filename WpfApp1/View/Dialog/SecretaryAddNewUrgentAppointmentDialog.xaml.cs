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
using WpfApp1.View.Model.Patient;
using static WpfApp1.Model.Appointment;
using static WpfApp1.Model.Doctor;

namespace WpfApp1.View.Dialog
{
    /// <summary>
    /// Interaction logic for SecretaryAddNewUrgentAppointmentDialog.xaml
    /// </summary>
    public partial class SecretaryAddNewUrgentAppointmentDialog : Window
    {
        private AppointmentController _appointmentController;
        private DoctorController _doctorController;
        private UserController _userController;
        private PatientController _patientController;
        private NotificationController _notificationController;
        public ObservableCollection<User> Doctors { get; set; }
        public ObservableCollection<User> Patients { get; set; }
        public ObservableCollection<AppointmentView> AvailableAppointments { get; set; }
        public SecretaryAddNewUrgentAppointmentDialog()
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;

            _doctorController = app.DoctorController;
            _userController = app.UserController;
            _patientController = app.PatientController;
            Patients = new ObservableCollection<User>();
            List<Patient> allPatients = _patientController.GetAll().ToList();
            SpecializationComboBox.ItemsSource = Enum.GetValues(typeof(SpecType)).Cast<SpecType>();
            allPatients.ForEach(patient =>
            {
                Patients.Add(_userController.GetById(patient.Id));

            });
        }
        private void Make_Appointment_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;

            _appointmentController = app.AppointmentController;
            SpecType specialization = (SpecType)SpecializationComboBox.SelectedValue;
            Patient patient = _patientController.GetByUsername(((User)PatientComboBox.SelectedValue).Username);
            int patientId = patient.Id;
            List<AppointmentView> avApps = _appointmentController.MakeUrgent(patientId, specialization, DateTime.Now).ToList();
            AvailableAppointments = new ObservableCollection<AppointmentView>(avApps);

            AvailableAppointmentsGrid.ItemsSource = AvailableAppointments;
            AvailableAppointmentsGrid.Items.Refresh();
        }
        private void Move_Appointment_Click(object sender, RoutedEventArgs e)
        {
            int appointmentId = ((AppointmentView)AvailableAppointmentsGrid.SelectedItem).Id;
            Appointment oldAppointment = _appointmentController.GetById(appointmentId);
            Console.WriteLine(appointmentId);
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            _doctorController = app.DoctorController;
            _patientController = app.PatientController;
            _notificationController = app.NotificationController;

            DateTime appointmentBeginning = ((AppointmentView)AvailableAppointmentsGrid.SelectedItem).Beginning;
            string doctor_username = ((AppointmentView)AvailableAppointmentsGrid.SelectedItem).Username;
            Doctor doctor = _doctorController.GetByUsername(doctor_username);
            Patient patient = _patientController.GetByUsername(((User)PatientComboBox.SelectedValue).Username);
            DateTime nearestMoving = _appointmentController.GetNearestMoving(appointmentId);

            Appointment movedAppointment = new Appointment(nearestMoving, nearestMoving.AddHours(1), AppointmentType.regular, 
                false, oldAppointment.DoctorId, oldAppointment.PatientId, oldAppointment.RoomId);
            
            _appointmentController.Create(movedAppointment);

            Appointment newAppointment = new Appointment(appointmentId, appointmentBeginning, appointmentBeginning.AddHours(1), AppointmentType.regular, true, doctor.Id, patient.Id, doctor.RoomId);
            _appointmentController.Update(newAppointment);
            
            string titleForPatient = "Your appointment has been moved";
            string contentForPatient = "Your appointement on " + " " + oldAppointment.Beginning + " " + "is moved to" + " " + nearestMoving;

            Notification notification = new Notification(DateTime.Now, contentForPatient, titleForPatient, oldAppointment.PatientId, false, false);
            _notificationController.Create(notification);

            string titleForDoctor = "You have new urgent appointment";
            string contentForDoctor = "You have new urgent appointment on  " + " " + appointmentBeginning;

            Notification notificationForDoctor = new Notification(DateTime.Now, contentForDoctor, titleForDoctor, doctor.Id, false, false);
            _notificationController.Create(notificationForDoctor);
        }

        private void MakeGuest_Click(object sender, RoutedEventArgs e)
        {
            var s = new SecretaryAddGuestPatientDialog();
            s.ShowDialog(); 
        }
    }
}
