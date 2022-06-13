using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Model.Preview;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class RenovationController
    {
        public RenovationService _renovationService;

        public RenovationController(RenovationService renovationService)
        {
            _renovationService = renovationService;
        }

        public Renovation Create(Renovation renovation)
        {
            return _renovationService.Create(renovation);
        }

        /*public void PrintAll()
        {
            _renovationService.PrintAll();
        }*/


        public List<String> GetDaysAvailableForRenovation(List<int> ids, string beginning = "")
        {
            return _renovationService.GetDaysAvailableForRenovation(ids, beginning);
        }
        public List<BusynessPreview> GetBusynessPreview()
        {
            return _renovationService.GetBusynessPreview();
        }
    }
}
