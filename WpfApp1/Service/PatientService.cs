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
        private readonly NotificationRepository _notificationRepo;
        private readonly TherapyRepository _therapyRepo;

        public PatientService(PatientRepository patientRepo, NotificationRepository notificationRepo, TherapyRepository therapyRepository) 
        {
            _patientRepo = patientRepo;
            _notificationRepo = notificationRepo;
            _therapyRepo = therapyRepository;
        }

        public IEnumerable<Patient> GetAll()
        {
            var patients = _patientRepo.GetAll();
            return patients;
        }

        public List<Notification> GetPatientsNotifications(int userId)
        {
            List<Notification> notifications = _notificationRepo.GetAll().ToList();
            List<Notification> patientsNotifications = new List<Notification>();
            foreach (Notification  notification in notifications)
            {
                if(notification.UserId == userId)
                {
                    patientsNotifications.Add(notification);
                }
            }
            return patientsNotifications;
        }

        public List<Therapy> GetPatientsTherapies(int patientId)
        {
            List<Therapy> patientsTherapies = new List<Therapy>();
            List<int> therapyIds = _patientRepo.GetById(patientId).TherapyIds;

            therapyIds.ForEach(therapy => patientsTherapies.Add(_therapyRepo.GetById(therapy)));

            return patientsTherapies;
        }
      
        public Patient Create(Patient patient)
        {


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


            return _patientRepo.GetById(patientId);
        }
    }
}
