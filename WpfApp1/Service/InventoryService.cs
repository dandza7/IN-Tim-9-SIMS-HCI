using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class InventoryService
    {
        public readonly InventoryRepository _inventoryRepository;
        public readonly RoomRepository _roomRepository;

        public InventoryService(InventoryRepository inventoryRepository, RoomRepository roomRepositroy)
        {
            _inventoryRepository = inventoryRepository;
            _roomRepository = roomRepositroy;
        }
    }
}
