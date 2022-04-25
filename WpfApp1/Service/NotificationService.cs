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

        public IEnumerable<Notification> GetUsersNotifications(int userId)
        {
            //sortira userove notifikacije u rastucem redoslijedu vremena kada su poslane (starije notifikacije idu na vrh)
            return _notificationRepo.GetAllForUser(userId).OrderBy(notification => notification.Date).ToList();
        }

        public IEnumerable<Notification> GetUsersNotDeletedNotifications(int userId)
        {
            return _notificationRepo.GetAllNotDeletedForUser(userId).OrderBy(notification => notification.Date).ToList();
        }

        private void CreateNotificationForPatient(int patientId, string drugName, DateTime whenToSend)
        {
            
            string content = "Take " + drugName + " in one hour time!";
            string title = "Patient " + patientId + " " + drugName + " Therapy";
            bool isDuplicate = false;
            List<Notification> sentNotifications = _notificationRepo.GetAllForUser(patientId).ToList();
            foreach(Notification sentNotification in sentNotifications)
            {
                if (sentNotification.Date == whenToSend && sentNotification.Content.Equals(content))
                {
                    isDuplicate = true;
                }
            }
            if (whenToSend < DateTime.Now && isDuplicate == false)
            {
                Notification notification = new Notification(whenToSend, content, title, patientId, false, false);
                _notificationRepo.Create(notification);
            }
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
        // U ponoć se fizički brišu sve notifikacije pacijenta koje je on obrisao fiziški
        // Dugi 'if' samo provjerava da li je prošla ponoć i da li je to pacijent koji otvara notifikacije
        public void DeleteOldUsersNotifications(int patientId)
        {
            List<Notification> deletedNotifications = _notificationRepo.GetAllLogicallyDeleted().ToList();
            DateTime currentTime = DateTime.Now;
            foreach (Notification notification in deletedNotifications)
            {
                if(notification.UserId == patientId && currentTime.Year >= notification.Date.Year &&
                    currentTime.Month >= notification.Date.Month && currentTime.Day > notification.Date.Day)
                {
                    _notificationRepo.Delete(patientId);
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

        public bool DeleteLogically(int id)
        {
            return _notificationRepo.DeleteLogically(id);
        }
    }
}
