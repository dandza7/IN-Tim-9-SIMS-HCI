using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository.Interface;

namespace WpfApp1.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private const string NOT_FOUND_ERROR = "Room with {0}:{1} can not be found!";
        private string _path;
        private string _delimiter;

        public RoomRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
        }

        public Room GetById(int id)
        {
            List<Room> rooms = File.ReadAllLines(_path)
                .Select(ConvertCsvFormatToRoom)
                .ToList();
            foreach (Room room in rooms)
            {
                if (room.Id == id)
                    return room;
            }
            return null;
        }

        public IEnumerable<Room> GetAll()
        {
            return File.ReadAllLines(_path)
                .Select(ConvertCsvFormatToRoom);
        }
        public IEnumerable<Room> GetAllByType(string type) {
            List<Room> rooms = File.ReadAllLines(_path)
                .Select(ConvertCsvFormatToRoom)
                .ToList();
            List<Room> roomsByType = new List<Room>();
            foreach (Room room in rooms)
            {
                if (room.Type == type)
                    roomsByType.Add(room);
            }
            return roomsByType;
        }
    
        private int GetMaxId(List<Room> rooms)
        {
            return rooms.Count() == 0 ? 0 : rooms.Max(room => room.Id);
        }

        public Room Create(Room room)
        {
            room.Id = GetMaxId(GetAll().ToList()) + 1;
            AppendLineToFile(_path, ConvertRoomToCsvFormat(room));
            return room;
        }

        private Room ConvertCsvFormatToRoom(string roomCsvFormat)
        {
            var tokens = roomCsvFormat.Split(_delimiter.ToCharArray());
            return new Room(
                int.Parse(tokens[0]),
                tokens[1],
                tokens[2],
                bool.Parse(tokens[3])
                );
        }

        private string ConvertRoomToCsvFormat(Room room)
        {
            return string.Join(_delimiter,
                room.Id,
                room.Nametag,
                room.Type,
                room.IsActive
                );
        }

        private void AppendLineToFile(String path, String line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }

        public bool Delete(int id)
        {
            List<Room> rooms = GetAll().ToList();
            List<string> newFile = new List<string>();
            bool isDeleted = false;
            foreach (Room r in rooms)
            {
                if (r.Id != id)
                {
                    newFile.Add(ConvertRoomToCsvFormat(r));
                    isDeleted = true;
                }
            }
            File.WriteAllLines(_path, newFile);
            return isDeleted;
        }

        public Room Update(Room room)
        {
            List<Room> rooms = GetAll().ToList();
            List<string> newFile = new List<string>();
            foreach (Room r in rooms)
            {

                if (r.Id == room.Id)
                {
                    r.Type = room.Type;
                    r.Nametag = room.Nametag;
                    r.IsActive = room.IsActive;

                }
                newFile.Add(ConvertRoomToCsvFormat(r));
            }
            File.WriteAllLines(_path, newFile);
            return room;
        }

    }
}
