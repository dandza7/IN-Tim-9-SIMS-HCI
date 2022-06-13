using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Model.Preview;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class DoctorService
    {
        private readonly DoctorRepository _doctorRepo;
        private readonly UserRepository _userRepo;
        public DoctorService(UserRepository userRepo, DoctorRepository doctorRepo)
        {
            _doctorRepo = doctorRepo;
            _userRepo = userRepo;
        }
        internal IEnumerable<Doctor> GetAll()
        {
            var doctors = _doctorRepo.GetAll();
            return doctors;
        }
        internal IEnumerable<DoctorPreview> GetAllPreviews()
        {
            var doctors = _userRepo.GetAllDoctors();
            List<DoctorPreview> retVal = new List<DoctorPreview>();
            foreach(User doc in doctors)
            {
                retVal.Add(new DoctorPreview(doc.Id, doc.Name, doc.Surname));
            }
            return retVal;
        }
        public Doctor GetById(int id)
        {
            return _doctorRepo.GetById(id);
        }

        public Doctor GetByUsername(string username)
        {
            User user = _userRepo.GetByUsername(username);
            return _doctorRepo.GetById(user.Id);
        }

        public IEnumerable<Doctor> GetAllGeneralPracticioners()
        {
            return _doctorRepo.GetAllGeneralPracticioners();
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
        public void MoveDoctorsToMainOffice(int roomId)
        {
            List<Doctor> doctors = this._doctorRepo.GetAll().ToList();
            foreach (Doctor doctor in doctors)
            {
                if (doctor.RoomId == roomId)
                {
                    doctor.RoomId = 2;
                    _doctorRepo.Update(doctor);
                }

            }
        }
    }
}
