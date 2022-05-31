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
        public readonly DynamicEquipmentRequestRepository _dynamicEquipmentRequestRepository;
        public readonly InventoryRepository _inventoryRepository;

        public DynamicEquipmentRequestService(DynamicEquipmentRequestRepository dynReqRepository, InventoryRepository eqRepository)
        {
            _dynamicEquipmentRequestRepository = dynReqRepository;
            _inventoryRepository = eqRepository;
        }
        public DynamicEquipmentRequest Create(DynamicEquipmentRequest dynReq)
        {
            return _dynamicEquipmentRequestRepository.Create(dynReq);

        }
        public IEnumerable<DynamicEquipmentRequest> GetAll()
        {
            return _dynamicEquipmentRequestRepository.GetAll();
        }

        public bool Delete(int id)
        {
            return _dynamicEquipmentRequestRepository.Delete(id);
        }
        public List<DynamicEquipmentRequest> UpdateDynamicEquipment()
        {
            List<DynamicEquipmentRequest> requests = _dynamicEquipmentRequestRepository.GetAllForUpdating();

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

                _dynamicEquipmentRequestRepository.Delete(request.Id);

            }
            return requests;
        }

    }
}
