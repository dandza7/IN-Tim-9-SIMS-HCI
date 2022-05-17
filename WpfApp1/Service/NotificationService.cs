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
        private readonly MedicalRecordRepository _medicalRecordRepo;
        private readonly TherapyRepository _therapyRepo;
        public NotificationService(NotificationRepository notificationRepo, 
            DrugRepository drugRepo, 
            MedicalRecordRepository medicalRecordRepo,
            TherapyRepository therapyRepo)
        {
            _notificationRepo = notificationRepo;
            _drugRepo = drugRepo;
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

        public void GetScheduledTherapyNotifications(int patientId)
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
                    CreateTherapyNotificationForPatient(patientId, drugName, startingTime, therapy.Frequency);
                    startingTime = startingTime.AddHours(timeBetweenNotifications);
                }
            }   
        }

        private void CreateTherapyNotificationForPatient(int patientId, string drugName, DateTime whenToSend, float frequency)
        {

            string content = "Take " + drugName + " in one hour time!";
            string title = "Patient " + patientId + " " + drugName + " Therapy";
            bool isDuplicate = false;
            List<Notification> sentNotifications = _notificationRepo.GetAllForUser(patientId).ToList();
            foreach (Notification sentNotification in sentNotifications)
            {
                if (sentNotification.Date == whenToSend && sentNotification.Content.Equals(content))
                {
                    isDuplicate = true;
                }
                // Ukoliko se terapija pije manje od jednom dnevno onda treba provjeriti koliko je dana prošlo od prethodne notifikacije
                // za terapiju, ukoliko je prošlo manje nego što treba proći da bi se opet pila onda se ne treba slati nova notifikacija
                if (frequency < 1)
                {
                    int daysToPass = (int)Math.Round(1 / frequency);
                    if (sentNotification.Content.Equals(content) && sentNotification.Date.AddDays(daysToPass) > whenToSend)
                    {
                        isDuplicate = true;
                    }
                }
            }
            if (whenToSend < DateTime.Now && isDuplicate == false)
            {
                Notification notification = new Notification(whenToSend, content, title, patientId, false, false);
                _notificationRepo.Create(notification);
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
