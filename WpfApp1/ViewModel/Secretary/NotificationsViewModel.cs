using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.View.Dialog;
using WpfApp1.ViewModel.Commands.Secretary;

namespace WpfApp1.ViewModel.Secretary
{
    public class NotificationsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private NotificationController _notificationController;
        private ObservableCollection<Notification> _notifications;
        private Notification _selectedNotification;
        private string _content;
        private string _title;
        public OpenReadNotificationDialog OpenDialog { get; set; }

        public NotificationsViewModel()
        {
            LoadNotifications();
            OpenDialog = new OpenReadNotificationDialog(this);
        }

        public ObservableCollection<Notification> Notifications
        {
            get
            {
                return _notifications;
            }
            set
            {
                if (value != _notifications)
                {
                    _notifications = value;
                    OnPropertyChanged("Notifications");
                }
            }
        }
        public Notification SelectedNotification
        {
            get
            {
                return _selectedNotification;
            }
            set
            {
                if (value != _selectedNotification)
                {
                    _selectedNotification = value;
                    OnPropertyChanged("SelectedNotification");
                }
            }
        }
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (value != _content)
                {
                    _content = value;
                    OnPropertyChanged("Content");
                }
            }
        }
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged("SelectedNotification");

            }
        }

        private void LoadNotifications()
        {
            var app = Application.Current as App;
            _notificationController = app.NotificationController;
            int userId = (int)app.Properties["userId"];
            Notifications = new ObservableCollection<Notification>(_notificationController.GetUsersNotifications(userId));
        }

        private void setSelectedNotification()
        {
            var app = Application.Current as App;
            Content = SelectedNotification.Content;
        }

        public void OpenNotificationDialog()
        {
            var app = Application.Current as App;
            setSelectedNotification();
            Console.WriteLine(Content);
            var s = new SecretaryViewNotificationDialog(SelectedNotification.Id);
            s.Show();
        }
    }
}
