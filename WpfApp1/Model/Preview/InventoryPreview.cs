using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model.Preview
{
    public class InventoryPreview
    {
        public int Id { get; set; }
        public string Room { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }

        public InventoryPreview(int id, string room, string name, string type, int amount)
        {
            Id = id;
            Room = room;
            Name = name;
            Type = type;
            Amount = amount;
        }
    }
}
