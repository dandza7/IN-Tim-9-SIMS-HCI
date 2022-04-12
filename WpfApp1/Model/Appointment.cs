using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime Beginning { get; set; }  
        public DateTime Ending { get; set; }
        public Appointment() {}
        public Appointment(DateTime beginning, DateTime ending)
        {
            Beginning = beginning;
            Ending = ending;
        }
        public Appointment(int id, DateTime beginning, DateTime ending)
        {
            Id = id;
            Beginning = beginning;
            Ending = ending;
        }

        
    }
}
