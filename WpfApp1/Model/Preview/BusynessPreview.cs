using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model.Preview
{
    public class BusynessPreview
    {
        public string RoomNametag { get; set; }
        public string Type { get; set; }
        public DateTime Beginning { get; set; }
        public DateTime Ending { get; set; }

        public BusynessPreview(string roomNametag, string type, DateTime beginning, DateTime ending)
        {
            RoomNametag = roomNametag;
            Type = type;
            Beginning = beginning;
            Ending = ending;
        }
    }
}
