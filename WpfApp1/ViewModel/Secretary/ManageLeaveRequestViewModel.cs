using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.ViewModel.Commands.Secretary;

namespace WpfApp1.ViewModel.Secretary
{
    public class ManageLeaveRequestViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private RequestController _requestController;
        private NotificationController _notificationController;
        private string _title;
        private string _urgency;
        private string _doctor;
        private DateTime _beginning;
        private DateTime _ending;
        private Request _request;
        private string _comment;
        private string _content;
        public DenyRequest Deny { get; set; }
        public AcceptRequest Accept { get; set; }
        public ManageLeaveRequestViewModel()
        {
            LoadLeaveRequest();
            Deny = new DenyRequest(this);
            Accept = new AcceptRequest(this);
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        public DateTime Beggining
        {
            get
            {
                return _beginning;
            }
            set
            {
                if (value != _beginning)
                {
                    _beginning = value;
                    OnPropertyChanged("Beggining");
                }
            }
        }
        public DateTime Ending
        {
            get
            {
                return _ending;
            }
            set
            {
                if (value != _ending)
                {
                    _ending = value;
                    OnPropertyChanged("Ending");
                }
            }
        }
        public string Doctor
        {
            get
            {
                return _doctor;
            }
            set
            {
                if (value != _doctor)
                {
                    _doctor = value;
                    OnPropertyChanged("Doctor");
                }
            }
        }

        public string Urgency
        {
            get
            {
                return _urgency;
            }
            set
            {
                if (value != _urgency)
                {
                    _urgency = value;
                    OnPropertyChanged("Urgency");
                }
            }
        }
        public Request Request
        {
            get
            {
                return _request;
            }
            set
            {
                if (value != _request)
                {
                    _request = value;
                    OnPropertyChanged("Request");
                }
            }
        }
        public string Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged("Comment");
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

        private void LoadLeaveRequest()
        {
            var app = Application.Current as App;
            _requestController = app.RequestController;
            int requestId = (int)app.Properties["requestId"];
            Request = _requestController.GetById(requestId);
            Title = Request.Title;
            Content = Request.Content;
            Beggining = Request.Beginning;
            Ending = Request.Ending;
            Urgency = Request.Urgnet ? "Urgent" : "Not urgent";
        }

        public void DenyRequest()
        {
            var app = Application.Current as App;
            _notificationController = app.NotificationController;
            int requestId = (int)app.Properties["requestId"];
            Request = _requestController.GetById(requestId);
            Request deniedRequest = new Request(Request.Id,Request.Beginning,Request.Ending,Request.RequestStatusType.Declined,Request.DoctorId,Request.Title,Request.Content,Request.Urgnet,Comment);
            _requestController.Update(deniedRequest);
            string title = "Odbijen zahtev za odstustvo";
            string content = "Vas zahtev za odsustvo" + deniedRequest.Beginning + "je odbijen zbog " + Comment ;
            Notification deniedNotification = new Notification(DateTime.Now,content, title, deniedRequest.DoctorId, false, false);
            _notificationController.Create(deniedNotification);
        }
        public void AcceptRequest()
        {
            var app = Application.Current as App;
            _notificationController = app.NotificationController;
            int requestId = (int)app.Properties["requestId"];
            Request = _requestController.GetById(requestId);
            Request acceptedRequest = new Request(Request.Id, Request.Beginning, Request.Ending, Request.RequestStatusType.Accepted, Request.DoctorId, Request.Title, Request.Content, Request.Urgnet, "");
            _requestController.Update(acceptedRequest);
            string title = "Prihvacen zahtev za odstustvo";
            string content = "Vas zahtev za odsustvo" + acceptedRequest.Beginning + "je prihvacen";
            Notification acceptedNotification = new Notification(DateTime.Now, content, title, acceptedRequest.DoctorId, false, false);
            _notificationController.Create(acceptedNotification);
        }

    }

}
