using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Model.Preview;
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
        //DANETOVA FUNKCIJA ISPOD
        public IEnumerable<DoctorPreview> GetAllPreviews()
        {
            return _doctorService.GetAllPreviews();
        }

        public Doctor GetById(int id)
        {
            return _doctorService.GetById(id);
        }

        public Doctor GetByUsername(string username)
        {
            return _doctorService.GetByUsername(username);
        }

        public IEnumerable<Doctor> GetAllGeneralPracticioners()
        {
            return _doctorService.GetAllGeneralPracticioners();
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
    }
}
