using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class Patient: User
    {

        private string _email;
        private string _street;
        private string _city;
        private string _country;
        private int _numberOfCancellations;
        private DateTime _lastCancellationDate;
        
        public string Email
        {
            get { return _email; }
            set
            {
                if (value != _email)
                {
                    _email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        public string Street
        {
            get { return _street; }
            set
            {
                if (value != _street)
                {
                    _street = value;
                    OnPropertyChanged("Street");
                }
            }
        }

        public string City
        {
            get { return _city; }
            set
            {
                if (value != _city)
                {
                    _city = value;
                    OnPropertyChanged("City");
                }
            }
        }

        public string Country
        {
            get { return _country; }
            set
            {
                if (value != _country)
                {
                    _country = value;
                    OnPropertyChanged("Country");
                }
            }
        }

        public int NumberOfCancellations
        {
            get { return _numberOfCancellations; }
            set
            {
                if (value != _numberOfCancellations)
                {
                    _numberOfCancellations = value;
                    OnPropertyChanged("NumberOfCancellations");
                }
            }
        }
        
        public DateTime LastCancellationDate
        {
            get { return _lastCancellationDate; }
            set
            {
                if (value != _lastCancellationDate)
                {
                    _lastCancellationDate = value;
                    OnPropertyChanged("LastCancellationDate");
                }
            }
        }

        public Patient()
        {

        }

        public Patient(int id, string name, string surname): base(id, name, surname, RoleType.patient)
        {
        }

        public Patient(string name, string surname): base(name, surname, RoleType.patient)
        {
        }

        public Patient(int id, string name, string surname, string jmbg, string username, string password, string phoneNumber, 
            string email, string street, string city, string country, int numberOfCancellations, DateTime lastCancellationDate)
            : base(id, name, surname, jmbg, username, password, phoneNumber, RoleType.patient)
        {
            Email = email;
            Street = street;
            City = city;
            Country = country;
            NumberOfCancellations = numberOfCancellations;
            LastCancellationDate = lastCancellationDate;
        }
        public Patient(string email)
        {
            Email = email;
        }

        public Patient(string name, string surname, string jmbg, string username, string password, string phoneNumber, 
            string email, string street, string city, string country, int numberOfCancellations, DateTime lastCancellationDate)
            : base(name, surname, jmbg, username, password, phoneNumber, RoleType.patient)

        {
            Email = email;
            Street = street;
            City = city;
            Country = country;
            NumberOfCancellations = numberOfCancellations;
            LastCancellationDate = lastCancellationDate;
        }

        public Patient(int id, string email, string street, string city, string country, int numberOfCancellations, DateTime lastCancellationDate)
        {
            Id = id;
            Email = email;
            Street = street;
            City = city;
            Country = country;
            NumberOfCancellations = numberOfCancellations;
            LastCancellationDate = lastCancellationDate;
        }
        
    }
}
