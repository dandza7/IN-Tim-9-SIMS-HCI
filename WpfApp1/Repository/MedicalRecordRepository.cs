using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository.Interface;

namespace WpfApp1.Repository
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private string _path;
        private string _delimiter;

        public MedicalRecordRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
        }

        private int GetMaxId(IEnumerable<MedicalRecord> medicalRecords)
        {
            return medicalRecords.Count() == 0 ? 0 : medicalRecords.Max(medicalRecord => medicalRecord.Id);
        }

        public IEnumerable<MedicalRecord> GetAll()
        {
            List<string> lines = File.ReadAllLines(_path).ToList();
            List<MedicalRecord> medicalRecords = new List<MedicalRecord>();
            foreach (string line in lines)
            {
                if (line == "") continue;
                medicalRecords.Add(ConvertCSVFormatToMedicalRecord(line));
            }
            return medicalRecords;
        }

        public MedicalRecord GetById(int id)
        {
            return GetAll().ToList().SingleOrDefault(medicalRecord => medicalRecord.Id == id);
        }

        public MedicalRecord GetPatientsMedicalRecord(int patientId)
        {
            return GetAll().ToList().SingleOrDefault(medicalRecord => medicalRecord.PatientId == patientId);
        }

        public MedicalRecord Create(MedicalRecord medicalRecord)
        {
            int maxId = GetMaxId(GetAll());
            medicalRecord.Id = ++maxId;
            AppendLineToFile(_path, ConvertMedicalRecordToCSVFormat(medicalRecord));
            return medicalRecord;
        }

        public MedicalRecord Update(MedicalRecord medicalRecord)
        {
            List<MedicalRecord> medicalRecords = GetAll().ToList();
            List<string> newFile = new List<string>();
            foreach (MedicalRecord md in medicalRecords)
            {

                if (md.Id == medicalRecord.Id)
                {
                    md.PatientId = medicalRecord.PatientId;
                }
                newFile.Add(ConvertMedicalRecordToCSVFormat(md));
            }
            File.WriteAllLines(_path, newFile);
            return medicalRecord;
        }

        public bool Delete(int id)
        {
            List<MedicalRecord> medicalRecords = GetAll().ToList();
            List<string> newFile = new List<string>();
            bool isDeleted = false;
            foreach (MedicalRecord md in medicalRecords)
            {
                if (md.Id != id)
                {
                    newFile.Add(ConvertMedicalRecordToCSVFormat(md));
                    isDeleted = true;
                }
            }
            File.WriteAllLines(_path, newFile);
            return isDeleted;
        }

        private MedicalRecord ConvertCSVFormatToMedicalRecord(string medicalRecordCSVFormat)
        {
            var tokens = medicalRecordCSVFormat.Split(_delimiter.ToCharArray());
            return new MedicalRecord(int.Parse(tokens[0]), int.Parse(tokens[1]));
        }

        private string ConvertMedicalRecordToCSVFormat(MedicalRecord medicalRecord)
        {
            return string.Join(_delimiter,
                medicalRecord.Id,
                medicalRecord.PatientId);
        }

        private void AppendLineToFile(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }
    }
}
