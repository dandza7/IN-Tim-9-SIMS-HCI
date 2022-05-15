using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class DynamicEquipmentRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public DateTime RequestDate { get; set; }

        public DynamicEquipmentRequest(int id, string name, int amount, DateTime requestDate)
        {
            Id = id;
            Name = name;
            Amount = amount;
            RequestDate = requestDate;
        }
        public DynamicEquipmentRequest(string name, int amount, DateTime requestDate)
        {
            Name = name;
            Amount = amount;
            RequestDate = requestDate;
        }
    }
}
