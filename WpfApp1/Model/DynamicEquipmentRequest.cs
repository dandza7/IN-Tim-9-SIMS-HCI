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
        public DateTime ArrivalDate { get; set; }

        public DynamicEquipmentRequest(int id, string name, int amount, DateTime arrivalDate)
        {
            Id = id;
            Name = name;
            Amount = amount;
            ArrivalDate = arrivalDate;
        }
        public DynamicEquipmentRequest(string name, int amount, DateTime arrivalDate)
        {
            Name = name;
            Amount = amount;
            ArrivalDate = arrivalDate;
        }
    }
}
