using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class PatientService
    {
        private readonly PatientRepository _patientRepo;
        private readonly UserRepository _userRepo;
        private readonly MedicalRecordRepository _medicalRecordRepo;
        private readonly AppointmentRepository _appointmentRepo;

        public PatientService(UserRepository userRepository, 
            PatientRepository patientRepository, 
            MedicalRecordRepository medicalRecordRepository,
            AppointmentRepository appointmentRepository) 
        {
            _patientRepo = patientRepository;
            _userRepo = userRepository;
            _medicalRecordRepo = medicalRecordRepository;
            _appointmentRepo = appointmentRepository;
        }

        public IEnumerable<Patient> GetAll()
        {
            return _patientRepo.GetAll();
        }

        public Patient Create(Patient patient)
        {
            MedicalRecord mr = new MedicalRecord(patient.Id);
            _medicalRecordRepo.Create(mr);

            return _patientRepo.Create(patient);
        }

        public Patient Update(Patient patient)
        {
            return _patientRepo.Update(patient);
        }

        public bool Delete(int patientId)
        {
            MedicalRecord mr = _medicalRecordRepo.GetPatientsMedicalRecord(patientId);
            List<Appointment> patientsAppointments = _appointmentRepo.GetAllAppointmentsForPatient(patientId).ToList();
            patientsAppointments.ForEach(appointment => _appointmentRepo.Delete(appointment.Id));
            _medicalRecordRepo.Delete(mr.Id);
            _userRepo.Delete(patientId);
            return _patientRepo.Delete(patientId);
        }
        public Patient GetById(int id)
        {
            User u = _userRepo.GetById(id);
            Patient p = _patientRepo.GetById(id);
            Patient p1 = new Patient(u.Name, u.Surname, u.Jmbg, u.Username, u.Password, u.PhoneNumber,
            p.Email, p.Street, p.City, p.Country, p.NumberOfCancellations, p.LastCancellationDate);
            return p1;
        }

        public Patient GetByUsername(string username)
        {
            User user = _userRepo.GetByUsername(username);
            if (_patientRepo.GetById(user.Id) == null) return null;
            else
                return _patientRepo.GetById(user.Id);
        }
    }
}
