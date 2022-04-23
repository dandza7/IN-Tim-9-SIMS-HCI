using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly NotificationRepository _notificationRepo;
        public UserService(UserRepository userRepository, NotificationRepository notificationRepo)
        {
            _userRepository = userRepository;
            _notificationRepo = notificationRepo;
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User GetById(int userId)
        {
            return _userRepository.GetById(userId);
        }

        public IEnumerable<User> GetAllPatients()
        {
            return _userRepository.GetAllPatients();
        }

        public IEnumerable<User> GetAllDoctors()
        {
            return _userRepository.GetAllDoctors();
        }

        public IEnumerable<User> GetAllExecutives()
        {
            return _userRepository.GetAllExecutives();
        }

        public IEnumerable<User> GetAllSecretaries()
        {
            return _userRepository.GetAllSecretaries();
        }

        public IEnumerable<Notification> GetUsersNotifications(int userId)
        {
            List<Notification> notifications = _notificationRepo.GetAll().ToList();
            List<Notification> usersNotifications = new List<Notification>();
            foreach (Notification notification in notifications)
            {
                if (notification.UserId == userId)
                {
                    usersNotifications.Add(notification);
                }
            }
            //sortira userove notifikacije u rastucem redoslijedu vremena kada su poslane (starije notifikacije idu na vrh)
            return usersNotifications.OrderBy(notification => notification.Date).ToList();
        }

        public User Create(User user)
        {
            return _userRepository.Create(user);
        }

        public User Update(User user)
        {
            return _userRepository.Update(user);
        }

        public bool Delete(int id)
        {
            return _userRepository.Delete(id);
        }
    }
}
