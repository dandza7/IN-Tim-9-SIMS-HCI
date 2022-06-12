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
    public class AllergyRepository : IAllergyRepository
    {
        private string _path;
        private string _delimiter;

        public AllergyRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
        }
        private int GetMaxId(IEnumerable<Allergy> allergies)
        {
            return allergies.Count() == 0 ? 0 : allergies.Max(therapy => therapy.Id);
        }

        public IEnumerable<Allergy> GetAll()
        {
            List<string> lines = File.ReadAllLines(_path).ToList();
            List<Allergy> allergies = new List<Allergy>();
            foreach (string line in lines)
            {
                if (line == "") continue;
                allergies.Add(ConvertCSVFormatToAllergy(line));
            }
            return allergies;
        }
        public IEnumerable<Allergy> GetPatientsAllergies(int medicalRecordId)
        {
            List<Allergy> patientsAllergies = new List<Allergy>();
            List<Allergy> allergies = GetAll().ToList();
            allergies.ForEach(allergy =>
            {
                if (allergy.MedicalRecordId == medicalRecordId)
                {
                    patientsAllergies.Add(allergy);
                }
            });

            return patientsAllergies;
        }
        public Allergy GetById(int id)
        {
            List<Allergy> allergies = GetAll().ToList();
            return allergies.FirstOrDefault(allergy => allergy.Id == id);
        }

        public Allergy Create(Allergy allergy)
        {
            int maxId = GetMaxId(GetAll());
            allergy.Id = ++maxId;
            AppendLineToFile(_path, ConvertAllergyToCSVFormat(allergy));
            return allergy;
        }
        public bool Delete(int allergyId)
        {
            List<Allergy> allergies = GetAll().ToList();
            List<string> newFile = new List<string>();
            bool isDeleted = false;
            foreach (Allergy a in allergies)
            {
                if (a.Id != allergyId)
                {
                    newFile.Add(ConvertAllergyToCSVFormat(a));
                    isDeleted = true;
                }
            }
            File.WriteAllLines(_path, newFile);
            return isDeleted;
        }
        private Allergy ConvertCSVFormatToAllergy(string allergyCSVFormat)
        {
            var tokens = allergyCSVFormat.Split(_delimiter.ToCharArray());
            return new Allergy(int.Parse(tokens[0]),
                int.Parse(tokens[1]),
                tokens[2],
                int.Parse(tokens[3])
                );
        }

        private string ConvertAllergyToCSVFormat(Allergy allergy)
        {
            return string.Join(_delimiter,
                allergy.Id,
                allergy.MedicalRecordId,
                allergy.AllergyName,
                allergy.DrugId);

        }

        private void AppendLineToFile(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }
    }
}
