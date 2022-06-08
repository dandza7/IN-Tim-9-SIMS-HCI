using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository.Interface;

namespace WpfApp1.Repository
{
    public class DrugRepository : IDrugRepository
    {
        private string _path;
        private string _delimiter;

        public DrugRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
        }

        private int GetMaxId(IEnumerable<Drug> drugs)
        {
            return drugs.Count() == 0 ? 0 : drugs.Max(drug => drug.Id);
        }

        public IEnumerable<Drug> GetAll()
        {
            List<string> lines = File.ReadAllLines(_path).ToList();
            List<Drug> drugs = new List<Drug>();
            foreach (string line in lines)
            {
                if (line == "") continue;
                drugs.Add(ConvertCSVFormatToDrug(line));
            }
            return drugs;
        }

        public Drug GetById(int id)
        {
            return GetAll().ToList().SingleOrDefault(drug => drug.Id == id);
        }

        public Drug Create(Drug drug)
        {
            int maxId = GetMaxId(GetAll());
            drug.Id = ++maxId;
            AppendLineToFile(_path, ConvertDrugToCSVFormat(drug));
            return drug;
        }

        public Drug Update(Drug drug)
        {
            List<Drug> drugs = GetAll().ToList();
            List<string> newFile = new List<string>();
            foreach (Drug d in drugs)
            {

                if (d.Id == drug.Id)
                {
                    d.Name = drug.Name;
                    d.Info = drug.Info;
                    d.IsVerified = drug.IsVerified;
                    d.IsRejected = drug.IsRejected;
                    d.Comment = drug.Comment;
                }
                newFile.Add(ConvertDrugToCSVFormat(d));
            }
            File.WriteAllLines(_path, newFile);
            return drug;
        }

        public bool Delete(int id)
        {
            List<Drug> drugs = GetAll().ToList();
            List<string> newFile = new List<string>();
            bool isDeleted = false;
            foreach (Drug d in drugs)
            {
                if (d.Id != id)
                {
                    newFile.Add(ConvertDrugToCSVFormat(d));
                    isDeleted = true;
                }
            }
            File.WriteAllLines(_path, newFile);
            return isDeleted;
        }

        private Drug ConvertCSVFormatToDrug(string drugCSVFormat)
        {
            var tokens = drugCSVFormat.Split(_delimiter.ToCharArray());
            return new Drug(int.Parse(tokens[0]),
                tokens[1],
                tokens[2].Replace("|", "\r\n"),
                bool.Parse(tokens[3]),
                bool.Parse(tokens[4]),
                tokens[5]);
        }
        private string ConvertDrugToCSVFormat(Drug drug)
        {
            return string.Join(_delimiter,
                drug.Id,
                drug.Name,
                drug.Info.Replace("\r\n", "|"),
                drug.IsVerified.ToString(),
                drug.IsRejected.ToString(),
                drug.Comment);
        }

        private void AppendLineToFile(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }
    }
}
