using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class TherapyService
    {
        private readonly TherapyRepository _therapyRepo;

        public TherapyService(TherapyRepository therapyRepo)
        {
            _therapyRepo = therapyRepo;
        }

        public IEnumerable<Therapy> GetAll()
        {
            return _therapyRepo.GetAll();
        }

        public Therapy Create(Therapy therapy)
        {
            return _therapyRepo.Create(therapy);
        }

        public Therapy Update(Therapy therapy)
        {
            return _therapyRepo.Update(therapy);
        }

        public bool Delete(int id)
        {
            return _therapyRepo.Delete(id);
        }

        public IEnumerable<Therapy> GetByMedicalRecordId(int medicalRecordId)
        {
            return _therapyRepo.GetPatientsTherapies(medicalRecordId);
        }
    }
}
