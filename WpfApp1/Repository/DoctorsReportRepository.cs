using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository
{
    public class DoctorsReportRepository
    {
        private string _path;
        private string _delimiter;

        public DoctorsReportRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
        }

        private int GetMaxId(IEnumerable<DoctorsReport> doctorsReports)
        {
            return doctorsReports.Count() == 0 ? 0 : doctorsReports.Max(medicalRecord => medicalRecord.Id);
        }

        public IEnumerable<DoctorsReport> GetAll()
        {
            List<string> lines = File.ReadAllLines(_path).ToList();
            List<DoctorsReport> doctorsReports = new List<DoctorsReport>();
            foreach (string line in lines)
            {
                if (line == "") continue;
                doctorsReports.Add(ConvertCSVFormatToDoctorsReport(line));
            }
            return doctorsReports;
        }

        public DoctorsReport GetById(int id)
        {
            return GetAll().ToList().SingleOrDefault(doctorsReport => doctorsReport.Id == id);
        }

        public DoctorsReport GetByAppointmentId(int appointmentId)
        {

            return GetAll().ToList().SingleOrDefault(doctorsReport => doctorsReport.AppointmentId== appointmentId);
        }

        public DoctorsReport GetByAppointmentId2(int appointmentId)
        {

            foreach (DoctorsReport dr in GetAll())
                if (dr.AppointmentId == appointmentId) return dr;
            return new DoctorsReport();
        }

        public DoctorsReport Create(DoctorsReport doctorsReport)
        {
            int maxId = GetMaxId(GetAll());
            doctorsReport.Id = ++maxId;
            AppendLineToFile(_path, ConvertDoctorsReportToCSVFormat(doctorsReport));
            return doctorsReport;
        }

        public DoctorsReport Update(DoctorsReport doctorsReport)
        {
            List<DoctorsReport> doctorsReports = GetAll().ToList();
            List<string> newFile = new List<string>();
            foreach (DoctorsReport report in doctorsReports)
            {

                if (report.Id == doctorsReport.Id)
                {
                    report.AppointmentId = doctorsReport.AppointmentId;
                    report.Description = doctorsReport.Description;
                }
                newFile.Add(ConvertDoctorsReportToCSVFormat(report));
            }
            File.WriteAllLines(_path, newFile);
            return doctorsReport;
        }

        public bool Delete(int id)
        {
            List<DoctorsReport> doctorsReports = GetAll().ToList();
            List<string> newFile = new List<string>();
            bool isDeleted = false;
            foreach (DoctorsReport report in doctorsReports)
            {
                if (report.Id != id)
                {
                    newFile.Add(ConvertDoctorsReportToCSVFormat(report));
                    isDeleted = true;
                }
            }
            File.WriteAllLines(_path, newFile);
            return isDeleted;
        }

        private DoctorsReport ConvertCSVFormatToDoctorsReport(string doctorsReportCSVFormat)
        {
            var tokens = doctorsReportCSVFormat.Split(_delimiter.ToCharArray());
            return new DoctorsReport(int.Parse(tokens[0]), int.Parse(tokens[1]), tokens[2]);
        }

        private string ConvertDoctorsReportToCSVFormat(DoctorsReport doctorsReport)
        {
            return string.Join(_delimiter,
                doctorsReport.Id,
                doctorsReport.AppointmentId,
                doctorsReport.Description);
        }

        private void AppendLineToFile(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }
    }
}
