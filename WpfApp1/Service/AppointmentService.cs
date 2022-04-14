using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class AppointmentService
    {
        private readonly AppointmentRepository _appointmentRepo;

        public AppointmentService(AppointmentRepository appointmentRepo)
        {
            _appointmentRepo = appointmentRepo;
        }

        internal IEnumerable<Appointment> GetAll()
        {
            var appointments = _appointmentRepo.GetAll();
            return appointments;
        }

        private Appointment FindAppointmentById(IEnumerable<Appointment> appointments, long id)
        {
            return appointments.SingleOrDefault(appointment => appointment.Id == id);
        }

        public Appointment Create(Appointment appointment)
        {
            // save appointments
            return _appointmentRepo.Create(appointment);
        }

        public Appointment Update(Appointment appointment)
        {
            // save appointments
            return _appointmentRepo.Update(appointment);
        }

        public bool Delete(int id)
        {
            return _appointmentRepo.Delete(id);
        }

        public List<Appointment> UpdateAppointments()
        {
            return _appointmentRepo.UpdateAppointments();
        }
    }
}
