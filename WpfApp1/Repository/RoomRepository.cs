using System;
using System.Collections.Generic;
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

        public RoomRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
        }

        public Room Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Room> GetAll()
        {
            return File.ReadAllLines(_path)
                .Select(ConvertCsvFormatToRoom)
                .ToList();
        }

        public int GetMaxId(List<Room> rooms)
        {
            throw new NotImplementedException();
        }

        public int Create(Room room)
        {
            throw new NotImplementedException();
        }

        public Room ConvertCsvFormatToRoom(string roomCsvFormat)
        {
            var tokens = roomCsvFormat.Split(_delimiter.ToCharArray());
            return new Room(
                int.Parse(tokens[0]),
                tokens[1],
                int.Parse(tokens[2]));
        }

        public string ConvertRoomToCsvFormat(Room room)
        {
            throw new NotImplementedException();
        }

        public void AppendLineToFile(String path, String line)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Room Update(Room room)
        {
            throw new NotImplementedException();
        }

    }
}
