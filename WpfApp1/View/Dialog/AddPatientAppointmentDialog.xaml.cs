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

namespace WpfApp1.View.Dialog
{
    /// <summary>
    /// Interaction logic for AddPatientAppointmentDialog.xaml
    /// </summary>
    public partial class AddPatientAppointmentDialog : Page
    {
        
        private AppointmentController _appointmentController;
        private DoctorController _doctorController;
        public ObservableCollection<Doctor> Doctors { get; set; }
        public AddPatientAppointmentDialog()
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;
            _doctorController = app.DoctorController;
            Doctors = new ObservableCollection<Doctor>();
            List<Doctor> allDoctors = _doctorController.GetAll().ToList();
            allDoctors.ForEach(doctor =>
            { 
                if (doctor.Specialization == Doctor.SpecType.generalPracticioner && doctor.IsAvailable) Doctors.Add(doctor); 
            });
        }
        
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            _doctorController = app.DoctorController;
            if (DoctorComboBox.SelectedValue == null) return;
            if (BeginningDTP.Text == null || EndingDTP.Text == null) return;
            Doctor doctor = _doctorController.GetByUsername(((Doctor)DoctorComboBox.SelectedValue).Username);
            _appointmentController.Create(new Appointment(DateTime.Parse(BeginningDTP.Text), DateTime.Parse(EndingDTP.Text), AppointmentType.regular, false, doctor.Id, 3, doctor.RoomId));
            DataGrid dataView = (DataGrid)app.Properties["DataView"];
            dataView.ItemsSource = null;
            dataView.ItemsSource = _appointmentController.UpdateData();
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientAppointmentsView();
        }

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientAppointmentsView();
        }
    }
}
