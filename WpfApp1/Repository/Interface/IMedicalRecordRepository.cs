using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository.Interface
{
    public interface IMedicalRecordRepository
    {
        IEnumerable<MedicalRecord> GetAll();
        MedicalRecord GetById(int id);

        MedicalRecord GetPatientsMedicalRecord(int patientId);

        MedicalRecord Create(MedicalRecord medicalRecord);

        MedicalRecord Update(MedicalRecord medicalRecord);

        bool Delete(int id);

    }
}
