using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class PatientController
    {
        private readonly PatientService _patientService;

        public PatientController(PatientService patientService)
        {
            _patientService = patientService;
        }

        public IEnumerable<Patient> GetAll()
        {
            return _patientService.GetAll();
        }

        public IEnumerable<Therapy> GetPatientsTherapies(int patientId)
        {
            return _patientService.GetPatientsTherapies(patientId);
        }

        public IEnumerable<Drug> GetPatientsDrugs(int patientId)
        {
            return _patientService.GetPatientsDrugs(patientId);
        }
        public Patient GetByUsername(string username)
        {
            return _patientService.GetByUsername(username);
        }
        public Patient Create(Patient patient)
        {
            return _patientService.Create(patient);
        }
        public Patient Update(Patient patient)
        {
            return _patientService.Update(patient);
        }
        public bool Delete(int patientId)
        {
            return _patientService.Delete(patientId);
        }
        public Patient GetById(int patientId)
        {
            return _patientService.GetById(patientId);
        }
    }
}
