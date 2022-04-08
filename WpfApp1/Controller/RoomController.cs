using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class RoomController
    {
        private readonly RoomService _service;

        public RoomController(RoomService service)
        {
            _service = service;
        }

        public List<Room> GetAll()
        {
            return _service.GetAll();
        }
    }
}
