using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class InventoryMovingService : Service<InventoryMoving>
    {
        public readonly InventoryMovingRepository _invMovRepository;
        public readonly InventoryRepository _invRepository;

        public InventoryMovingService(InventoryMovingRepository invMovRepository, InventoryRepository invRepository)
        {
            _invMovRepository = invMovRepository;
            _invRepository = invRepository;
        }
        public InventoryMoving MoveToday(InventoryMoving invMov)
        {
            Inventory updatedInv = _invRepository.Get(invMov.InventoryId);
            updatedInv.RoomId = invMov.RoomId;
            _invRepository.Update(updatedInv);
            return invMov;
        }
        public InventoryMoving Create(InventoryMoving invMov)
        {
            return _invMovRepository.Create(invMov);
        }

        public IEnumerable<InventoryMoving> GetAll()
        {
            return _invMovRepository.GetAll();
        }
        public InventoryMoving GetById(int id)
        {
            return _invMovRepository.GetById(id);
        }
        public bool Delete(int id)
        {
            return _invMovRepository.Delete(id);
        }

    }
}
