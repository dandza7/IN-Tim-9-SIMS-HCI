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
        public Room(int id, string nametag, int type)
        {
            Id = id;
            Nametag = nametag;
            Available = true;
            Type = (RoomType)type;
        }
        public Room(int id, string nametag, RoomType type)
        {
            Id = id;
            Nametag = nametag;
            Available = true;
            Type = type;
        }

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
