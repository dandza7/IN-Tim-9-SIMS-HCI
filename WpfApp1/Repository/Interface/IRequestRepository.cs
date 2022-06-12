using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository.Interface
{
    public interface IRequestRepository
    {

        List<Request> GetAll();
        Request GetById(int id);
        IEnumerable<Request> GetAllByDoctorId(int id);
        IEnumerable<Request> GetAllPending();
        Request Create(Request request);
        Request Update(Request requestForUpdate);

    }
}
