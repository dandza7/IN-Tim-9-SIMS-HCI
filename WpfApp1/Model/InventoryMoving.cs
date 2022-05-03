using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class InventoryMoving
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public int RoomId { get; set; }
        public DateTime MovingDate { get; set; }

        public InventoryMoving(int id, int inventoryId, int roomId, DateTime movingDate)
        {
            Id = id;
            InventoryId = inventoryId;
            RoomId = roomId;
            MovingDate = movingDate;
        }
    }
}
