using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository.Interface
{
    public interface IRenovationRepository
    {
        IEnumerable<Renovation> GetAll();
        Renovation GetById(int id);
        Renovation Create(Renovation renovation);
        bool Delete(int id);
        bool IsRoomAvailable(int roomId, DateTime startOfInterval, DateTime endOfinterval);
    }
}
