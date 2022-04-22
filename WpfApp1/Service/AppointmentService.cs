using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;
using WpfApp1.View.Converter;
using WpfApp1.View.Model.Patient;
using WpfApp1.View.Model.Secretary;

namespace WpfApp1.Service
{
    public class AppointmentService
    {
        private readonly AppointmentRepository _appointmentRepo;
        private readonly DoctorRepository _doctorRepo;
        private readonly PatientRepository _patientRepo;
        private readonly RoomRepository _roomRepo;
        private readonly UserRepository _userRepo;

        public AppointmentService(AppointmentRepository appointmentRepo, 
            DoctorRepository doctorRepository, 
            PatientRepository patientRepo,
            RoomRepository roomRepository,
            UserRepository userRepository)
        {
            _appointmentRepo = appointmentRepo;
            _doctorRepo = doctorRepository;
            _patientRepo = patientRepo;
            _roomRepo = roomRepository;
            _userRepo = userRepository;
        }

        public IEnumerable<Appointment> GetAll()
        {
            return _appointmentRepo.GetAll();
        }

        public Appointment Create(Appointment appointment)
        {
            return _appointmentRepo.Create(appointment);
        }

        public Appointment Update(Appointment appointment)
        {
            return _appointmentRepo.Update(appointment);
        }

        public bool Delete(int id)
        {
            return _appointmentRepo.Delete(id);
        }

        public List<AppointmentView> UpdateData()
        {
            List<AppointmentView> appointmentViews = new List<AppointmentView>();
            List<Appointment> appointments = _appointmentRepo.GetAll().ToList();
            foreach (Appointment appointment in appointments)
            {
                Doctor doctor = _doctorRepo.GetById(appointment.DoctorId);
                Room room = _roomRepo.Get(doctor.RoomId);
                appointmentViews.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(appointment, doctor, room));
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
                User user = _userRepo.GetById(doctor.Id);
                Room room = _roomRepo.Get(doctor.RoomId);
                appointmentViews.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(appointment, user, room));
            }
            return appointmentViews;
        }
        public List<SecretaryAppointmentView> GetSecretaryAppointmentViews()
        {
            List<SecretaryAppointmentView> appointmentViews = new List<SecretaryAppointmentView>();
            List<Appointment> appointments = _appointmentRepo.GetAll().ToList();
            foreach (Appointment appointment in appointments)
            {
                User doctor = _userRepo.GetById(_doctorRepo.GetById(appointment.DoctorId).Id);
                User patient = _userRepo.GetById(_patientRepo.GetById(appointment.PatientId).Id);
                appointmentViews.Add(AppointmentConverter.ConvertSecretaryAppointmentSecretaryAppointmentView(appointment, doctor, patient));
            }
            return appointmentViews;
        }
        public Appointment GetById(int appointmentId)
        {
            return _appointmentRepo.GetById(appointmentId);
        }

    }
}
