using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository.Interface
{
    public interface IDrugRepository
    {
        IEnumerable<Drug> GetAll();
        Drug GetById(int id);
        Drug Create(Drug drug);
        Drug Update(Drug drug);
        bool Delete(int id);
    }
}
