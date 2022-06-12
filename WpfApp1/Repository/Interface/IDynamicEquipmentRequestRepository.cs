using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository.Interface
{
    public interface IDynamicEquipmentRequestRepository
    {
        DynamicEquipmentRequest GetById(int id);

        List<DynamicEquipmentRequest> GetAllForUpdating();

        List<DynamicEquipmentRequest> GetAll();

        DynamicEquipmentRequest Create(DynamicEquipmentRequest request);

        bool Delete(int id);
    }
}
