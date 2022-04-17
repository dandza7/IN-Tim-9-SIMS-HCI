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

namespace WpfApp1.View.Dialog
{
    /// <summary>
    /// Interaction logic for MovePatientAppointmentDialog.xaml
    /// </summary>
    public partial class MovePatientAppointmentDialog : Page
    {
        private AppointmentController _appointmentController;
        private DoctorController _doctorController;
        private int _id;
        public ObservableCollection<Doctor> Doctors { get; set; }
        public MovePatientAppointmentDialog()
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

        private void MoveAppointment_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            _doctorController = app.DoctorController;
            if (BeginningDTP.Text == null || EndingDTP.Text == null) return;
            if (DoctorComboBox.SelectedValue == null) return;
            Doctor doctor = _doctorController.GetByUsername(((Doctor)DoctorComboBox.SelectedValue).Username);
            _appointmentController.Update(new Appointment(
                (int)app.Properties["appointmentId"], 
                DateTime.Parse(BeginningDTP.Text), 
                DateTime.Parse(EndingDTP.Text), 
                AppointmentType.regular, 
                false, 
                doctor.Id, 
                3, 
                doctor.RoomId));
            DataGrid dataView = (DataGrid)app.Properties["DataView"];
            dataView.ItemsSource = null;
            dataView.ItemsSource = _appointmentController.UpdateData();
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientAppointmentsView();
        }
    }
}
