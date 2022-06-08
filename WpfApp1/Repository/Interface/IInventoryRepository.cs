using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository.Interface
{
    public interface IInventoryRepository
    {
        IEnumerable<Inventory> GetAll();
        Inventory GetById(int id);
        Inventory Create(Inventory inv);
        Inventory Update(Inventory inv);
        bool Delete(int id);
        void UpdateAll(List<Inventory> inventories);
        Inventory AddAmount(string name, int amount);
        IEnumerable<Inventory> GetAllDynamic();
        Inventory GetByName(string name);
    }
}
