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
            List<DynamicEquipmentRequest> dynReqsForUpdating = _dynamicEquipmentRequestRepository.GetAllForUpdating();

            foreach (DynamicEquipmentRequest dynreq in dynReqsForUpdating)
            {
                Inventory inventoryForUpdating = _inventoryRepository.GetByName(dynreq.Name);

                    if (inventoryForUpdating != null)
                    {
                    _inventoryRepository.Update(new Inventory(inventoryForUpdating.Id, 0, dynreq.Name, "D", inventoryForUpdating.Amount + dynreq.Amount));
                    }
                    else
                    {
                        Inventory newDynamicEquipment = new Inventory(0, dynreq.Name, "D", dynreq.Amount);
                    _inventoryRepository.Create(newDynamicEquipment);
                    }
                _dynamicEquipmentRequestRepository.Delete(dynreq.Id);

            }
            return dynReqsForUpdating;
        }

    }
}
