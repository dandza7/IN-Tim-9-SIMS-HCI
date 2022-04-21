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
        public NotificationService(NotificationRepository notificationRepo, DrugRepository drugRepo)
        {
            _notificationRepo = notificationRepo;
            _drugRepo = drugRepo;
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
                _notificationRepo.Create(notification);
                return true;
            }
            return false;
        }

        public void SchedulePatientsNotifications(int patientId, Therapy therapy)
        {
            double timeBetweenNotifications = 24 / therapy.Frequency;
            string drugName = _drugRepo.GetById(therapy.DrugId).Name;
            DateTime startingTime = DateTime.Today.AddHours(8);
            int howManyTimes = (int)(Math.Ceiling(therapy.Frequency));

            for(int i = 0; i < howManyTimes; i++)
            {
                CreateNotificationForPatient(patientId, drugName, startingTime);
                startingTime = startingTime.AddHours(timeBetweenNotifications);
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
