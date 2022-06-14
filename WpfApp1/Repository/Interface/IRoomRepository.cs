using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository.Interface
{
    public interface IRoomRepository
    {
        Room GetById(int id);
        IEnumerable<Room> GetAll();
        Room Create(Room room);
        bool Delete(int id);
        Room Update(Room room);
        IEnumerable<Room> GetAllByType(string type);
    }
}
