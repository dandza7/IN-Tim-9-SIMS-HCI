﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class Meeting : INotifyPropertyChanged
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
        private string _title;
        private DateTime _beginning;
        private DateTime _ending;
        private int _roomId;
        public List<string> _users;

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


        public List<string> Users
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

        public int RoomId
        {
            get
            {
                return _roomId;
            }
            set
            {
                if (value != _roomId)
                {
                    _roomId = value;
                    OnPropertyChanged("RoomId");
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

        public Meeting(int id, string title, DateTime start, DateTime end, int roomId, List<string> list)
        {
            Id = id;
            Beginning = start;
            Ending = end;
            RoomId = roomId;
            Users = list;
            Title = title;
        }
        public Meeting(string title, DateTime start, DateTime end, int roomId, List<string> list)
        {
            Beginning = start;
            Ending = end;
            RoomId = roomId;
            Users = list;
            Title= title;
        }
    }
}