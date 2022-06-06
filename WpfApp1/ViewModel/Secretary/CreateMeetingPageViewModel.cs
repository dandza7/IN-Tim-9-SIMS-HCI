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
using WpfApp1.View.Model.Patient;
using WpfApp1.ViewModel.Commands.Secretary;

namespace WpfApp1.ViewModel.Secretary
{
    public class CreateMeetingPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private RoomController _roomController;
        private MeetingController _meetingController;
        private NotificationController _notificationController;
        private ObservableCollection<Room> _rooms;
        public UserController _userController;
        private ObservableCollection<User> _users;
        private ObservableCollection<object> checkedUsers = new ObservableCollection<object>();
        private ObservableCollection<AppointmentView> _meetings;
        private string _beginning;
        private string _ending;
        private Room _selectedRoom;
        public FindMeetingTerm Find { get; set; }
        public ScheduleMeeting Schedule { get; set; }
        public ObservableCollection<Room> Rooms
        {
            get
            {
                return _rooms;
            }
            set
            {
                if (value != _rooms)
                {
                    _rooms = value;
                    OnPropertyChanged("Rooms");
                }
            }
        }
        public ObservableCollection<User> Users
        {
            get
            {
                return _users;
            }
            set
            {
                if (value != _users)
                {
                    _users = value;
                    OnPropertyChanged("Users");
                }
            }
        }
        public ObservableCollection<object> CheckedUsers
        {
            get
            {
                return checkedUsers;
            }
            set
            {
                checkedUsers = value;
                OnPropertyChanged("CheckedUsers");
            }
        }
        public ObservableCollection<AppointmentView> Meetings
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

        public string Beginning
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

        public Room SelecetedRoom
        {
            get
            {
                return _selectedRoom;
            }
            set
            {
                if (value != _selectedRoom)
                {
                    _selectedRoom = value;
                    OnPropertyChanged("SelectedRoom");
                }
            }
        }

        public CreateMeetingPageViewModel()
        {
            LoadRooms();
            LoadUsers();
            Find = new FindMeetingTerm(this);
            Schedule = new ScheduleMeeting(this);
        }

        private void LoadRooms()
        {
            var app = Application.Current as App;
            _roomController = app.RoomController;

            Rooms = new ObservableCollection<Room>(_roomController.GetAll());

        }
        private void LoadUsers()
        {
            var app = Application.Current as App;
            _userController = app.UserController;

            Users = new ObservableCollection<User>(_userController.GetAllEmployees());

        }
        public void ScheduleMeeting()
        {
            var app = Application.Current as App;
            _meetingController = app.MeetingController;
            List<string> userIds = new List<string>();
            for (int i = 0; i < CheckedUsers.Count; i++)
            {
                User user = (User)CheckedUsers[i];
                userIds.Add(user.Id.ToString());
            }
            Meeting newMeeting = new Meeting(DateTime.Parse(Beginning), DateTime.Parse(Beginning).AddHours(1), SelecetedRoom.Id, userIds);
            _meetingController.Create(newMeeting);
            Notify(userIds);
        }
        private void Notify(List<string> users)
        {
            var app = Application.Current as App;
            _notificationController = app.NotificationController;
            string title = "Meeting Invitation";
            string content = "You are invited to meeting on " + Beginning + ".";
            foreach (string user in users) {
                int userId = Int32.Parse(user);
                Notification notification = new Notification(DateTime.Now, content, title,userId, false, false);
                _notificationController.Create(notification);   
            }
        }

    }
}
