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
        public List<int> RoomsIds { get; set; }
        public string Description { get; set; }
        public DateTime Beginning { get; set; }
        public DateTime Ending { get; set; }
        public string Type { get; set; }

        public Renovation(int id, List<int> roomsIds, string description, DateTime beginning, DateTime ending, string type)
        {
            Id = id;
            RoomsIds = roomsIds;
            Description = description;
            Beginning = beginning;
            Ending = ending;
            Type = type;
        }
    }
}
