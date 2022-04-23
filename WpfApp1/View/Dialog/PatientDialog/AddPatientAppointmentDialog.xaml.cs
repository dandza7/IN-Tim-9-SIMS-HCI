using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfApp1.View.Model.Patient;
using static WpfApp1.Model.Appointment;

namespace WpfApp1.View.Dialog.PatientDialog
{
    /// <summary>
    /// Interaction logic for AddPatientAppointmentDialog.xaml
    /// </summary>
    public partial class AddPatientAppointmentDialog : Page
    {
        private AppointmentController _appointmentController;
        private DoctorController _doctorController;
        private UserController _userController;
        public ObservableCollection<User> Doctors { get; set; }
        public AddPatientAppointmentDialog()
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;

            _doctorController = app.DoctorController;
            _userController = app.UserController;

            Doctors = new ObservableCollection<User>();
            List<Doctor> allDoctors = _doctorController.GetAll().ToList();
            
            allDoctors.ForEach(doctor =>
            { 
                if (doctor.Specialization == Doctor.SpecType.generalPracticioner && doctor.IsAvailable) Doctors.Add(_userController.GetById(doctor.Id)); 
            });
        }
        
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            _doctorController = app.DoctorController;

            if (PriorityComboBox.SelectedValue == null) return;
            if (DoctorComboBox.SelectedValue == null) return;
            if (BeginningDTP.Text == null || EndingDTP.Text == null) return;

            Doctor doctor = _doctorController.GetByUsername(((User)DoctorComboBox.SelectedValue).Username);

            app.Properties["priority"] = PriorityComboBox.SelectedValue.ToString().TrimStart("System.Windows.Controls.ComboBoxItem: ".ToCharArray());
            app.Properties["doctorId"] = doctor.Id;
            app.Properties["startOfInterval"] = DateTime.Parse(BeginningDTP.Text);
            app.Properties["endOfInterval"] = DateTime.Parse(EndingDTP.Text);

            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new ListAvailableAppointments();
        }

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientAppointmentsView();
        }
    }
}
