using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class InventoryMovingService
    {
        public readonly InventoryMovingRepository _invMovRepository;

        public InventoryMovingService(InventoryMovingRepository invMovRepository)
        {
            _invMovRepository = invMovRepository;
        }
    }
}
