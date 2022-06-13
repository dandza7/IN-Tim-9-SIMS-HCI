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
using WpfApp1.View.Dialog.PatientDialog;

namespace WpfApp1.View.Model.Patient
{
    /// <summary>
    /// Interaction logic for PatientProfileView.xaml
    /// </summary>
    public partial class PatientProfileView : Page
    {
        private NotificationController _notificationController;
        private AppointmentController _appointmentController;
        private PatientController _patientController;
        private UserController _userController;
        private DrugController _drugController;
        private DoctorsReportController _doctorsReportController;
        private TherapyController _therapyController;

        public ObservableCollection<Notification> Notifications { get; set; }
        public ObservableCollection<TherapyView> Therapies { get; set; }
        public ObservableCollection<AppointmentView> Reports { get; set; }
        public PatientView Patient { get; set; }

        public bool IsSelected { get; set; } = false;

        public PatientProfileView()
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;

            _patientController = app.PatientController;
            _notificationController = app.NotificationController;
            _userController = app.UserController;
            _drugController = app.DrugController;
            _appointmentController = app.AppointmentController;
            _therapyController = app.TherapyController;

            int patientId = (int)app.Properties["userId"];

            Patient = PatientConverter.ConvertPatientToPatientView(_userController.GetById(patientId), _patientController.GetById(patientId));
            _notificationController.DeleteOldPatientsTherapyNotifications(patientId);
            _notificationController.GetScheduledTherapyNotifications(patientId);
            _notificationController.GetScheduledAlarmsForPatient(patientId);
            Notifications = new ObservableCollection<Notification>(_notificationController.GetUsersNotDeletedNotifications(patientId));

            List<Therapy> therapies = _therapyController.GetPatientsTherapies(patientId).ToList();
            ObservableCollection<TherapyView> therapyViews = new ObservableCollection<TherapyView>();
            foreach(Therapy therapy in therapies)
            {
                Drug drug = _drugController.GetById(therapy.DrugId);
                therapyViews.Add(TherapyConverter.ConvertTherapyAndDrugToTherapyView(therapy, drug));
            }
            Therapies = therapyViews;

            Reports = new ObservableCollection<AppointmentView>(_appointmentController.GetPatientsReportsView(patientId));
            app.Properties["Reports"] = Reports;
        }

        private void DeleteNotification_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            int patientId = (int)app.Properties["userId"];
            int notificationId = ((Notification)PatientNotificationsDataGrid.SelectedItem).Id;

            _notificationController = app.NotificationController;

            _notificationController.DeleteLogically(notificationId);
            PatientNotificationsDataGrid.ItemsSource = null;
            PatientNotificationsDataGrid.ItemsSource = _notificationController.GetUsersNotDeletedNotifications(patientId);
        }

        private void SearchReports_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            int patientId = (int)app.Properties["userId"];

            if (!FromDP.SelectedDate.HasValue)
            {
                PatientErrorMessageBox.Show("You didn't select the start of the searching interval!");
                return;
            }

            if (!ToDP.SelectedDate.HasValue)
            {
                PatientErrorMessageBox.Show("You didn't select the end of the searching interval!");
                return;
            }

            DateTime startOfInterval = FromDP.SelectedDate.Value;
            DateTime endOfInterval = ToDP.SelectedDate.Value.AddDays(1);

            if(startOfInterval > endOfInterval)
            {
                PatientErrorMessageBox.Show("Start of interval must be before its ending!");
                return;
            }

            if(endOfInterval >= DateTime.Now)
            {
                endOfInterval = DateTime.Now.AddMinutes(-1);
            }

            if(startOfInterval >= DateTime.Now)
            {
                PatientErrorMessageBox.Show("Start of interval must be in the past!");
                return;
            }

            ObservableCollection<AppointmentView> apps = new ObservableCollection<AppointmentView>(_appointmentController.GetPatientsReportsInTimeInterval(patientId, startOfInterval, endOfInterval));
            PatientReportsDataGrid.ItemsSource = null;
            PatientReportsDataGrid.ItemsSource = apps;
            app.Properties["from"] = startOfInterval;
            app.Properties["to"] = endOfInterval;
            IsSelected = true;
            app.Properties["Reports"] = apps;
        }

        private void ShowDetails_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            int appointmentId = ((AppointmentView)PatientReportsDataGrid.SelectedItem).Id;

            app.Properties["appointmentId"] = appointmentId;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new ShowReportDetails();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            const string APPOINTMENT_HISTORY_HELP = "Here you can see all the appointments you've been to. " +
                "If you are interested about appointments in certain time interval you can select the time interval and then press the " +
                "search button. If you are interested in a more detailed view of appointment click on the button for details. " +
                "After reading the details you are welcome to grade the appointment.";
            PatientHelp.Show(APPOINTMENT_HISTORY_HELP);
        }

        private void PDFConverter_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            if (!IsSelected)
            {
                app.Properties["from"] = new DateTime(2001, 1, 1);
                app.Properties["to"] = new DateTime(2030, 1, 1);
            }
            Window PDFView = new PDFView();
            PDFView.Show();
        }
    }
}
