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
        public int InventoryId { get; set; }
        public int Amount { get; set; }
        public DateTime MovingDate { get; set; }

        public DynamicEquipmentRequest(int id, int inventoryId, int amount, DateTime requestDate)
        {
            Id = id;
            InventoryId = inventoryId;
            Amount = amount;    
            MovingDate = requestDate;
        }
        public DynamicEquipmentRequest(int inventoryId, int amount, DateTime requestDate)
        {
            InventoryId = inventoryId;
            Amount = amount;
            MovingDate = requestDate;
        }
    }
}
