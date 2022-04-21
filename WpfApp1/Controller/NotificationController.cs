using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class NotificationController
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public IEnumerable<Notification> GetAll()
        {
            return _notificationService.GetAll();
        }

        public Notification Create(Notification notification)
        {
            return _notificationService.Create(notification);
        }

        public Notification Update(Notification notification)
        {
            return _notificationService.Update(notification);
        }

        public bool Delete(int id)
        {
            return _notificationService.Delete(id);
        }

        public void SchedulePatientsNotifications(int patientId, Therapy therapy)
        {
            _notificationService.SchedulePatientsNotifications(patientId, therapy);
        }
    }
}
