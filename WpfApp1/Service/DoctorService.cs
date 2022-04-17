using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class DoctorService
    {
        private readonly DoctorRepository _doctorRepo;

        public DoctorService(DoctorRepository doctorRepo)
        {
            _doctorRepo = doctorRepo;
        }
        internal IEnumerable<Doctor> GetAll()
        {
            var doctors = _doctorRepo.GetAll();
            return doctors;
        }
        public Doctor GetById(int id)
        {
            return _doctorRepo.GetById(id);
        }

        public Doctor GetByUsername(string username)
        {
            return _doctorRepo.GetByUsername(username);
        }

        public Doctor Create(Doctor doctor)
        {
            return _doctorRepo.Create(doctor);
        }
        public Doctor Update(Doctor doctor)
        {
            return _doctorRepo.Update(doctor);
        }
        public bool Delete(int id)
        {
            return _doctorRepo.Delete(id);
        }
        public List<Doctor> UpdateDoctors()
        {
            return _doctorRepo.UpdateDoctors();
        }
    }
}
