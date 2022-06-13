using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class TherapyController
    {
        private readonly TherapyService _therapyService;

        public TherapyController(TherapyService therapyService)
        {
            _therapyService = therapyService;
        }

        public IEnumerable<Therapy> GetAll()
        {
            return _therapyService.GetAll();
        }

        public Therapy Create(Therapy therapy)
        {
            return _therapyService.Create(therapy);
        }

        public Therapy Update(Therapy therapy)
        {
            return _therapyService.Update(therapy);
        }

        public bool Delete(int id)
        {
            return _therapyService.Delete(id);
        }

        public IEnumerable<Therapy> GetByMedicalRecordId(int medicalRecordId)
        {
            return _therapyService.GetByMedicalRecordId(medicalRecordId);
        }

        public IEnumerable<Therapy> GetPatientsTherapies(int patientId)
        {
            return _therapyService.GetPatientsTherapies(patientId);
        }

        public IEnumerable<Drug> GetPatientsDrugs(int patientId)
        {
            return _therapyService.GetPatientsDrugs(patientId);
        }
    }
}
