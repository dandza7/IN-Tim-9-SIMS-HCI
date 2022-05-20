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
using WpfApp1.View.Model.Patient;
using static WpfApp1.Model.Appointment;

namespace WpfApp1.View.Dialog.PatientDialog
{
    /// <summary>
    /// Interaction logic for ListAvailableAppointments.xaml
    /// </summary>
    public partial class ListAvailableAppointments : Page
    {
        private AppointmentController _appointmentController;
        private DoctorController _doctorController;
        public ObservableCollection<AppointmentView> AvailableAppointments { get; set; }
        public ListAvailableAppointments()
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;
            _doctorController = app.DoctorController;
            _appointmentController = app.AppointmentController;

            string priority = (string)app.Properties["priority"];
            DateTime startOfInterval = (DateTime)app.Properties["startOfInterval"];
            DateTime endOfInterval = (DateTime)app.Properties["endOfInterval"];
            int doctorId = (int)app.Properties["doctorId"];
            int patientId = (int)app.Properties["userId"];
            int oldAppointmentId = (int)app.Properties["oldAppointmentId"];
            Doctor doctor = _doctorController.GetById(doctorId);
            AvailableAppointments = new ObservableCollection<AppointmentView>(_appointmentController.GetAvailableAppointmentOptions(
                priority, startOfInterval, endOfInterval, doctorId, doctor.Specialization, patientId, oldAppointmentId).ToList());
        }
        
        private void ChooseAppointment_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            _doctorController = app.DoctorController;

            DateTime appointmentBeginning = ((AppointmentView)AvailableAppointmentsGrid.SelectedItem).Beginning;
            DateTime appointmentEnding = appointmentBeginning.AddHours(1);
            Doctor doctor = _doctorController.GetByUsername(((AppointmentView)AvailableAppointmentsGrid.SelectedItem).Username);
            int patientId = (int)app.Properties["userId"];
            int oldAppointmentId = (int)app.Properties["oldAppointmentId"];

            if(oldAppointmentId == -1)
            {
                _appointmentController.Create(new Appointment(appointmentBeginning, appointmentEnding, AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId));
            } else {
                _appointmentController.Update(new Appointment(oldAppointmentId, appointmentBeginning, appointmentEnding, AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId));
            }

            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientAppointmentsView();
        }
    }
}
