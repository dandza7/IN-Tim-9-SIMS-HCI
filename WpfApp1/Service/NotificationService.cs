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

        public NotificationService(NotificationRepository notificationRepo)
        {
            _notificationRepo = notificationRepo;
        }

        public IEnumerable<Notification> GetAll()
        {
            return _notificationRepo.GetAll();
        }

        public Notification Create(Notification notification)
        {
            return _notificationRepo.Create(notification);
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
