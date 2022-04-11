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

    }
}
