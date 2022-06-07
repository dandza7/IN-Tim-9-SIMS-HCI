using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Service
{
    public interface Service<T>
    {
        IEnumerable<T> GetAll();

        T GetById(int id);

        T Create(T entity);

        bool Delete(int id);

    }
}
