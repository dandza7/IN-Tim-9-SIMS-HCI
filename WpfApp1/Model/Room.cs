using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class Room
    {
        public int Id { get; set; }
        public string Nametag { get; set; }
        public bool Available { get; set; }
        public String Type { get; set; }

        //public Appointment[] appointment;
        public Room(int id, string nametag, String type)
        {
            Id = id;
            Nametag = nametag;
            Available = true;
            Type =type;
        }

    }
}
