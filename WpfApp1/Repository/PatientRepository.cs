using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository
{
    public class PatientRepository
    {
        private string _path;
        private string _delimiter;
        private int _patientMaxId;

        public PatientRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
            _patientMaxId = (int)GetMaxId(GetAll());
        }

        private long GetMaxId(IEnumerable<Patient> patients)
        {
            return patients.Count() == 0 ? 0 : patients.Max(transaction => transaction.Id);
        }

        public IEnumerable<Patient> GetAll()
        {
            return File.ReadAllLines(_path)
                .Select(ConvertCSVFormatToPatient)
                .ToList();
        }
        public Patient Create(Patient patient)
        {
            patient.Id = ++_patientMaxId;
            AppendLineToFile(_path, ConvertPatientToCSVFormat(patient));
            return patient;
        }
        public Patient Update(Patient patient)
        {
            List<Patient> patients = GetAll().ToList();
            List<string> newFile = new List<string>();
            foreach (Patient p in patients)
            {
                if (p.Id == patient.Id)
                {
                    p.Name = patient.Name;
                    p.Surname = patient.Surname;
                    p.Jmbg = patient.Jmbg;
                    p.Username = patient.Username;
                    p.Password = patient.Password;
                    p.Email = patient.Email;
                    p.PhoneNumber = patient.PhoneNumber;
                }
                newFile.Add(ConvertPatientToCSVFormat(p));
            }
            File.WriteAllLines(_path, newFile);
            return patient;
        }

        public bool Delete(int patientId)
        {
            List<Patient> patients = GetAll().ToList();
            List<string> newFile = new List<string>();
            bool isDeleted = false;
            foreach (Patient p in patients)
            {
                if (p.Id != patientId)
                {
                    newFile.Add(ConvertPatientToCSVFormat(p));
                    isDeleted = true;
                }
            }
            File.WriteAllLines(_path, newFile);
            return isDeleted;
        }

        public Patient GetById(int patientId)
        {
            List<Patient> patients = GetAll().ToList();
            foreach (Patient p in patients)
            {
                if (p.Id == patientId)
                {
                    return p;
                }
                
            }

              return null;
            
        }

        private Patient ConvertCSVFormatToPatient(string patientCSVFormat)
        {
            var tokens = patientCSVFormat.Split(_delimiter.ToCharArray());
            List<string> stringTherapyIds = tokens[8].Split("|".ToCharArray()).ToList();
            List<int> therapyIds = ConvertStringListToIntList(stringTherapyIds);
            return new Patient(int.Parse(tokens[0]), 
                tokens[1], 
                tokens[2], 
                tokens[3], 
                tokens[4], 
                tokens[5], 
                tokens[6], 
                tokens[7],
                therapyIds);
        }

        private List<int> ConvertStringListToIntList(List<string> stringList)
        {
            List<string> strings = stringList.ToList();
            List<int> notificationIds = new List<int>();
            foreach(string str in strings)
            {
                if (str == "") continue;
                notificationIds.Add(int.Parse(str));
            }
            return notificationIds;
        }

        private string ConvertPatientToCSVFormat(Patient patient)
        {
            return string.Join(_delimiter,
                patient.Id,
                patient.Name.ToString(),
                patient.Surname.ToString(),
                patient.Jmbg.ToString(),
                patient.Username.ToString(),
                patient.Password.ToString(),
                patient.PhoneNumber.ToString(),
                patient.Email.ToString(),
                ConvertIntListToCSVFormat(patient.TherapyIds));
        }

        private string ConvertIntListToCSVFormat(List<int> therapyIds)
        {
            return string.Join("|", therapyIds.Select(id => id.ToString()).ToArray());
        }

        private void AppendLineToFile(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }
    }
}
