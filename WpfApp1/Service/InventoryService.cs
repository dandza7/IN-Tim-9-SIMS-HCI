using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Model.Preview;
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

        public List<InventoryPreview> GetPreviews()
        {
            List<Inventory> invs = _inventoryRepository.GetAll();
            List<InventoryPreview> inventoryPreviews = new List<InventoryPreview>();
            foreach(Inventory inv in invs)
            {
                string nametag = _roomRepository.Get(inv.RoomId) == null ? " " : _roomRepository.Get(inv.RoomId).Nametag;
                inventoryPreviews.Add(new InventoryPreview(inv.Id, nametag, inv.Name, inv.Type, inv.Amount));
            }
            return inventoryPreviews;
        }
    }
}
