using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class Drug : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private int _id;
        private string _name;
        private string _info;
        private bool _isVerified;
        private bool _isRejected;
        private string _comment;

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



        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Info
        {
            get
            {
                return _info;
            }
            set
            {
                if (value != _info)
                {
                    _info = value;
                    OnPropertyChanged("Info");
                }
            }
        }

        public bool IsVerified
        {
            get
            {
                return _isVerified;
            }
            set
            {
                if (value != _isVerified)
                {
                    _isVerified = value;
                    OnPropertyChanged("IsVerified");
                }
            }
        }

        public bool IsRejected
        {
            get
            {
                return _isRejected;
            }
            set
            {
                if (value != _isRejected)
                {
                    _isRejected = value;
                    OnPropertyChanged("IsRejected");
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

        public Drug(int id, string name, string info, bool isVerified, bool isRejected, string comment)
        {
            Id = id;
            Name = name;
            Info = info;
            IsVerified = isVerified;
            IsRejected = isRejected;
            Comment = comment;
        }
    }
}
