using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;
using WpfApp1.Repository.Interface;

namespace WpfApp1.Service
{
    public class InventoryMovingService
    {
        public readonly IInventoryMovingRepository _invMovRepository;
        public readonly IInventoryRepository _invRepository;

        public InventoryMovingService(IInventoryMovingRepository invMovRepository, IInventoryRepository invRepository)
        {
            _invMovRepository = invMovRepository;
            _invRepository = invRepository;
        }
        public InventoryMoving MoveToday(InventoryMoving invMov)
        {
            Inventory updatedInv = _invRepository.GetById(invMov.InventoryId);
            updatedInv.RoomId = invMov.RoomId;
            _invRepository.Update(updatedInv);
            return invMov;
        }
        public InventoryMoving Create(InventoryMoving invMov)
        {
            return _invMovRepository.Create(invMov);
        }

        public void CancelInvenoryMovings(int roomId)
        {
            List<InventoryMoving> inventoryMovings = this._invMovRepository.GetAll().ToList();
            foreach (InventoryMoving inventoryMoving in inventoryMovings)
            {
                if (inventoryMoving.RoomId == roomId)
                    _invMovRepository.Delete(inventoryMoving.Id);
            }
        }
    }
}
