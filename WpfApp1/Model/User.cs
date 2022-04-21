using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{

    public class User: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public enum RoleType
        {
            secretary,
            doctor,
            patient,
            executive
        }

        private int _id;
        private string _name;
        private string _surname;
        private string _username;
        private string _password;
        private string _phoneNumber;
        private string _jmbg;
        private RoleType _role;

        public int Id
        {
            get { return _id; }
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
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Surname
        {
            get { return _surname; }
            set
            {
                if (value != _surname)
                {
                    _surname = value;
                    OnPropertyChanged("Surname");
                }
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged("Username");
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (value != _password)
                {
                    _password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                if (value != _phoneNumber)
                {
                    _phoneNumber = value;
                    OnPropertyChanged("PhoneNumber");
                }
            }
        }

        public string Jmbg
        {
            get { return _jmbg; }
            set
            {
                if (value != _jmbg)
                {
                    _jmbg = value;
                    OnPropertyChanged("Jmbg");
                }
            }
        }

        public RoleType Role
        {
            get { return _role; }
            set
            {
                if (value != _role)
                {
                    _role = value;
                    OnPropertyChanged("Role");
                }
            }
        }

        public User()
        {

        }

        public User(int id, string name, string surname, string username, string password, string phoneNumber, string jmbg, RoleType role)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Username = username;
            Password = password;
            PhoneNumber = phoneNumber;
            Jmbg = jmbg;
            Role = role;
        }

        public User(int id, string name, string surname, RoleType role)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Role = role;
        }

        public User(string name, string surname, RoleType role)
        {
            Name = name;
            Surname = surname;
            Role = role;
        }

        public User(string name, string surname, string jmbg, string username, string password, string phoneNumber, RoleType role)
        {
            Name = name;
            Surname = surname;
            Jmbg = jmbg;
            Username = username;
            Password = password;
            PhoneNumber = phoneNumber;
            Role = role;
        }

    }
}
