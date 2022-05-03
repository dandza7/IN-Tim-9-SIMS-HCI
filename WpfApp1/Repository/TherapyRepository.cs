using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository
{
    public class TherapyRepository
    {
        private string _path;
        private string _delimiter;

        public TherapyRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
        }

        private int GetMaxId(IEnumerable<Therapy> therapies)
        {
            return therapies.Count() == 0 ? 0 : therapies.Max(therapy => therapy.Id);
        }

        public IEnumerable<Therapy> GetAll()
        {
            List<string> lines = File.ReadAllLines(_path).ToList();
            List<Therapy> therapies = new List<Therapy>();
            foreach (string line in lines)
            {
                if (line == "") continue;
                therapies.Add(ConvertCSVFormatToTherapy(line));
            }
            return therapies;
        }

        public IEnumerable<Therapy> GetPatientsTherapies(int medicalRecordId)
        {
            List<Therapy> patientsTherapies = new List<Therapy>();
            List<Therapy> therapies = GetAll().ToList();
            therapies.ForEach(therapy =>
            {
                if (therapy.MedicalRecordId == medicalRecordId)
                {
                    patientsTherapies.Add(therapy);
                }
            });

            return patientsTherapies;
        }
        public Therapy GetById(int id)
        {
            List<Therapy> therapies = GetAll().ToList();
            return therapies.FirstOrDefault(therapy => therapy.Id == id);
        }

        public Therapy Create(Therapy therapy)
        {
            int maxId = GetMaxId(GetAll());
            therapy.Id = ++maxId;
            AppendLineToFile(_path, ConvertTherapyToCSVFormat(therapy));
            return therapy;
        }

        public Therapy Update(Therapy therapy)
        {
            List<Therapy> therapies = GetAll().ToList();
            List<string> newFile = new List<string>();
            foreach (Therapy t in therapies)
            {
                if (t.Id == therapy.Id)
                {
                    t.MedicalRecordId = therapy.MedicalRecordId;
                    t.DrugId = therapy.DrugId;
                    t.Frequency = therapy.Frequency;
                    t.Duration = therapy.Duration;
                }
                newFile.Add(ConvertTherapyToCSVFormat(t));
            }
            File.WriteAllLines(_path, newFile);
            return therapy;
        }

        public bool Delete(int id)
        {
            List<Therapy> therapies = GetAll().ToList();
            List<string> newFile = new List<string>();
            bool isDeleted = false;
            foreach (Therapy t in therapies)
            {
                if (t.Id != id)
                {
                    newFile.Add(ConvertTherapyToCSVFormat(t));
                    isDeleted = true;
                }
            }
            File.WriteAllLines(_path, newFile);
            return isDeleted;
        }

        private Therapy ConvertCSVFormatToTherapy(string therapyCSVFormat)
        {
            var tokens = therapyCSVFormat.Split(_delimiter.ToCharArray());
            return new Therapy(int.Parse(tokens[0]),
                int.Parse(tokens[1]),
                int.Parse(tokens[2]),
                float.Parse(tokens[3], CultureInfo.InvariantCulture.NumberFormat),
                int.Parse(tokens[4]));
        }

        private string ConvertTherapyToCSVFormat(Therapy therapy)
        {
            return string.Join(_delimiter,
                therapy.Id,
                therapy.MedicalRecordId,
                therapy.DrugId,
                therapy.Frequency,
                therapy.Duration);
        }

        private void AppendLineToFile(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }
    }
}
