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

namespace WpfApp1.View.Model.Patient
{
    /// <summary>
    /// Interaction logic for PatientProfileView.xaml
    /// </summary>
    public partial class PatientProfileView : Page
    {
        private NotificationController _notificationController;
        private PatientController _patientController;

        public ObservableCollection<Notification> Notifications { get; set; }
        
        public PatientProfileView()
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;

            _patientController = app.PatientController;
            _notificationController = app.NotificationController;
            
            int patientId = (int)app.Properties["userId"];

            _notificationController.GetScheduledPatientsNotifications(patientId);
            _patientController.DeleteOldPatientsNotifications(patientId);
            Notifications = new ObservableCollection<Notification>(_notificationController.GetUsersNotDeletedNotifications(patientId));
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
    }
}
