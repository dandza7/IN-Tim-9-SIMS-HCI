using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository.Interface
{
    public interface INotificationRepository
    {
        IEnumerable<Notification> GetAll();

        IEnumerable<Notification> GetAllForUser(int userId);

        IEnumerable<Notification> GetAllNotDeletedForUser(int userId);

        IEnumerable<Notification> GetAllLogicallyDeleted();

        Notification GetById(int id);

        Notification Create(Notification notification);

        Notification Update(Notification notification);

        bool Delete(int id);

        bool DeleteLogically(int id);
    }
}
