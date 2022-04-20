using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class UserController
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public IEnumerable<User> GetAll()
        {
            return _userService.GetAll();
        }

        public User GetById(int userId)
        {
            return _userService.GetById(userId);
        }

        public User GetByUsername(string username)
        {
            return _userService.GetByUsername(username);
        }

        public User Create(User user)
        {
            return _userService.Create(user);
        }

        public User Update(User user)
        {
            return _userService.Update(user);
        }

        public bool Delete(int id)
        {
            return _userService.Delete(id);
        }
    }

}
