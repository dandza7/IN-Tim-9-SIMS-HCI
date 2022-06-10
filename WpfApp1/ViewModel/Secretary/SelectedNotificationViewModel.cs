using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Controller;

namespace WpfApp1.ViewModel.Secretary
{
    public class SelectedNotificationViewModel : INotifyPropertyChanged
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
        private string _title;
        private string _content;
        private DateTime _date;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged("Title");

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
                _content = value;
                OnPropertyChanged("Content");

            }
        }

        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                OnPropertyChanged("Date");

            }
        }
        public SelectedNotificationViewModel(int id)
        {
            var app = Application.Current as App;
            _notificationController = app.NotificationController;
            Title = _notificationController.GetById(id).Title;
            Content = _notificationController.GetById(id).Content;
            Date = _notificationController.GetById(id).Date;
        }

    }
}
