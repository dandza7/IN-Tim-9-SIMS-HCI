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
        public String Type { get; set; }
        public bool IsActive { get; set; }
        public Room(int id, string nametag, String type, bool isActive)
        {
            Id = id;
            Nametag = nametag;
            Type = type;
            IsActive = isActive;
        }

    }
}
