using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class DoctorsReportService
    {
        private readonly DoctorsReportRepository _doctorsReportRepo;
        public DoctorsReportService(DoctorsReportRepository doctorsReportRepo)
        {
            _doctorsReportRepo = doctorsReportRepo;
        }

        public IEnumerable<DoctorsReport> GetAll()
        {
            return _doctorsReportRepo.GetAll();
        }

        public DoctorsReport GetById(int id)
        {
            return _doctorsReportRepo.GetById(id);
        }

        public DoctorsReport GetByAppointmentId(int appointmentId)
        {
            return _doctorsReportRepo.GetByAppointmentId(appointmentId);
        }

        public DoctorsReport Create(DoctorsReport doctorsReport)
        {
            return _doctorsReportRepo.Create(doctorsReport);
        }

        public DoctorsReport Update(DoctorsReport doctorsReport)
        {
            return _doctorsReportRepo.Update(doctorsReport);
        }

        public bool Delete(int id)
        {
            return _doctorsReportRepo.Delete(id);
        }
    }
}
