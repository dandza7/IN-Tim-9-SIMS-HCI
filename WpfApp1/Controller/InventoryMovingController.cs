using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class InventoryMovingController
    {
        private readonly InventoryMovingService _invMovService;

        public InventoryMovingController(InventoryMovingService invMovService)
        {
            _invMovService = invMovService;
        }
        public InventoryMoving NewMoving(InventoryMoving invMov)
        {
            if(DateTime.Compare(invMov.MovingDate, DateTime.Today) == 0)
            {
                return _invMovService.MoveToday(invMov);
            } 
            else
            {
                return _invMovService.Create(invMov);
            }
        }
    }
}
