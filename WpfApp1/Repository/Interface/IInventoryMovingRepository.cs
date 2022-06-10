using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository.Interface
{
    public interface IInventoryMovingRepository
    {
        IEnumerable<InventoryMoving> GetAll();
        InventoryMoving GetById(int id);
        InventoryMoving Create(InventoryMoving invMov);
        bool Delete(int id);
    }
}
