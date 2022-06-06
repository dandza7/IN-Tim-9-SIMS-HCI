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
        AllergyController _allergyController;
        NotificationController _notificationController;
        public List<Therapy> patientTherapies = new List<Therapy>();
        public List<DoctorsReport> patientReports = new List<DoctorsReport>();
        public List<int> allergiesDrugIds = new List<int>();
        public ObservableCollection<DoctorAppointmentView> upcomingAppointments = new ObservableCollection<DoctorAppointmentView>();
        public DoctorAppointmentView currentAppointment = new DoctorAppointmentView();

        public List<int> DrugIds = new List<int>();
        public List<int> DoctorIds = new List<int>();

        public int userId = -1;
        public static int trenutniTerminIndex = 0;


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
                _allergyController = app.AllergyController;
                _notificationController = app.NotificationController;
            }//controller initialization


            foreach (Drug d in _drugController.GetAll()) DrugIds.Add(d.Id);
            foreach (User u in _doctorController.GetAll()) DoctorIds.Add(u.Id);
            DoctorIds.Remove(userId);
            foreach (Appointment a in _appointmentController.GetAllByDoctorId(userId))
                if (a.Beginning >= DateTime.Now)
                    this.upcomingAppointments.Add(
                        AppointmentConverter.ConvertAppointmentToDoctorAppointmentView(
                            a,
                            _doctorController.GetById(a.DoctorId),
                            _patientController.GetById(a.PatientId)
                            )
                        );
            Debug.WriteLine(trenutniTerminIndex);
            if (upcomingAppointments.Count > 0) FillMedicalRecord(upcomingAppointments[trenutniTerminIndex]);
        }
        public void RefreshMedicalRecordForms()
        {
            UrgentCB.IsChecked = false;
            EndingDTP.Text = BeginningDTP.Text = "";
            TypeCB.SelectedItem = DoctorCB.SelectedIndex = -1;

            DescriptionLabel.Content = "New Description";
            ReportsGrid.SelectedIndex = -1;
            DescriptionTB.Clear();

            TherapyIdLabel.Content = "New Therapy:";
            TherapiesGrid.SelectedIndex = -1;
            DrugCB.SelectedIndex = -1;
            FrequencyTB.Clear();
            DurationTB.Clear();

            AllergyWarningLabel.Visibility = Visibility.Hidden;
            DrugIdLabel.Foreground = Brushes.White;
            SaveTherapyBT.IsEnabled = true;
        }
        public void FillMedicalRecord(DoctorAppointmentView appointment)
        {
            currentAppointment = appointment;
            patientReports = _doctorsReportController.GetByPatientId(currentAppointment.PatientId);
            patientTherapies = (List<Therapy>)_therapyController.GetByMedicalRecordId(_medicalRecordController.GetByPatientId(currentAppointment.PatientId).Id);
            foreach (Allergy a in _allergyController.GetAllAllergiesForPatient(_medicalRecordController.GetByPatientId(currentAppointment.PatientId).Id)) allergiesDrugIds.Add(a.DrugId);

            UpcomingAppointmentsGrid.ItemsSource = upcomingAppointments;
            ReportsGrid.ItemsSource = patientReports;
            TherapiesGrid.ItemsSource = patientTherapies;
            DrugCB.ItemsSource = DrugIds;
            DoctorCB.ItemsSource = DoctorIds;

            FromLabel.Content = currentAppointment.Beginning;
            ToLabel.Content = currentAppointment.Ending;
            PatientTextBlock.Text = currentAppointment.PatientId.ToString();

            RefreshMedicalRecordForms();
        }
        private void FutureAppointmentsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Appointment a = _appointmentController.GetById(((DoctorAppointmentView)UpcomingAppointmentsGrid.SelectedItems[0]).Id);
            FillMedicalRecord(AppointmentConverter.ConvertAppointmentToDoctorAppointmentView(a, _doctorController.GetById(a.DoctorId), _patientController.GetById(a.PatientId)));

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
            RefreshMedicalRecordForms();
        }

        private void SaveTherapyBT_Click(object sender, RoutedEventArgs e)
        {
            if ((string)TherapyIdLabel.Content == "New Therapy:")
                _therapyController.Create(new Therapy(
                    _medicalRecordController.GetByPatientId(currentAppointment.PatientId).Id,
                    DrugCB.SelectedIndex,
                    float.Parse(FrequencyTB.Text),
                    int.Parse(DurationTB.Text)
                    ));
            else _therapyController.Update(new Therapy(
                int.Parse((string)TherapyIdLabel.Content),
                _medicalRecordController.GetByPatientId(currentAppointment.PatientId).Id,
                DrugCB.SelectedIndex,
                float.Parse(FrequencyTB.Text),
                int.Parse(DurationTB.Text)
                ));

            RefreshMedicalRecordForms();
        }

        private void SaveDescriptionBT_Click(object sender, RoutedEventArgs e)
        {
            if ((string)DescriptionLabel.Content == "New Description") _doctorsReportController.Create(new DoctorsReport(currentAppointment.Id, DescriptionTB.Text));
            else
            {
                DoctorsReport dr = _doctorsReportController.GetById(((DoctorsReport)ReportsGrid.SelectedItems[0]).Id);
                dr.Description = DescriptionTB.Text;
                _doctorsReportController.Update(dr);
            }
            RefreshMedicalRecordForms();
        }

        private void ClearDescriptionBT_Click(object sender, RoutedEventArgs e)
        {
            RefreshMedicalRecordForms();
        }

        private void ReportsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReportsGrid.SelectedIndex != -1)
            {
                DescriptionLabel.Content = "Update Description";
                DescriptionTB.Text = ((DoctorsReport)ReportsGrid.SelectedItems[0]).Description;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (upcomingAppointments.Count < trenutniTerminIndex) trenutniTerminIndex++;//ne radi (ostaje 0 uvek)
            else trenutniTerminIndex = 0;
            FillMedicalRecord(upcomingAppointments[trenutniTerminIndex]);
        }

        private void TherapiesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TherapiesGrid.SelectedIndex != -1)
            {
                Therapy t = (Therapy)TherapiesGrid.SelectedItems[0];
                TherapyIdLabel.Content = "Update Therapy";
                DrugCB.SelectedItem = t.DrugId;
                FrequencyTB.Text = t.Frequency.ToString();
                DurationTB.Text = t.Duration.ToString();
            }
        }

        private void DrugCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (DrugCB.SelectedIndex != -1 && allergiesDrugIds.Contains(int.Parse(DrugCB.SelectedItem.ToString())))
            {
                AllergyWarningLabel.Visibility = Visibility.Visible;
                DrugIdLabel.Foreground = AllergyWarningLabel.Foreground;
                SaveTherapyBT.IsEnabled = false;
            }
            else
            {
                AllergyWarningLabel.Visibility = Visibility.Hidden;
                DrugIdLabel.Foreground = Brushes.White;
                SaveTherapyBT.IsEnabled = true;
            }

        }

        private void RefferalConfirmBT_Click(object sender, RoutedEventArgs e)
        {
            Enum.TryParse(TypeCB.SelectedItem.ToString(), true, out Appointment.AppointmentType type);
            SendAppointmentNotification(
                _appointmentController.Create(new Appointment(
                        Convert.ToDateTime(BeginningDTP.Text),
                        Convert.ToDateTime(EndingDTP.Text),
                        type,
                        (bool)UrgentCB.IsChecked,
                        int.Parse(DoctorCB.SelectedItem.ToString()),
                        currentAppointment.PatientId,
                        _doctorController.GetById(int.Parse(DoctorCB.SelectedItem.ToString())).RoomId
                        ))
                );
            RefreshMedicalRecordForms();

        }
        private void SendAppointmentNotification(Appointment appointment)
        {
            _notificationController.Create(new Notification(DateTime.Now, "You have a new appointment at " + appointment.Beginning, "Appointment Notification", appointment.DoctorId, false, false));
            _notificationController.Create(new Notification(DateTime.Now, "You have a new appointment at " + appointment.Beginning, "Appointment Notification", appointment.PatientId, false, false));
        }

        private void RefferalDiscardBT_Click(object sender, RoutedEventArgs e)
        {
            RefreshMedicalRecordForms();
        }
    }
}
