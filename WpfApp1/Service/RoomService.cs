using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class RoomService
    {
        public readonly RoomRepository _roomRepository;

        public RoomService(RoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }
        public List<Room> GetAll()
        {
            return _roomRepository.GetAll();
        }

        public Room Create(Room room)
        {
            return _roomRepository.Create(room);
        }

        public Room Update(Room room)
        {
            return _roomRepository.Update(room);
        }

        public bool Delete(int id)
        {
            return _roomRepository.Delete(id);
        }
        public int GetIdByNametag(string nametag)
        {
            List<Room> rooms = _roomRepository.GetAll();
            foreach(Room room in rooms)
            {
                if (room.Nametag.Equals(nametag))
                {
                    return room.Id;
                }
            }
            return -1;
        }
    }
}
