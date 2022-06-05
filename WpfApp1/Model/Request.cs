using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{


    public class Request : INotifyPropertyChanged
    {
        public enum RequestStatusType
        {
            Accepted,
            Declined,
            Pending
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public Request() { }

        public Request(int v1, DateTime dateTime1, DateTime dateTime2, RequestStatusType type, int v2, string v3, string v4, bool v5,string v6)
        {
            _id = v1;
            _beginning = dateTime1;
            _ending = dateTime2;
            _status = type;
            _doctorId = v2;
            _title = v3;
            _content = v4;
            _urgent = v5;
            _comment = v6;
        }
        public Request(DateTime dateTime1, DateTime dateTime2, RequestStatusType type, int v2, string v3, string v4, bool v5, string v6)
        {

            _beginning = dateTime1;
            _ending = dateTime2;
            _status = type;
            _doctorId = v2;
            _title = v3;
            _content = v4;
            _urgent = v5;
            _comment = v6;
        }

        private int _id;
        private int _doctorId;
        private string _title;
        private string _content;
        private string _comment;
        private DateTime _beginning;
        private DateTime _ending;
        private RequestStatusType _status;
        private bool _urgent;

        public bool Urgnet
        {
            get
            {
                return _urgent;
            }
            set
            {
                if (value != _urgent)
                {
                    _urgent = value;
                    OnPropertyChanged("Urgnet");
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
        public int DoctorId
        {
            get
            {
                return _doctorId;
            }
            set
            {
                if (value != _doctorId)
                {
                    _doctorId = value;
                    OnPropertyChanged("DoctorId");
                }
            }
        }
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }
        }
        public DateTime Beginning
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
                    OnPropertyChanged("Beginning");
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

        public RequestStatusType Status
        {
            get { return _status; }
            set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

    }
}
