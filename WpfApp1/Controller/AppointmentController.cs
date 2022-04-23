using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;
using WpfApp1.View.Model.Patient;
using WpfApp1.View.Model.Secretary;

namespace WpfApp1.Controller
{
    public class AppointmentController
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public IEnumerable<Appointment> GetAll()
        {
            return _appointmentService.GetAll();
        }

        public Appointment Create(Appointment appointment)
        {
            return _appointmentService.Create(appointment);
        }

        public Appointment Update(Appointment appointment)
        {
            return _appointmentService.Update(appointment);
        }

        public bool Delete(int id)
        {
            return  _appointmentService.Delete(id);
        }

        public List<SecretaryAppointmentView> GetSecretaryAppointmentViews()
        {
            return _appointmentService.GetSecretaryAppointmentViews();
        }

        public List<AppointmentView> GetPatientsAppointmentsView(int patientId)
        {
            return _appointmentService.GetPatientsAppointmentsView(patientId);
        }

        public List<AppointmentView> GetAvailableAppointmentOptions(string priority, 
            DateTime startOfInterval, 
            DateTime endOfInterval, 
            int doctorId, 
            int patientId)
        {
            return _appointmentService.GetAvailableAppointmentOptions(priority, startOfInterval, endOfInterval, doctorId, patientId);
        }
    }
}
