using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository.Interface;
using static WpfApp1.Model.Doctor;
using static WpfApp1.Model.User;

namespace WpfApp1.Repository
{
    public class DoctorRepository : IDoctorRepository
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

        public IEnumerable<Doctor> GetAllGeneralPracticioners()
        {
            List<Doctor> allDoctors = GetAll().ToList();
            List<Doctor> generalPracticioners = new List<Doctor>();

            foreach (Doctor doctor in allDoctors)
            {
                if(doctor.Specialization == SpecType.generalPracticioner && doctor.IsAvailable)
                {
                    generalPracticioners.Add(doctor);
                }
            }

            return generalPracticioners;
        }
        public IEnumerable<Doctor> GetAllDoctorsBySpecialization(SpecType spec)
        {
            List<Doctor> allDoctors = GetAll().ToList();
            List<Doctor> doctors = new List<Doctor>();

            foreach (Doctor doctor in allDoctors)
            {
                if (doctor.Specialization == spec && doctor.IsAvailable)
                {
                    doctors.Add(doctor);
                }
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
        public void UpdateAll(List<Doctor> doctors)
        {
            List<string> newFile = new List<string>();
            foreach (Doctor doctor in doctors)
            {
                newFile.Add(ConvertDoctorToCSVFormat(doctor));
            }
            File.WriteAllLines(_path, newFile);
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

        private Doctor ConvertCSVFormatToDoctor(string doctorCSVFormat)
        {
            var tokens = doctorCSVFormat.Split(_delimiter.ToCharArray());
            Enum.TryParse(tokens[1], true, out SpecType spec);
            return new Doctor(int.Parse(tokens[0]), 
                spec, 
                bool.Parse(tokens[2]),
                int.Parse(tokens[3]));
        }
        private string ConvertDoctorToCSVFormat(Doctor doctor)
        {
            return string.Join(_delimiter,
               doctor.Id,
               doctor.Specialization.ToString(),
               doctor.IsAvailable.ToString(),
               doctor.RoomId.ToString());
        }
        private void AppendLineToFile(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }
    }
}
