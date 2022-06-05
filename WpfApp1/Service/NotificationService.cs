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
        private readonly NoteRepository _noteRepo;
        public NotificationService(NotificationRepository notificationRepo, 
            DrugRepository drugRepo, 
            MedicalRecordRepository medicalRecordRepo,
            TherapyRepository therapyRepo,
            NoteRepository noteRepo)
        {
            _notificationRepo = notificationRepo;
            _drugRepo = drugRepo;
            _medicalRecordRepo = medicalRecordRepo;
            _therapyRepo = therapyRepo;
            _noteRepo = noteRepo;
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
            return _notificationRepo.GetAllForUser(userId).OrderBy(notification => notification.Date).ToList();
        }

        public IEnumerable<Notification> GetUsersNotDeletedNotifications(int userId)
        {
            return _notificationRepo.GetAllNotDeletedForUser(userId).OrderBy(notification => notification.Date).ToList();
        }

        public void GetScheduledAlarmsForPatient(int patientId)
        {
            List<Note> patientsNotes = _noteRepo.GetPatientsNotes(patientId).ToList();
            
            foreach(Note note in patientsNotes)
            {
                if(note.AlarmTime <= DateTime.Now)
                {
                    CreateAlarmNotificationForPatient(note);
                }
            }
        }

        private void CreateAlarmNotificationForPatient(Note note)
        {
            string title = "Patient Notification Alarm for Note " + note.Id;
            string content = "You set an alarm for the note!";
            
            if(!IsAlarmDuplicate(note))
            {
                Notification notification = new Notification(note.AlarmTime, content, title, note.PatientId, false, false);
                _notificationRepo.Create(notification);
            }
        }

        private bool IsAlarmDuplicate(Note note)
        {
            List<Notification> sentNotifications = _notificationRepo.GetAllForUser(note.PatientId).ToList();
            string title = "Patient Notification Alarm for Note " + note.Id;
            foreach (Notification sentNotification in sentNotifications)
            {
                if (sentNotification.Title.Equals(title))
                    return true;
            }
            return false;
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
            List<Notification> sentNotifications = _notificationRepo.GetAllForUser(patientId).ToList();
            foreach (Notification sentNotification in sentNotifications)
                if(IsTherapyDuplicate(sentNotification, whenToSend, content, frequency)) return;
            
            if (whenToSend < DateTime.Now)
            {
                Console.WriteLine("Notifikacija sa sadrzajem: '" + content + "' do sada nije poslana.");
                _notificationRepo.Create(new Notification(whenToSend, content, title, patientId, false, false));
            }
        }

        private bool IsTherapyDuplicate(Notification notification, DateTime sendingTime, string content, float frequency)
        {
            if (notification.Date == sendingTime && notification.Content.Equals(content))
                return true;

            if (frequency < 1)
            {
                int daysToPass = (int)Math.Round(1 / frequency);
                if (notification.Content.Equals(content) && notification.Date.AddDays(daysToPass) > sendingTime)
                    return true;
            }
            return false;
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
