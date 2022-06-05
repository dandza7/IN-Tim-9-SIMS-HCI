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
    public class LeaveRequestsViewModel : INotifyPropertyChanged
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
        private ObservableCollection<Request> _requests;
        private Request _request;
        private string _title;
        private string _urgency;
        private string _doctor;
        private DateTime _beginning;

        public OpenLeaveRequestDialog OpenDialog { get; set; }

        public LeaveRequestsViewModel()
        {
            LoadLeaveRequests();
            OpenDialog = new OpenLeaveRequestDialog(this);

        }
        public ObservableCollection<Request> Requests
        {
            get
            {
                return _requests;
            }
            set
            {
                if (value != _requests)
                {
                    _requests = value;
                    OnPropertyChanged("Requests");
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


        private void LoadLeaveRequests()
        {
            var app = Application.Current as App;
            _requestController = app.RequestController;

            Requests = new ObservableCollection<Request>(_requestController.GetAllPending());

        }
        public void OpenRequestDialog(int id)
        {
            var app = Application.Current as App;
            app.Properties["requestId"] = id;
            var s = new SecretaryLeaveRequestDialog();
            s.Show();
        }

    }
}
