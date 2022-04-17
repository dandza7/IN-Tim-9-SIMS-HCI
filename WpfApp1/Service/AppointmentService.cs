using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;
using WpfApp1.View.Converter;
using WpfApp1.View.Model.Patient;

namespace WpfApp1.Service
{
    public class AppointmentService
    {
        private readonly AppointmentRepository _appointmentRepo;
        private readonly DoctorRepository _doctorRepo;
        public AppointmentService(AppointmentRepository appointmentRepo, DoctorRepository doctorRepository)
        {
            _appointmentRepo = appointmentRepo;
            _doctorRepo = doctorRepository;
        }

        internal IEnumerable<Appointment> GetAll()
        {
            var appointments = _appointmentRepo.GetAll();
            return appointments;
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

        public List<AppointmentView> UpdateData()
        {
            List<AppointmentView> appointmentViews = new List<AppointmentView>();
            List<Appointment> appointments = _appointmentRepo.UpdateAppointments();
            foreach (Appointment appointment in appointments)
            {
                Doctor doctor = _doctorRepo.GetById(appointment.DoctorId);
                appointmentViews.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(appointment, doctor));
            }
            return appointmentViews;
        }

        public List<AppointmentView> GetAppointmentViews()
        {
            List<AppointmentView> appointmentViews = new List<AppointmentView>();
            List<Appointment> appointments = _appointmentRepo.GetAll().ToList();
            foreach (Appointment appointment in appointments)
            {
                Doctor doctor = _doctorRepo.GetById(appointment.DoctorId);
                appointmentViews.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(appointment, doctor));
            }
            return appointmentViews;
        }
    }
}
