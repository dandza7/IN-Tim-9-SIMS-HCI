using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class DoctorsReportController
    {
        private readonly DoctorsReportService _doctorsReportService;

        public DoctorsReportController(DoctorsReportService doctorsReportService)
        {
            _doctorsReportService = doctorsReportService;
        }

        public IEnumerable<DoctorsReport> GetAll()
        {
            return _doctorsReportService.GetAll();
        }

        public DoctorsReport GetById(int id)
        {
            return _doctorsReportService.GetById(id);
        }

        public DoctorsReport GetByAppointmentId(int appointmentId)
        {
            return _doctorsReportService.GetByAppointmentId(appointmentId);
        }
        public List<DoctorsReport> GetByPatientId(int patientId)
        {
            return _doctorsReportService.GetByPatientId(patientId);
        }

        public DoctorsReport Create(DoctorsReport doctorsReport)
        {
            return _doctorsReportService.Create(doctorsReport);
        }

        public DoctorsReport Update(DoctorsReport doctorsReport)
        {
            return _doctorsReportService.Update(doctorsReport);
        }

        public bool Delete(int id)
        {
            return _doctorsReportService.Delete(id);
        }
    }
}
