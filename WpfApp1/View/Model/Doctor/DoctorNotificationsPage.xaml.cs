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

namespace WpfApp1.View.Model.Doctor
{
    /// <summary>
    /// Interaction logic for DoctorNotificationsPage.xaml
    /// </summary>
    public partial class DoctorNotificationsPage : Page
    {
        public NotificationController _notificationController;
        public List<Notification> Notifications;
        public DoctorNotificationsPage()
        {
            InitializeComponent();
            var app = Application.Current as App;
            _notificationController = app.NotificationController;
            Notifications = new List<Notification>();
            Notifications = (List<Notification>)_notificationController.GetUsersNotifications(1);
            NotificationsGrid.ItemsSource = Notifications;
            this.DataContext = this;
        }

    }
}
