using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class Note: INotifyPropertyChanged
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
        private int _patientId;
        private string _content;
        private DateTime _alarmTime;

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

        public int PatientId
        {
            get
            {
                return _patientId;
            }
            set
            {
                if (value != _patientId)
                {
                    _patientId = value;
                    OnPropertyChanged("PatientId");
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

        public DateTime AlarmTime
        {
            get
            {
                return _alarmTime;
            }
            set
            {
                if (value != _alarmTime)
                {
                    _alarmTime = value;
                    OnPropertyChanged("AlarmTime");
                }
            }
        }

        public Note(int id, int patientId, string content, DateTime alarmTime)
        {
            Id = id;
            PatientId = patientId;
            Content = content;
            AlarmTime = alarmTime;
        }

        public Note(int patientId, string content, DateTime time)
        {
            PatientId = patientId;
            Content = content;
            AlarmTime = time;
        }
    }
}
