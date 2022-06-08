using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Service
{
    public interface Service<T>
    {
        T Create(T entity);
    }
}
