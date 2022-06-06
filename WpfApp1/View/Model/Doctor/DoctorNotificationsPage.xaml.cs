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
        public int userId = -1;
        public DoctorNotificationsPage()
        {
            InitializeComponent();
            var app = Application.Current as App;
            userId = int.Parse(app.Properties["userId"].ToString());
            _notificationController = app.NotificationController;
            Notifications = new List<Notification>();
            Notifications = (List<Notification>)_notificationController.GetUsersNotDeletedNotifications(userId);
            NotificationsGrid.ItemsSource = Notifications;
            this.DataContext = this;
        }

        private void NotificationsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Notification notification = ((Notification)NotificationsGrid.SelectedItems[0]);
            DateLabel.Content = notification.Date;
            TitleTB.Text = notification.Title;
            ContentTB.Text = notification.Content;
            notification.IsRead = true;
            _notificationController.Update(notification);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Notification notification = ((Notification)NotificationsGrid.SelectedItems[0]);
            notification.IsDeleted = true;
            _notificationController.Update(notification);
            DateLabel.Content = "dd/mm/yyyy";
            TitleTB.Clear();
            ContentTB.Clear();
        }
    }
}
