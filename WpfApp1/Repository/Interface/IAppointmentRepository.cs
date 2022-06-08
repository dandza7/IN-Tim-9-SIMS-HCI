using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository.Interfaces
{
    public interface IAppointmentRepository
    {
        IEnumerable<Appointment> GetAll();

        Appointment GetById(int appointmentId);

        Appointment Create(Appointment appointment);

        Appointment Update(Appointment appointment);

        bool Delete(int id);

        IEnumerable<Appointment> GetAllAppointmentsInTimeInterval(DateTime start, DateTime end);

        IEnumerable<Appointment> GetAllAppointmentsInTimeIntervalForDoctor(DateTime start, DateTime end, int doctorId);

        IEnumerable<Appointment> GetDoctorsMovableAppointments(DateTime start, DateTime end, int doctorId);

        IEnumerable<Appointment> GetAllAppointmentsForDoctor(int doctorId);

        IEnumerable<Appointment> GetAllAppointmentsForPatient(int patientId);

        IEnumerable<Appointment> GetAllByPatientId(int patientId);

        List<Appointment> GetAllByDoctorId(int id);

    }
}
