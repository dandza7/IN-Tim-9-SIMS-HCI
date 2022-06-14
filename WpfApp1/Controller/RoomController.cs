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
    public class RoomController
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService service)
        {
            _roomService = service;
        }

        public List<Room> GetAll()
        {
            return _roomService.GetAll().ToList();
        }
        public List<Room> GetAllByType(string type)
        {
            return _roomService.GetAllByType(type).ToList();
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
        public Room GetByNametag(string nametag)
        {
            return _roomService.GetByNametag(nametag);
        }
        public List<string> GetEditableNametags()
        {
            return _roomService.GetEditableNametags().ToList();
        }

    }
}
