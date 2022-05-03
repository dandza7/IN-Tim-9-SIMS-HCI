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

        public PatientService(UserRepository userRepo, 
            PatientRepository patientRepo, 
            TherapyRepository therapyRepository, 
            MedicalRecordRepository medicalRecordRepository,
            DrugRepository drugRepository,
            NotificationRepository notificationRepository) 
        {
            _patientRepo = patientRepo;
            _therapyRepo = therapyRepository;
            _userRepo = userRepo;
            _medicalRecordRepo = medicalRecordRepository;
            _drugRepo = drugRepository;
            _notificationRepo = notificationRepository;
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
                if(id == patientId)
                {
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


            return _patientRepo.Delete(patientId);
        }
        public Patient GetById(int patientId)
        {
            User u = _userRepo.GetById(patientId);
            Patient p = _patientRepo.GetById(patientId);
            Patient p1 = new Patient(u.Name, u.Surname, u.Jmbg, u.Username, u.Password, u.PhoneNumber, p.Email);
            return p1;
        }
        public Patient GetByUsername(string username)
        {
            User user = _userRepo.GetByUsername(username);
            return _patientRepo.GetById(user.Id);
        }
    }
}
