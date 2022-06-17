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
        public IEnumerable<Appointment> GetAllBySpecialization(string spec)
        {
            return _appointmentService.GetAllBySpecialization(spec);
        }
        public IEnumerable<Appointment> GetAllByMonth(int month)
        {
            return _appointmentService.GetAllByMonth(month);
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
        public List<SecretaryAppointmentView> GetSecretaryAppointmentViewsInTimeInterval(DateTime startOfInterval, DateTime endOfInterval)
        {
            return _appointmentService.GetSecretaryAppointmentViewsInTimeInterval(startOfInterval, endOfInterval);
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
                                                                    int doctorId, SpecType specialization, int patientId, int oldAppointmentId)
        {
            return _appointmentService.GetAvailableAppointmentOptions(priority, startOfInterval, endOfInterval, 
                                                                        doctorId, specialization, patientId, oldAppointmentId);
        }

        public Appointment GetById(int id)
        {
            return _appointmentService.GetById(id);
        }
        public List<AppointmentView> GetSortedMovableAppointments(SpecType specialization, DateTime startOfInterval)
        {
            return _appointmentService.GetSortedMovableAppointments(specialization, startOfInterval);
        }

        public DateTime GetNearestFreeTerm(int appointmentId)
        {
            return _appointmentService.GetNearestFreeTerm(appointmentId);
        }


        public IEnumerable<Appointment> GetAllByPatientId(int patientId)
        {
            return _appointmentService.GetAllByPatientId(patientId);
        }
        public List<Appointment> GetAllByDoctorId(int id)
        {
            return _appointmentService.GetAllByDoctorId(id);
        }

        public bool CreateUrgentAppointment(int patientId, SpecType specialization, DateTime startOfInterval)
        {
            return _appointmentService.CreateUrgentAppointment(patientId, specialization, startOfInterval);
        }


    }
}
