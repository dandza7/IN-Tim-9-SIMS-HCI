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
        private readonly DynamicEquipmentRequestService _requestService;

        public DynamicEquipmentRequestController(DynamicEquipmentRequestService requestService)
        {
            _requestService = requestService;
        }
        public DynamicEquipmentRequest Create(DynamicEquipmentRequest request)
        {
                return _requestService.Create(request);

        }
        public IEnumerable<DynamicEquipmentRequest> GetAll()
        {
            return _requestService.GetAll();
        }
        public bool Delete(int id)
        {
            return _requestService.Delete(id);
        }
        public List<DynamicEquipmentRequest> UpdateDynamicEquipment()
        {
            return _requestService.UpdateDynamicEquipment();
        }
    }
}
