using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();

        User Create(User user);

        User Update(User user);

        bool Delete(int userId);

        User GetById(int userId);

        User GetByUsername(string username);

        IEnumerable<User> GetAllPatients();

        IEnumerable<User> GetAllDoctors();

        IEnumerable<User> GetAllEmployees();

        IEnumerable<User> GetAllExecutives();

        IEnumerable<User> GetAllSecretaries();
    }
}
