using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class Patient
    {
        public IEnumerable<Appointment> Appointments { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string JMBG { get; set; }


        public Patient()
        {

        }

        public Patient(int id, string name, string surname)
        {
            Appointments = new List<Appointment>();
            Id = id;
            Name = name;
            Surname = surname;
        }
        public Patient(string name, string surname)
        {
            Appointments = new List<Appointment>();
            Name = name;
            Surname = surname;
        }

        public Patient(int id, string name, string surname, string jMBG, string username, string password)
        {
            Appointments = new List<Appointment>();
            Id = id;
            Name = name;
            Surname = surname;
            Username = username;
            Password = password;
            JMBG = jMBG;
        }

        public Patient(string name, string surname, string jMBG, string username, string password)
        {
            Name = name;
            Surname = surname;
            Username = username;
            Password = password;
            JMBG = jMBG;
        }
    }
}
