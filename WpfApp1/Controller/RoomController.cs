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
        private readonly RoomService _roomService;

        public RoomController(RoomService service)
        {
            _roomService = service;
        }

        public List<Room> GetAll()
        {
            return _roomService.GetAll();
        }

        public Room GetById(int id)
        {
            return _roomService.GetById(id);
        }

        public Room Create(Room room)
        {
            return _roomService.Create(room);
        }

        public Room Update(Room room)
        {
            return _roomService.Update(room);
        }
        public bool Delete(int id)
        {
            return _roomService.Delete(id);
        }
        public int GetIdByNametag(string nametag)
        {
            return _roomService.GetIdByNametag(nametag);
        }
    }
}
