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
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
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
        public IEnumerable<User> GetAllEmployees()
        {
            return _userRepository.GetAllEmployees();
        }

        public IEnumerable<User> GetAllExecutives()
        {
            return _userRepository.GetAllExecutives();
        }

        public IEnumerable<User> GetAllSecretaries()
        {
            return _userRepository.GetAllSecretaries();
        }

        public User Create(User user)
        {
            return _userRepository.Create(user);
        }

        public User CheckLogIn(string username, string pw)
        {
            List<User> users = _userRepository.GetAll().ToList();
            foreach(User user in users)
            {
                if (!user.Username.Equals(username)) continue;
                return user.Password.Equals(pw) ? user : null;
            }
            return null;
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
