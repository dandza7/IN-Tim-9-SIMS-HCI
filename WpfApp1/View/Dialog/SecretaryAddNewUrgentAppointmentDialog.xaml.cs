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
        public ObservableCollection<AppointmentView> MovableAppointments { get; set; }
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

        private void Find_Appointment_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;

            _appointmentController = app.AppointmentController;
            SpecType specialization = (SpecType)SpecializationComboBox.SelectedValue;
            Patient patient = _patientController.GetByUsername(((User)PatientComboBox.SelectedValue).Username);
            int patientId = patient.Id;
            bool isCreated = false;

            isCreated = _appointmentController.CreateUrgentAppointment(patientId, specialization, DateTime.Now);

            if(isCreated == true)
            {
                Console.WriteLine("Zakazan hitni termin.");
            }
            else
            {
                List<AppointmentView> movableAppointments = _appointmentController.GetSortedMovableAppointments(specialization, DateTime.Now).ToList();

                MovableAppointments = new ObservableCollection<AppointmentView>(movableAppointments);
                MovableAppointmentsGrid.ItemsSource = MovableAppointments;
                MovableAppointmentsGrid.Items.Refresh();
            }

        }

        private void Move_Appointment_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            _doctorController = app.DoctorController;
            _patientController = app.PatientController;
            _notificationController = app.NotificationController;

            int appointmentForMovingId = ((AppointmentView)MovableAppointmentsGrid.SelectedItem).Id;
            Appointment appointmentForMoving = _appointmentController.GetById(appointmentForMovingId);
            Patient patient = _patientController.GetByUsername(((User)PatientComboBox.SelectedValue).Username);
            DateTime nearestMoving = _appointmentController.GetNearestFreeTerm(appointmentForMovingId);

            Appointment movedAppointment = new Appointment(nearestMoving, nearestMoving.AddHours(1), AppointmentType.regular, 
                false, appointmentForMoving.DoctorId, appointmentForMoving.PatientId, appointmentForMoving.RoomId);
            
            _appointmentController.Create(movedAppointment);

            Appointment urgentAppointment = new Appointment(appointmentForMovingId, appointmentForMoving.Beginning, 
                appointmentForMoving.Beginning.AddHours(1), AppointmentType.regular, true, appointmentForMoving.DoctorId, patient.Id, appointmentForMoving.RoomId);
            
            _appointmentController.Update(urgentAppointment);

            NotifyPatient(appointmentForMoving, nearestMoving);

            NotifyDoctor(urgentAppointment);
        }

        private void NotifyPatient(Appointment oldAppointment, DateTime nearestMoving)
        {
            string titleForPatient = "Your appointment has been moved";
            string contentForPatient = "Your appointement on " + " " + oldAppointment.Beginning + " " + "is moved to" + " " + nearestMoving;

            Notification notification = new Notification(DateTime.Now, contentForPatient, titleForPatient, oldAppointment.PatientId, false, false);
            _notificationController.Create(notification);
        }

        private void NotifyDoctor(Appointment newAppointemnt)
        {
            string titleForDoctor = "You have new urgent appointment";
            string contentForDoctor = "You have new urgent appointment on  " + " " + newAppointemnt.Beginning;

            Notification notificationForDoctor = new Notification(DateTime.Now, contentForDoctor, titleForDoctor, newAppointemnt.DoctorId, false, false);
            _notificationController.Create(notificationForDoctor);

        }

        private void MakeGuest_Click(object sender, RoutedEventArgs e)
        {
            var s = new SecretaryAddGuestPatientDialog();
            s.ShowDialog(); 
        }
    }
}
