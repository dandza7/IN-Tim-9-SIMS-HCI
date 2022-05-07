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
        public readonly DynamicEquipmentRequestRepository _dynReqRepository;
        public readonly InventoryRepository _invRepository;

        public DynamicEquipmentRequestService(DynamicEquipmentRequestRepository dynReqRepository, InventoryRepository eqRepository)
        {
            _dynReqRepository = dynReqRepository;
            _invRepository = eqRepository;
        }
        public DynamicEquipmentRequest Create(DynamicEquipmentRequest dynReq)
        {
            return _dynReqRepository.Create(dynReq);

        }
        public IEnumerable<DynamicEquipmentRequest> GetAll()
        {
            return _dynReqRepository.GetAll();
        }
        public bool Delete(int id)
        {
            return _dynReqRepository.Delete(id);
        }
    }
}
