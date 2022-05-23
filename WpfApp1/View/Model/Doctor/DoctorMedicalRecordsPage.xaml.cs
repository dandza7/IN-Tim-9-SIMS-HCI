using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using WpfApp1.View.Model.Patient;

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
        TherapyController _therapyController;
        DoctorsReportController _doctorsReportController;
        MedicalRecordController _medicalRecordController;
        DrugController _drugController;

        public List<Therapy> patientTherapies = new List<Therapy>();
        public List<DoctorsReport> patientReports = new List<DoctorsReport>();
        public ObservableCollection<DoctorAppointmentView> upcomingAppointments = new ObservableCollection<DoctorAppointmentView>();
        public DoctorAppointmentView currentAppointment = new DoctorAppointmentView();

        public int userId = -1;
        public int trenutniTerminIndex = 0;


        public DoctorMedicalRecordsPage()
        {
            InitializeComponent();
            var app = Application.Current as App;
            userId = int.Parse(app.Properties["userId"].ToString());
            this.DataContext = this;
            {//controller initialization
                _appointmentController = app.AppointmentController;
                _doctorController = app.DoctorController;
                _patientController = app.PatientController;
                _therapyController = app.TherapyController;
                _doctorsReportController = app.DoctorsReportController;
                _medicalRecordController = app.MedicalRecordController;
                _drugController = app.DrugController;
            }//controller initialization

            //upcoming appointments
            foreach (Appointment a in _appointmentController.GetAllByDoctorId(userId))
                if (a.Beginning >= DateTime.Now)
                    this.upcomingAppointments.Add(
                        AppointmentConverter.ConvertAppointmentToDoctorAppointmentView(
                            a,
                            _doctorController.GetById(a.DoctorId),
                            _patientController.GetById(a.PatientId)//nema id(id je 0 uvek), a kad ima izbija exception
                            )
                        );
            //current appointment
            currentAppointment = upcomingAppointments[trenutniTerminIndex];//uzima najskoriji

            //patients reports
            foreach (Appointment a in _appointmentController.GetAllByPatientId(currentAppointment.PatientId)) patientReports.Add(_doctorsReportController.GetByAppointmentId(a.Id));


            //patient therapies
            patientTherapies = (List<Therapy>)_therapyController.GetByMedicalRecordId(_medicalRecordController.GetByPatientId(3/*currentAppointment.PatientId*/).Id);

            UpcomingAppointmentsGrid.ItemsSource = upcomingAppointments;
            ReportsGrid.ItemsSource = patientReports;
            TherapiesGrid.ItemsSource = patientTherapies;

            FromLabel.Content = currentAppointment.Beginning;
            ToLabel.Content = currentAppointment.Ending;
            PatientTextBlock.Text = currentAppointment.PatientId.ToString();

        }

        private void FutureAppointmentsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Appointment a = _appointmentController.GetById(((DoctorAppointmentView)UpcomingAppointmentsGrid.SelectedItems[0]).Id);
            currentAppointment = AppointmentConverter.ConvertAppointmentToDoctorAppointmentView(a, _doctorController.GetById(a.DoctorId), _patientController.GetById(a.PatientId));
            FromLabel.Content = currentAppointment.Beginning;
            ToLabel.Content = currentAppointment.Ending;
            PatientTextBlock.Text = currentAppointment.PatientId.ToString();
        }

        private void InfoBT_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ReportsBT_Click(object sender, RoutedEventArgs e)
        {
            TherapiesView.Visibility = ReferalView.Visibility = Visibility.Hidden;
            ReportsView.Visibility = Visibility.Visible;
        }

        private void TherapiesBT_Click(object sender, RoutedEventArgs e)
        {
            ReportsView.Visibility = ReferalView.Visibility = Visibility.Hidden;
            TherapiesView.Visibility = Visibility.Visible;
        }

        private void ReferalBT_Click(object sender, RoutedEventArgs e)
        {
            ReportsView.Visibility = TherapiesView.Visibility = Visibility.Hidden;
            ReferalView.Visibility = Visibility.Visible;

        }

        private void ClearTherapyBT_Click(object sender, RoutedEventArgs e)
        {
            TherapyIdLabel.Content = "New Therapy:";
            FrequencyTB.Clear();
            DurationTB.Clear();
        }

        private void SaveTherapyBT_Click(object sender, RoutedEventArgs e)
        {
            if ((string)TherapyIdLabel.Content == "New Therapy:")
                _therapyController.Create(new Therapy(
                    _medicalRecordController.GetByPatientId(2).Id,
                    DrugCB.SelectedIndex,
                    float.Parse(FrequencyTB.Text),
                    int.Parse(DurationTB.Text)
                    ));
            else _therapyController.Update(new Therapy(
                int.Parse((string)TherapyIdLabel.Content),
                _medicalRecordController.GetByPatientId(2).Id,
                DrugCB.SelectedIndex,
                float.Parse(FrequencyTB.Text),
                int.Parse(DurationTB.Text)
                ));
            TherapyIdLabel.Content = "New Therapy:";
            FrequencyTB.Clear();
            DurationTB.Clear();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _doctorsReportController.Create(new DoctorsReport(
                currentAppointment.Id,
                DescriptionTB.Text
                ));
            DescriptionTB.Clear();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DescriptionTB.Clear();
        }

        private void ReportsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (upcomingAppointments.Count < trenutniTerminIndex) trenutniTerminIndex++;//ne radi (ostaje 0 uvek)
            else trenutniTerminIndex = 0;
            currentAppointment = upcomingAppointments[trenutniTerminIndex];


            FromLabel.Content = currentAppointment.Beginning;
            ToLabel.Content = currentAppointment.Ending;
            PatientTextBlock.Text = currentAppointment.PatientId.ToString();
        }
    }
}
