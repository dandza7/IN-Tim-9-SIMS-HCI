using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class MedicalRecordController
    {
        private readonly MedicalRecordService _medicalRecordService;

        public MedicalRecordController(MedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        public IEnumerable<MedicalRecord> GetAll()
        {
            return _medicalRecordService.GetAll();
        }

        public MedicalRecord GetById(int id)
        {
            return _medicalRecordService.GetById(id);
        }
        public MedicalRecord GetByPatientId(int patientId)
        {
            return _medicalRecordService.GetPatientsMedicalRecord(patientId);
        }
        public MedicalRecord Create(MedicalRecord medicalRecord)
        {
            return _medicalRecordService.Create(medicalRecord);
        }

        public MedicalRecord Update(MedicalRecord medicalRecord)
        {
            return _medicalRecordService.Update(medicalRecord);
        }

        public bool Delete(int id)
        {
            return _medicalRecordService.Delete(id);
        }
    }
}
