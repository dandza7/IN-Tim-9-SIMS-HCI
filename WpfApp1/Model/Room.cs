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
        public RoomType Type { get; set; }

        //public Appointment[] appointment;

    }

    public enum RoomType
    {
        operating,
        meeting,
        office,
        storage,
        lobby
    }
}
