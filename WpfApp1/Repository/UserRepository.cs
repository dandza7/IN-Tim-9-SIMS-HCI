using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository.Interface;
using static WpfApp1.Model.User;

namespace WpfApp1.Repository
{
    public class UserRepository : IUserRepository
    {
        private string _path;
        private string _delimiter;

        public UserRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
        }

        private int GetMaxId(IEnumerable<User> users)
        {
            return users.Count() == 0 ? 0 : users.Max(user => user.Id);
        }

        public IEnumerable<User> GetAll()
        {
            List<string> lines = File.ReadAllLines(_path).ToList();
            List<User> users = new List<User>();
            foreach (string line in lines)
            {
                if (line == "") continue;
                users.Add(ConvertCSVFormatToUser(line));
            }
            return users;
        }

        public User Create(User user)
        {
            int maxId = GetMaxId(GetAll());
            user.Id = ++maxId;
            AppendLineToFile(_path, ConvertUserToCSVFormat(user));
            return user;
        }
        public User Update(User user)
        {
            List<User> users = GetAll().ToList();
            List<string> newFile = new List<string>();
            foreach (User u in users)
            {

                if (u.Id == user.Id)
                {
                    u.Name = user.Name;
                    u.Surname = user.Surname;
                    u.Username = user.Username;
                    u.Password = user.Password;
                    u.PhoneNumber = user.PhoneNumber;
                    u.Jmbg = user.Jmbg;
                    u.Role = user.Role;
                }
                newFile.Add(ConvertUserToCSVFormat(u));
            }
            File.WriteAllLines(_path, newFile);
            return user;
        }
        public bool Delete(int userId)
        {
            List<User> users = GetAll().ToList();
            List<string> newFile = new List<string>();
            bool isDeleted = false;
            foreach (User u in users)
            {
                if (u.Id != userId)
                {
                    newFile.Add(ConvertUserToCSVFormat(u));
                    isDeleted = true;
                }
            }
            File.WriteAllLines(_path, newFile);
            return isDeleted;
        }
        public User GetById(int userId)
        {
            List<User> users = GetAll().ToList();
            foreach (User u in users)
            {
                if (u.Id == userId)
                {
                    return u;
                }

            }
            return null;
        }

        public User GetByUsername(string username)
        {
            List<User> users = GetAll().ToList();
            foreach (User u in users)
            {
                if (u.Username == username)
                {
                    return u;
                }

            }
            return null;
        }

        public IEnumerable<User> GetAllPatients()
        {
            List<User> users = GetAll().ToList();
            List<User> patients = new List<User>();
            foreach(User user in users)
            {
                if(user.Role == RoleType.patient)
                {
                    patients.Add(user);
                }
            }
            return patients;
        }

        public IEnumerable<User> GetAllDoctors()
        {
            List<User> users = GetAll().ToList();
            List<User> doctors = new List<User>();
            foreach (User user in users)
            {
                if (user.Role == RoleType.doctor)
                {
                    doctors.Add(user);
                }
            }
            return doctors;
        }
        public IEnumerable<User> GetAllEmployees()
        {
            List<User> users = GetAll().ToList();
            List<User> employees = new List<User>();
            foreach (User user in users)
            {
                if (user.Role != RoleType.patient)
                {
                    employees.Add(user);
                }
            }
            return employees;
        }
        public IEnumerable<User> GetAllExecutives()
        {
            List<User> users = GetAll().ToList();
            List<User> executives = new List<User>();
            foreach (User user in users)
            {
                if (user.Role == RoleType.executive)
                {
                    executives.Add(user);
                }
            }
            return executives;
        }

        public IEnumerable<User> GetAllSecretaries()
        {
            List<User> users = GetAll().ToList();
            List<User> secretaries = new List<User>();
            foreach (User user in users)
            {
                if (user.Role == RoleType.secretary)
                {
                    secretaries.Add(user);
                }
            }
            return secretaries;
        }

        private User ConvertCSVFormatToUser(string userCSVFormat)
        {
            var tokens = userCSVFormat.Split(_delimiter.ToCharArray());
            Enum.TryParse(tokens[7], true, out RoleType role);
            return new User(int.Parse(tokens[0]),
                tokens[1],
                tokens[2],
                tokens[3],
                tokens[4],
                tokens[5],
                tokens[6],
                role);
        }
        private string ConvertUserToCSVFormat(User user)
        {
            return string.Join(_delimiter,
               user.Id,
               user.Name.ToString(),
               user.Surname.ToString(),
               user.Username.ToString(),
               user.Password.ToString(),
               user.PhoneNumber.ToString(),
               user.Jmbg.ToString(),
               user.Role.ToString());
        }
        private void AppendLineToFile(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }
    }
}
