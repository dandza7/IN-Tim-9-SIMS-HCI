using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository.Interface
{
    public interface IMeetingRepository
    {
        IEnumerable<Meeting> GetAll();

        Meeting Create(Meeting meeting);



    }
}
