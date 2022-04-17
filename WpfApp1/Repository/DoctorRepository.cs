using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using static WpfApp1.Model.Doctor;
using static WpfApp1.Model.User;

namespace WpfApp1.Repository
{
    public class DoctorRepository
    {
        private string _path;
        private string _delimiter;
        private int _patientMaxId;

        public DoctorRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
            _patientMaxId = GetMaxId(GetAll());
        }

        private int GetMaxId(IEnumerable<Doctor> doctors)
        {
            return doctors.Count() == 0 ? 0 : doctors.Max(transaction => transaction.Id);
        }

        public IEnumerable<Doctor> GetAll()
        {
            List<string> lines = File.ReadAllLines(_path).ToList();
            List<Doctor> doctors = new List<Doctor>();
            foreach (string line in lines)
            {
                if (line == "") continue;
                doctors.Add(ConvertCSVFormatToDoctor(line));
            }
            return doctors;
        }
        public Doctor Create(Doctor doctor)
        {
            doctor.Id = ++_patientMaxId;
            AppendLineToFile(_path, ConvertDoctorToCSVFormat(doctor));
            return doctor;
        }
        public Doctor Update(Doctor doctor)
        {
            List<Doctor> doctors = GetAll().ToList();
            List<string> newFile = new List<string>();
            foreach (Doctor d in doctors)
            {

                if (d.Id == doctor.Id)
                {
                    d.Name = doctor.Name;
                    d.Surname = doctor.Surname;
                    d.Username = doctor.Username;
                    d.Password = doctor.Password;
                    d.PhoneNumber = doctor.PhoneNumber;
                    d.Jmbg = doctor.Jmbg;
                    d.Role = doctor.Role;
                    d.Specialization = doctor.Specialization;
                    d.IsAvailable = doctor.IsAvailable;
                }
                newFile.Add(ConvertDoctorToCSVFormat(d));
            }
            File.WriteAllLines(_path, newFile);
            return doctor;
        }
        public bool Delete(int doctorId)
        {
            List<Doctor> doctors = GetAll().ToList();
            List<string> newFile = new List<string>();
            bool isDeleted = false;
            foreach (Doctor d in doctors)
            {
                if (d.Id != doctorId)
                {
                    newFile.Add(ConvertDoctorToCSVFormat(d));
                    isDeleted = true;
                }
            }
            File.WriteAllLines(_path, newFile);
            return isDeleted;
        }
        public Doctor GetById(int doctorId)
        {
            List<Doctor> doctors = GetAll().ToList();
            foreach (Doctor d in doctors)
            {
                if (d.Id == doctorId)
                {
                    return d;
                }

            }
            return null;
        }

        public Doctor GetByUsername(string username)
        {
            List<Doctor> doctors = GetAll().ToList();
            foreach (Doctor d in doctors)
            {
                if (d.Username == username)
                {
                    return d;
                }

            }
            return null;
        }

        private Doctor ConvertCSVFormatToDoctor(string doctorCSVFormat)
        {
            var tokens = doctorCSVFormat.Split(_delimiter.ToCharArray());
            RoleType role;
            Enum.TryParse(tokens[7], true, out role);
            SpecType spec;
            Enum.TryParse(tokens[8], true, out spec);
            return new Doctor(int.Parse(tokens[0]), tokens[1], tokens[2], tokens[3], tokens[4], tokens[5], tokens[6], role, spec, bool.Parse(tokens[9]));
        }
        private string ConvertDoctorToCSVFormat(Doctor doctor)
        {
            return string.Join(_delimiter,
               doctor.Id,
               doctor.Name.ToString(),
               doctor.Surname.ToString(),
               doctor.Username.ToString(),
               doctor.Password.ToString(),
               doctor.PhoneNumber.ToString(),
               doctor.Jmbg.ToString(),
               doctor.Role.ToString(),
               doctor.Specialization.ToString(),
               doctor.IsAvailable.ToString()); 
        }
        private void AppendLineToFile(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }
        public List<Doctor> UpdateDoctors()
        {
            return GetAll().ToList();
        }
    }
}
