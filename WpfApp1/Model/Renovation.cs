using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    
    public class Renovation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string Description { get; set; }
        public DateTime Beginning { get; set; }
        public DateTime Ending { get; set; }

        public Renovation(int id, int roomId, string description, DateTime beginning, DateTime ending)
        {
            Id = id;
            RoomId = roomId;
            Description = description;
            Beginning = beginning;
            Ending = ending;
        }
    }
}
