using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Repository
{
    internal interface Repository<T>
    {
        T GetById(int id);

        IEnumerable<T> GetAll();

        T Create(T entity);

        bool Delete(int id);
    }
}
