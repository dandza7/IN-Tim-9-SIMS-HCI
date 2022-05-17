using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class DynamicEquipmentRequestController
    {
        private readonly DynamicEquipmentRequestService _dynReqService;

        public DynamicEquipmentRequestController(DynamicEquipmentRequestService dynReqService)
        {
            _dynReqService = dynReqService;
        }
        public DynamicEquipmentRequest Create(DynamicEquipmentRequest dynReq)
        {
                return _dynReqService.Create(dynReq);

        }
        public IEnumerable<DynamicEquipmentRequest> GetAll()
        {
            return _dynReqService.GetAll();
        }
        public bool Delete(int id)
        {
            return _dynReqService.Delete(id);
        }
        public List<DynamicEquipmentRequest> UpdateDynamicEquipment()
        {
            return _dynReqService.UpdateDynamicEquipment();
        }
    }
}
