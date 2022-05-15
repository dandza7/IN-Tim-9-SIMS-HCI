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
        public readonly InventoryMovingRepository _inventoryMovingRepository;
        public readonly RoomRepository _roomRepository;

        public InventoryService(InventoryRepository inventoryRepository, RoomRepository roomRepositroy, InventoryMovingRepository inventoryMovingRepository)
        {
            _inventoryRepository = inventoryRepository;
            _roomRepository = roomRepositroy;
            _inventoryMovingRepository = inventoryMovingRepository;
        }

        public List<InventoryPreview> GetPreviews()
        {
            List<InventoryMoving> invMovs = _inventoryMovingRepository.GetAll();
            List<int> forDelete = new List<int>();
            foreach(InventoryMoving invMov in invMovs)
            {
                if(DateTime.Compare(invMov.MovingDate, DateTime.Today) <= 0)
                {
                    Inventory inv = _inventoryRepository.Get(invMov.InventoryId);
                    inv.RoomId = invMov.RoomId;
                    _inventoryRepository.Update(inv);
                    forDelete.Add(invMov.Id);
                }
            }
            foreach(int id in forDelete)
            {
                _inventoryMovingRepository.Delete(id);
            }
            List<Inventory> invs = _inventoryRepository.GetAll();
            List<InventoryPreview> inventoryPreviews = new List<InventoryPreview>();
            foreach(Inventory inv in invs)
            {
                string nametag = _roomRepository.Get(inv.RoomId) == null ? " " : _roomRepository.Get(inv.RoomId).Nametag;
                inventoryPreviews.Add(new InventoryPreview(inv.Id, nametag, inv.Name, inv.Type, inv.Amount));
            }
            return inventoryPreviews;
        }
        public Inventory GetById(int id)
        {
            return _inventoryRepository.GetById(id);
        }
        public List<string> GetSOPRooms()
        {
            List<Room> rooms = _roomRepository.GetAll();
            List<string> sopRooms = new List<string>();
            foreach(Room room in rooms)
            {
                if((room.Type.Equals("Storage") || room.Type.Equals("Operating")) && room.IsActive)
                {
                    sopRooms.Add(room.Nametag);
                }
            }
            return sopRooms;
        }

        public Inventory Create(Inventory inv, string roomName)
        {
            Inventory newInv = inv;
            List<Room> rooms = _roomRepository.GetAll();
            foreach(Room room in rooms)
            {
                if (room.Nametag.Equals(roomName))
                {
                    newInv.RoomId = room.Id;
                }
            }
            return _inventoryRepository.Create(newInv);
        }
        public IEnumerable<Inventory> GetAllDynamic()
        {
            return _inventoryRepository.GetAllDynamic();
        }
        public Inventory AddAmount(string name, int amount)
        {
            return _inventoryRepository.AddAmount(name, amount);
        }

    }
}
