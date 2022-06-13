using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Model.Preview;
using WpfApp1.Repository;
using WpfApp1.Repository.Interface;

namespace WpfApp1.Service
{
    public class InventoryService
    {
        public readonly IInventoryRepository _inventoryRepository;
        public readonly IInventoryMovingRepository _inventoryMovingRepository;
        public readonly RoomRepository _roomRepository;

        public InventoryService(IInventoryRepository inventoryRepository, RoomRepository roomRepositroy, IInventoryMovingRepository inventoryMovingRepository)
        {
            _inventoryRepository = inventoryRepository;
            _roomRepository = roomRepositroy;
            _inventoryMovingRepository = inventoryMovingRepository;
        }
        public IEnumerable<Inventory> GetAll()
        {
            return _inventoryRepository.GetAll();
        }
        public IEnumerable<InventoryPreview> GetPreviews()
        {
            List<InventoryMoving> invMovs = _inventoryMovingRepository.GetAll().ToList();
            List<int> forDelete = new List<int>();
            foreach(InventoryMoving invMov in invMovs)
            {
                if(DateTime.Compare(invMov.MovingDate, DateTime.Today) <= 0)
                {
                    Inventory inv = _inventoryRepository.GetById(invMov.InventoryId);
                    inv.RoomId = invMov.RoomId;
                    _inventoryRepository.Update(inv);
                    forDelete.Add(invMov.Id);
                }
            }
            foreach(int id in forDelete)
            {
                _inventoryMovingRepository.Delete(id);
            }
            List<Inventory> invs = _inventoryRepository.GetAll().ToList();
            List<InventoryPreview> inventoryPreviews = new List<InventoryPreview>();
            foreach(Inventory inv in invs)
            {
                string nametag = _roomRepository.GetById(inv.RoomId) == null ? " " : _roomRepository.GetById(inv.RoomId).Nametag;
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
            List<Room> rooms = _roomRepository.GetAll().ToList();
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

        public Inventory Create(Inventory inv)
        {
            return _inventoryRepository.Create(inv);

        }
        public IEnumerable<Inventory> GetAllDynamic()
        {
            return _inventoryRepository.GetAllDynamic();
        }

        public void MoveInventoryToMainStorage(int roomId)
        {
            List<Inventory> inventories = this._inventoryRepository.GetAll().ToList();
            foreach (Inventory inventory in inventories)
            {
                if (inventory.RoomId == roomId)
                {
                    inventory.RoomId = 1;
                    _inventoryRepository.Update(inventory);
                }
            }
            _inventoryRepository.UpdateAll(inventories);
        }
    }
}
