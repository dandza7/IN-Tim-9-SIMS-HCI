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
    /// Interaction logic for SecretaryAddNewAppointmentDialog.xaml
    /// </summary>
    public partial class SecretaryAddNewAppointmentDialog : Window
    {
        private AppointmentController _appointmentController;
        private DoctorController _doctorController;
        private UserController _userController;
        private PatientController _patientController;
        public ObservableCollection<User> Doctors { get; set; }
        public ObservableCollection<User> Patients { get; set; }
        public ObservableCollection<AppointmentView> AvailableAppointments { get; set; }
        public SecretaryAddNewAppointmentDialog()
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;

            _doctorController = app.DoctorController;
            _userController = app.UserController;
            _patientController = app.PatientController;

            Patients = new ObservableCollection<User>();
            List<Patient> allPatients = _patientController.GetAll().ToList();
            TypeComboBox.ItemsSource = Enum.GetValues(typeof(AppointmentType)).Cast<AppointmentType>();
            SpecializationComboBox.ItemsSource = Enum.GetValues(typeof(SpecType)).Cast<SpecType>();
            allPatients.ForEach(patient =>
            {
                Patients.Add(_userController.GetById(patient.Id));

            }
);

        }

            private void sel(object sender, RoutedEventArgs e)
            {
                        Doctors = new ObservableCollection<User>();
            List<Doctor> allDoctors = _doctorController.GetAll().ToList();
            allDoctors.ForEach(doctor =>
            {
                if (doctor.IsAvailable && doctor.Specialization == (SpecType)SpecializationComboBox.SelectedValue) Doctors.Add(_userController.GetById(doctor.Id));

            }
            );
            DoctorComboBox.ItemsSource = Doctors;

        }
            private void Find_Appointments_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            _doctorController = app.DoctorController;

            if (PriorityComboBox.SelectedValue == null) return;
            if (DoctorComboBox.SelectedValue == null) return;
            if (PatientComboBox.SelectedValue == null) return;
            if (BeginningDTP.Text == null || EndingDTP.Text == null) return;



            Doctor doctor = _doctorController.GetByUsername(((User)DoctorComboBox.SelectedValue).Username);
            
            Patient patient = _patientController.GetByUsername(((User)PatientComboBox.SelectedValue).Username);
            int patientId = patient.Id;
            int doctorId = doctor.Id;
            int oldAppointmentId = -1;
            string priority = PriorityComboBox.SelectedValue.ToString().TrimStart("System.Windows.Controls.ComboBoxItem: ".ToCharArray());
            DateTime startOfInterval= DateTime.Parse(BeginningDTP.Text);
            DateTime endOfInterval = DateTime.Parse(EndingDTP.Text);
            SpecType spec = (SpecType)SpecializationComboBox.SelectedValue;

            AvailableAppointments = new ObservableCollection<AppointmentView>(_appointmentController.GetAvailableAppointmentOptions(
                priority, startOfInterval, endOfInterval, doctorId, spec, patientId, oldAppointmentId).ToList());

            AvailableAppointmentsGrid.ItemsSource = AvailableAppointments;
            AvailableAppointmentsGrid.Items.Refresh();
        }
        private void Schedule_Appointment_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            _doctorController = app.DoctorController;

            DateTime appointmentBeginning = ((AppointmentView)AvailableAppointmentsGrid.SelectedItem).Beginning;
            DateTime appointmentEnding = appointmentBeginning.AddHours(1);
            Doctor doctor = _doctorController.GetByUsername(((AppointmentView)AvailableAppointmentsGrid.SelectedItem).Username);
            Patient patient = _patientController.GetByUsername(((User)PatientComboBox.SelectedValue).Username);
            AppointmentType type = (AppointmentType)TypeComboBox.SelectedValue;
            int patientId = patient.Id;
            int oldAppointmentId = -1;
            if(oldAppointmentId == -1)
            {
                _appointmentController.Create(new Appointment(appointmentBeginning, appointmentEnding, type, false, doctor.Id, patientId, doctor.RoomId));
            } else {
                _appointmentController.Update(new Appointment(oldAppointmentId, appointmentBeginning, appointmentEnding, type, false, doctor.Id, patientId, doctor.RoomId));
            }
            this.Close();

        }
        private void Close_Appointments_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
