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
        private readonly MedicalRecordRepository _medicalRecordRepo;
        private readonly DrugRepository _drugRepo;

        public TherapyService(TherapyRepository therapyRepo, MedicalRecordRepository medicalRecordRepo, DrugRepository drugRepo)
        {
            _therapyRepo = therapyRepo;
            _medicalRecordRepo = medicalRecordRepo;
            _drugRepo = drugRepo;
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

        public List<Therapy> GetPatientsTherapies(int patientId)
        {
            MedicalRecord medicalRecord = _medicalRecordRepo.GetPatientsMedicalRecord(patientId);
            List<Therapy> patientsTherapies = _therapyRepo.GetPatientsTherapies(medicalRecord.Id).ToList();

            return patientsTherapies;
        }

        public List<Drug> GetPatientsDrugs(int patientId)
        {
            List<Drug> drugs = new List<Drug>();
            List<Therapy> patientsTherapies = GetPatientsTherapies(patientId);

            patientsTherapies.ForEach(therapy => drugs.Add(_drugRepo.GetById(therapy.DrugId)));

            return drugs;
        }
    }
}
