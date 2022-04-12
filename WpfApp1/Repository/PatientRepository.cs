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
                    p.JMBG = patient.JMBG;
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
        public Patient Find(int patientId)
        {
            List<Patient> patients = GetAll().ToList();
            List<string> newFile = new List<string>();
            bool isFound = false;
            foreach (Patient p in patients)
            {
                if (p.Id == patientId)
                {
                    isFound = true;
                    return p;
                }
                
            }

              return null;
            
        }
        private Patient ConvertCSVFormatToPatient(string patientCSVFormat)
        {
            var tokens = patientCSVFormat.Split(_delimiter.ToCharArray());

            return new Patient(int.Parse(tokens[0]), tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], tokens[6], tokens[7]);
        }
        private string ConvertPatientToCSVFormat(Patient patient)
        {
            return string.Join(_delimiter,
                patient.Id,
                patient.Name.ToString(),
                patient.Surname.ToString(),
                patient.JMBG.ToString(),
                patient.Username.ToString(),
                patient.Password.ToString(),
                patient.Email.ToString(),
                patient.PhoneNumber.ToString());
        }

        private void AppendLineToFile(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }
    }
}
