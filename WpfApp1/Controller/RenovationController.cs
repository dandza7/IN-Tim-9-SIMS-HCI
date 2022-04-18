using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
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

        public void PrintAll()
        {
            _renovationService.PrintAll();
        }

        public List<String> GetBegginigns(int roomId)
        {
            return _renovationService.GetBeginnings(roomId);
        }
        public List<String> GetEndings(string beginning, int roomId)
        {
            return _renovationService.GetEndings(beginning, roomId);
        }
    }
}
