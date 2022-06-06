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
    public class MeetingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private MeetingController _meetingController;
        private ObservableCollection<Meeting> _meetings;
        private ObservableCollection<Room> _rooms;
        private Meeting _meeting;
        private string _title;
        private DateTime _beginning;
        public OpenNewMeetingDialog OpenDialog { get; set; }

        public MeetingViewModel()
        {
            LoadMeetings();
            OpenDialog = new OpenNewMeetingDialog(this);

        }
        public ObservableCollection<Meeting> Meetings
        {
            get
            {
                return _meetings;
            }
            set
            {
                if (value != _meetings)
                {
                    _meetings = value;
                    OnPropertyChanged("Meetings");
                }
            }
        }
        public Meeting Meeting
        {
            get
            {
                return _meeting;
            }
            set
            {
                if (value != _meeting)
                {
                    _meeting = value;
                    OnPropertyChanged("Meeting");
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




        private void LoadMeetings()
        {
            var app = Application.Current as App;
            _meetingController = app.MeetingController;

            Meetings = new ObservableCollection<Meeting>(_meetingController.GetAll());
        }

        public void OpenAddNewMeeting()
        {

            var s = new SecretaryAddNewMeetingDialog();
            s.Show();
        }


    }
}
