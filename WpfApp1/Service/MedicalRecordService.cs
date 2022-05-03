using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class MedicalRecordService
    {
        private readonly MedicalRecordRepository _medicalRecordRepo;
        public MedicalRecordService(MedicalRecordRepository medicalRecordRepo)
        {
            _medicalRecordRepo = medicalRecordRepo;
        }

        public IEnumerable<MedicalRecord> GetAll()
        {
            return _medicalRecordRepo.GetAll();
        }

        public MedicalRecord GetById(int id)
        {
            return _medicalRecordRepo.GetById(id);
        }

        public MedicalRecord Create(MedicalRecord medicalRecord)
        {
            return _medicalRecordRepo.Create(medicalRecord);
        }
        public MedicalRecord GetPatientsMedicalRecord(int id)
        {
            return _medicalRecordRepo.GetPatientsMedicalRecord(id);
        }
        public MedicalRecord Update(MedicalRecord medicalRecord)
        {
            return _medicalRecordRepo.Update(medicalRecord);
        }

        public bool Delete(int id)
        {
            return _medicalRecordRepo.Delete(id);
        }
    }
}
