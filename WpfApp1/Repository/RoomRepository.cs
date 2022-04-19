using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository
{
    public class RoomRepository
    {
        private const string NOT_FOUND_ERROR = "Room with {0}:{1} can not be found!";
        private string _path;
        private string _delimiter;
        private int _roomMaxId;

        public RoomRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
            _roomMaxId = GetMaxId(GetAll());
        }

        public Room Get(int id)
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

        public List<Room> GetAll()
        {
            return File.ReadAllLines(_path)
                .Select(ConvertCsvFormatToRoom)
                .ToList();
        }

        public int GetMaxId(List<Room> rooms)
        {
            return rooms.Count() == 0 ? 0 : rooms.Max(room => room.Id);
        }

        public Room Create(Room room)
        {
            room.Id = ++_roomMaxId;
            AppendLineToFile(_path, ConvertRoomToCsvFormat(room));
            return room;
        }

        public Room ConvertCsvFormatToRoom(string roomCsvFormat)
        {
            var tokens = roomCsvFormat.Split(_delimiter.ToCharArray());
            return new Room(
                int.Parse(tokens[0]),
                tokens[1],
                tokens[2]);
        }

        public string ConvertRoomToCsvFormat(Room room)
        {
            return string.Join(_delimiter,
                room.Id,
                room.Nametag,
                room.Type);
        }

        public void AppendLineToFile(String path, String line)
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

                }
                newFile.Add(ConvertRoomToCsvFormat(r));
            }
            File.WriteAllLines(_path, newFile);
            return room;
        }

    }
}
