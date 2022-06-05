using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class DynamicEquipmentRequestService
    {
        public readonly DynamicEquipmentRequestRepository _equipmentRequestRepo;
        public readonly InventoryRepository _inventoryRepository;

        public DynamicEquipmentRequestService(DynamicEquipmentRequestRepository requestRepo, InventoryRepository inventoryRepo)
        {
            _equipmentRequestRepo = requestRepo;
            _inventoryRepository = inventoryRepo;
        }
        public DynamicEquipmentRequest Create(DynamicEquipmentRequest request)
        {
            return _equipmentRequestRepo.Create(request);

        }
        public IEnumerable<DynamicEquipmentRequest> GetAll()
        {
            return _equipmentRequestRepo.GetAll();
        }

        public bool Delete(int id)
        {
            return _equipmentRequestRepo.Delete(id);
        }
        public List<DynamicEquipmentRequest> UpdateDynamicEquipment()
        {
            List<DynamicEquipmentRequest> requests = _equipmentRequestRepo.GetAllForUpdating();

            foreach (DynamicEquipmentRequest request in requests)
            {
                Inventory inventory = _inventoryRepository.GetByName(request.Name);

                    if (inventory != null)
                    {
                        _inventoryRepository.Update(new Inventory(inventory.Id, 0, request.Name, "D", inventory.Amount + request.Amount));
                    }
                    else
                    {
                        Inventory newDynamicEquipment = new Inventory(0, request.Name, "D", request.Amount);
                        _inventoryRepository.Create(newDynamicEquipment);
                    }

                _equipmentRequestRepo.Delete(request.Id);

            }
            return requests;
        }

    }
}
