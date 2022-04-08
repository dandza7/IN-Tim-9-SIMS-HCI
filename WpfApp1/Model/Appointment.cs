using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    internal class Appointment
    {
        public int Id { get; set; }
        public DateTime Beginning { get; set; }  
        public DateTime Ending { get; set; }
    }
}
