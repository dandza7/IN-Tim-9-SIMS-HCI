using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using static WpfApp1.Model.Doctor;

namespace WpfApp1.Repository.Interface
{
    public interface IDoctorRepository
    {
        IEnumerable<Doctor> GetAll();
        IEnumerable<Doctor> GetAllGeneralPracticioners();
        IEnumerable<Doctor> GetAllDoctorsBySpecialization(SpecType spec);
        Doctor Create(Doctor doctor);
        Doctor Update(Doctor doctor);
        void UpdateAll(List<Doctor> doctors);
        bool Delete(int doctorId);
        Doctor GetById(int doctorId);
    }
}
