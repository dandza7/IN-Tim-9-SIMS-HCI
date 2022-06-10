using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository.Interface;

namespace WpfApp1.Repository
{
    public class RenovationRepository : IRenovationRepository
    {
        private const string NOT_FOUND_ERROR = "Renovation with {0}:{1} can not be found!";
        private string _path;
        private string _delimiter;

        public RenovationRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
        }

        public Renovation GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Renovation> GetAll()
        {
            return File.ReadAllLines(_path)
                .Select(ConvertCsvFormatToRenovation)
                .ToList();
        }

        private IEnumerable<Renovation> GetAllRenovationsForRoom(int roomId)
        {
            List<Renovation> allRenovations = GetAll().ToList();
            List<Renovation> roomRenovations = new List<Renovation>();

            foreach (var renovation in allRenovations)
            {
                if (renovation.RoomsIds.Contains(roomId))
                {
                    roomRenovations.Add(renovation);
                }
            }

            return roomRenovations;
        }

        public bool IsRoomAvailable(int roomId, DateTime startOfInterval, DateTime endOfinterval)
        {
            List<Renovation> roomRenovations = GetAllRenovationsForRoom(roomId).ToList();

            foreach(Renovation renovation in roomRenovations)
            {
                // Rezervacije mogu biti samo u budućnosti tako da renoviranja u prošlosti nisu relevantna
                if (renovation.Ending < DateTime.Now) continue;
                // Može samo ovaj if jer će interval biti uvijek u neko kršteno doba
                if (startOfInterval > renovation.Beginning && endOfinterval < renovation.Ending) return false;
            }
            return true;
        }

        private int GetMaxId(List<Renovation> renovations)
        {
            return renovations.Count() == 0 ? 0 : renovations.Max(renovation => renovation.Id);
        }

        public Renovation Create(Renovation renovation)
        {
            renovation.Id = GetMaxId(GetAll().ToList()) + 1;
            AppendLineToFile(_path, ConvertRenovationToCsvFormat(renovation));
            return renovation;
        }

        private Renovation ConvertCsvFormatToRenovation(string renovationCsvFormat)
        {
            var tokens = renovationCsvFormat.Split(_delimiter.ToCharArray());
            string format = "dd/MM/yyyy H:mm:ss";
            var roomsIds = tokens[1].Split("|".ToCharArray());
            List<int> intIds = new List<int>();
            foreach(var roomId in roomsIds)
            {
                intIds.Add(int.Parse(roomId));
            }
            return new Renovation(
                int.Parse(tokens[0]),
                intIds,
                tokens[2],
                DateTime.ParseExact(tokens[3], format,
                CultureInfo.InvariantCulture),
                DateTime.ParseExact(tokens[4], format,
                CultureInfo.InvariantCulture),
                tokens[5]);
        }

        private string ConvertRenovationToCsvFormat(Renovation renovation)
        {
            string ids = "";
            int count = 0;
            //Console.WriteLine("BROJ SOBA: {0}", renovation.RoomsIds.Count());
            foreach(int id in renovation.RoomsIds)
            {
                count++;
                ids += id.ToString();
                if(count < renovation.RoomsIds.Count)
                {
                    ids += "|";
                }
            }
            //Console.WriteLine("A: {0}", ids);
            return string.Join(_delimiter,
                renovation.Id,
                ids,
                renovation.Description,
                renovation.Beginning,
                renovation.Ending,
                renovation.Type);
        }

        private void AppendLineToFile(String path, String line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }

        public bool Delete(int id)
        {
            List<Renovation> renovations = GetAll().ToList();
            List<string> newFile = new List<string>();
            bool isDeleted = false;
            foreach (Renovation r in renovations)
            {
                if (r.Id != id)
                {
                    newFile.Add(ConvertRenovationToCsvFormat(r));
                    isDeleted = true;
                }
            }
            File.WriteAllLines(_path, newFile);
            return isDeleted;
        }


    }
}
