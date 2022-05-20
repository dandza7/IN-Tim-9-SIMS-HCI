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
        private readonly TherapyRepository _therapyRepo;
        private readonly UserRepository _userRepo;
        private readonly MedicalRecordRepository _medicalRecordRepo;
        private readonly DrugRepository _drugRepo;
        private readonly NotificationRepository _notificationRepo;
        private readonly AppointmentRepository _appointmentRepo;

        public PatientService(UserRepository userRepository, 
            PatientRepository patientRepository, 
            TherapyRepository therapyRepository, 
            MedicalRecordRepository medicalRecordRepository,
            DrugRepository drugRepository,
            NotificationRepository notificationRepository,
            AppointmentRepository appointmentRepository) 
        {
            _patientRepo = patientRepository;
            _therapyRepo = therapyRepository;
            _userRepo = userRepository;
            _medicalRecordRepo = medicalRecordRepository;
            _drugRepo = drugRepository;
            _notificationRepo = notificationRepository;
            _appointmentRepo = appointmentRepository;
        }

        public IEnumerable<Patient> GetAll()
        {
            return _patientRepo.GetAll();
        }

        public List<Therapy> GetPatientsTherapies(int patientId)
        {
            MedicalRecord medicalRecord = _medicalRecordRepo.GetPatientsMedicalRecord(patientId);
            List<Therapy> patientsTherapies = _therapyRepo.GetPatientsTherapies(medicalRecord.Id).ToList();

            return patientsTherapies;
        }

        public List<Drug> GetPatientsDrugs(int patientId)
        {
            List<Drug> drugs = new List<Drug>();
            List<Therapy> patientsTherapies = GetPatientsTherapies(patientId);

            patientsTherapies.ForEach(therapy => drugs.Add(_drugRepo.GetById(therapy.DrugId)));

            return drugs;
        }

        // U ponoć se fizički brišu sve notifikacije pacijenta koje je on obrisao fiziški
        // Dugi 'if' samo provjerava da li je prošla ponoć i da li je to pacijent koji otvara notifikacije
        public void DeleteOldPatientsNotifications(int patientId)
        {
            List<Notification> deletedNotifications = _notificationRepo.GetAllLogicallyDeleted().ToList();
            MedicalRecord medicalRecord = _medicalRecordRepo.GetPatientsMedicalRecord(patientId);
            List<Therapy> patientsTherapies = _therapyRepo.GetPatientsTherapies(medicalRecord.Id).ToList();
            DateTime currentTime = DateTime.Now;
            List<Drug> drugs = _drugRepo.GetAll().ToList();
            foreach (Notification notification in deletedNotifications)
            {
                int id = notification.UserId;
                if (id != patientId) continue;
                int drugId = -1;
                float administrationFrequency = -1;
                int drugNameLength = notification.Content.Length - "Take ".Length - " in one hour time!".Length;
                string drugName = notification.Content.Substring("Take ".Length, drugNameLength);
                foreach(Drug drug in drugs)
                {
                    if (drug.Name.Equals(drugName))
                    {
                        drugId = drug.Id;
                    }
                    foreach (Therapy therapy in patientsTherapies)
                    {
                        if (therapy.DrugId == drugId)
                        {
                            administrationFrequency = therapy.Frequency;
                        }
                        // Provjerava da li se terapija uzima svaki dan i ako da onda da li je prošao dan kad je trebalo da se uzme
                        // Ukoliko je to slučaj može da se briše
                        if (administrationFrequency >= 1 && currentTime.Year >= notification.Date.Year &&
                        currentTime.Month >= notification.Date.Month && currentTime.Day > notification.Date.Day)
                        {
                            _notificationRepo.Delete(notification.Id);
                        }
                        else if (administrationFrequency < 1 && administrationFrequency > 0)
                        {
                            int daysToPass = (int)Math.Round(1 / administrationFrequency);
                            if(notification.Date.AddDays(daysToPass) <= DateTime.Now)
                            {
                                _notificationRepo.Delete(notification.Id);
                            }
                        }
                    }
                }            
            }
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
            return _patientRepo.GetById(user.Id);
        }
    }
}
