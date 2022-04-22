using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class NotificationService
    {
        private readonly NotificationRepository _notificationRepo;
        private readonly DrugRepository _drugRepo;
        private readonly PatientRepository _patientRepo;
        private readonly MedicalRecordRepository _medicalRecordRepo;
        private readonly TherapyRepository _therapyRepo;
        public NotificationService(NotificationRepository notificationRepo, 
            DrugRepository drugRepo, 
            PatientRepository patientRepo, 
            MedicalRecordRepository medicalRecordRepo,
            TherapyRepository therapyRepo)
        {
            _notificationRepo = notificationRepo;
            _drugRepo = drugRepo;
            _patientRepo = patientRepo;
            _medicalRecordRepo = medicalRecordRepo;
            _therapyRepo = therapyRepo;
        }

        public IEnumerable<Notification> GetAll()
        {
            List<Notification> ns = _notificationRepo.GetAll().ToList();
            foreach (var nsItem in ns)
            {
                Console.WriteLine(nsItem.Content);
            }
            return _notificationRepo.GetAll();
        }

        public Notification GetById(int id)
        {
            return _notificationRepo.GetById(id);
        }

        public Notification Create(Notification notification)
        {
            return _notificationRepo.Create(notification);
        }

        public bool CreateNotificationForPatient(int patientId, string drugName, DateTime whenToSend)
        {
            
            string content = "Take " + drugName + " in one hour time!";
            string title = "Patient " + patientId + " " + drugName + " Therapy";

            if(whenToSend < DateTime.Now)
            {
                Notification notification = new Notification(whenToSend, content, title, patientId);
                Console.WriteLine("napravio novu notifikaciju");
                _notificationRepo.Create(notification);
                return true;
            }
            return false;
        }

        public void GetScheduledPatientsNotifications(int patientId)
        {
            int medicalRecordId = _medicalRecordRepo.GetPatientsMedicalRecord(patientId).Id;
            List<Therapy> therapies = _therapyRepo.GetPatientsTherapies(medicalRecordId).ToList();
            
            foreach (Therapy therapy in therapies)
            {
                double timeBetweenNotifications = 16 / therapy.Frequency;
                string drugName = _drugRepo.GetById(therapy.DrugId).Name;
                int howManyTimes = (int)(Math.Ceiling(therapy.Frequency));
                DateTime startingTime = DateTime.Today.AddHours(7);

                for (int i = 0; i < howManyTimes; i++)
                {
                    CreateNotificationForPatient(patientId, drugName, startingTime);
                    startingTime = startingTime.AddHours(timeBetweenNotifications);
                }
            }   
        }

        public Notification Update(Notification notification)
        {
            return _notificationRepo.Update(notification);
        }

        public bool Delete(int id)
        {
            return _notificationRepo.Delete(id);
        }
    }
}
