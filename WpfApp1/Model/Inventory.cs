using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class Inventory
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }

        public Inventory(int id, int roomId, string name, string type, int amount)
        {
            Id = id;
            RoomId = roomId;
            Name = name;
            Type = type;
            Amount = amount;
        }
        public Inventory(int roomId, string name, string type, int amount)
        {
            RoomId = roomId;
            Name = name;
            Type = type;
            Amount = amount;
        }
    }
}
