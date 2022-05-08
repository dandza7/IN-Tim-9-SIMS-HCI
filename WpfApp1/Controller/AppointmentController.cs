using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;
using WpfApp1.View.Model.Patient;
using WpfApp1.View.Model.Secretary;
using static WpfApp1.Model.Doctor;

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

        public bool Delete(int appointmentId)
        {
            return  _appointmentService.Delete(appointmentId);
        }

        public bool AppointmentCancellationByPatient(int patientId, int appointmentId)
        {
            return _appointmentService.AppointmentCancellationByPatient(patientId, appointmentId);
        }

        public List<SecretaryAppointmentView> GetSecretaryAppointmentViews()
        {
            return _appointmentService.GetSecretaryAppointmentViews();
        }

        public List<AppointmentView> GetPatientsAppointmentsView(int patientId)
        {
            return _appointmentService.GetPatientsAppointmentsView(patientId);
        }

        public List<AppointmentView> GetPatientsReportsView(int patientId)
        {
            return _appointmentService.GetPatientsReportsView(patientId);
        }

        public List<AppointmentView> GetPatientsReportsInTimeInterval(int patientId, DateTime startOfInterval, DateTime endOfInterval)
        {
            return _appointmentService.GetPatientsReportsInTimeInterval(patientId, startOfInterval, endOfInterval);
        }

        public List<AppointmentView> GetAvailableAppointmentOptions(string priority, DateTime startOfInterval, DateTime endOfInterval, 
                                                                    int doctorId, int patientId, int oldAppointmentId)
        {
            return _appointmentService.GetAvailableAppointmentOptions(priority, startOfInterval, endOfInterval, 
                                                                        doctorId, patientId, oldAppointmentId);
        }
        public List<AppointmentView> SecretaryGetAvailableAppointmentOptions(string priority, DateTime startOfInterval, DateTime endOfInterval,
                                                            int doctorId, int patientId, int oldAppointmentId, SpecType spec)
        {
            return _appointmentService.SecretaryGetAvailableAppointmentOptions(priority, startOfInterval, endOfInterval,
                                                                        doctorId, patientId, oldAppointmentId, spec);
        }

        public Appointment GetById(int id)
        {
            return _appointmentService.GetById(id);
        }
    }
}
