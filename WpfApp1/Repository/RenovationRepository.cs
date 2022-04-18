﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository
{
    public class RenovationRepository
    {
        private const string NOT_FOUND_ERROR = "Renovation with {0}:{1} can not be found!";
        private string _path;
        private string _delimiter;
        private int _renovationMaxId;

        public RenovationRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
            _renovationMaxId = GetMaxId(GetAll());
        }

        public Renovation Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Renovation> GetAll()
        {
            return File.ReadAllLines(_path)
                .Select(ConvertCsvFormatToRoom)
                .ToList();
        }

        public int GetMaxId(List<Renovation> renovations)
        {
            return renovations.Count() == 0 ? 0 : renovations.Max(renovation => renovation.Id);
        }

        public Renovation Create(Renovation renovation)
        {
            renovation.Id = ++_renovationMaxId;
            AppendLineToFile(_path, ConvertRoomToCsvFormat(renovation));
            return renovation;
        }

        public Renovation ConvertCsvFormatToRoom(string renovationCsvFormat)
        {
            var tokens = renovationCsvFormat.Split(_delimiter.ToCharArray());
            return new Renovation(
                int.Parse(tokens[0]),
                int.Parse(tokens[1]),
                tokens[2],
                DateTime.Parse(tokens[3]),
                DateTime.Parse(tokens[4]));
        }

        public string ConvertRoomToCsvFormat(Renovation renovation)
        {
            return string.Join(_delimiter,
                renovation.Id,
                renovation.RoomId,
                renovation.Description,
                renovation.Beginning,
                renovation.Ending);
        }

        public void AppendLineToFile(String path, String line)
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
                    newFile.Add(ConvertRoomToCsvFormat(r));
                    isDeleted = true;
                }
            }
            File.WriteAllLines(_path, newFile);
            return isDeleted;
        }


    }
}
