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

namespace WpfApp1.View.Model.Doctor
{
    /// <summary>
    /// Interaction logic for DoctorMedicalRecordsPage.xaml
    /// </summary>
    public partial class DoctorMedicalRecordsPage : Page
    {

        AppointmentController _appointmentController;
        DoctorController _doctorController;
        PatientController _patientController;
        public ObservableCollection<DoctorAppointmentView> upcomingAppointments = new ObservableCollection<DoctorAppointmentView>();
        public DoctorAppointmentView currentAppointment = new DoctorAppointmentView();
        public DoctorMedicalRecordsPage()
        {
            InitializeComponent();
            this.DataContext = this;
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            _doctorController = app.DoctorController;
            _patientController = app.PatientController;
            foreach (Appointment a in _appointmentController.GetAllByDoctorId(1))//ID LEKARA
            {


                if (a.Beginning > DateTime.Now)
                    this.upcomingAppointments.Add(
                            AppointmentConverter.ConvertAppointmentToDoctorAppointmentView(
                                a,
                                _doctorController.GetById(a.DoctorId),
                                _patientController.GetById(a.PatientId)
                                )
                        );
                if (a.Beginning == DateTime.Now) currentAppointment = AppointmentConverter.ConvertAppointmentToDoctorAppointmentView(
                                a,
                                _doctorController.GetById(a.DoctorId),
                                _patientController.GetById(a.PatientId)
                                );
            }
            UpcomingAppointmentsGrid.ItemsSource = upcomingAppointments;
        }

        private void FutureAppointmentsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void InfoBT_Click(object sender, RoutedEventArgs e)
        {
            MedicalRecordFrame.Content = new Doctor_MedicalRecord_Info();
        }

        private void ReportsBT_Click(object sender, RoutedEventArgs e)
        {
            MedicalRecordFrame.Content = new Doctor_MedicalRecord_Reports();
        }

        private void TherapiesBT_Click(object sender, RoutedEventArgs e)
        {
            MedicalRecordFrame.Content = new Doctor_MedicalRecord_Therapies();
        }
    }
}
