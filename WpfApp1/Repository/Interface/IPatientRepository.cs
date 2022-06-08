using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository.Interface
{
    public interface IPatientRepository
    {
        IEnumerable<Patient> GetAll();

        Patient GetById(int id);

        Patient Create(Patient patient);

        Patient Update(Patient patient);

        bool Delete(int patientId);
    }
}
