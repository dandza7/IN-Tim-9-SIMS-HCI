using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class DoctorController
    {
        private readonly DoctorService _doctorService;

        public DoctorController(DoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public IEnumerable<Doctor> GetAll()
        {
            return _doctorService.GetAll();
        }

        public Doctor GetById(int id)
        {
            return _doctorService.GetById(id);
        }

        public Doctor GetByUsername(string username)
        {
            return _doctorService.GetByUsername(username);
        }

        public Doctor Create(Doctor doctor)
        {
            return _doctorService.Create(doctor);
        }

        public Doctor Update(Doctor doctor)
        {
            return _doctorService.Update(doctor);
        }
        public bool Delete(int id)
        {
            return _doctorService.Delete(id);
        }
        public List<Doctor> UpdateDoctorss()
        {
            return _doctorService.UpdateDoctors();
        }
    }
}
