using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository.Interface;

namespace WpfApp1.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private const string NOT_FOUND_ERROR = "Inventory with {0}:{1} can not be found!";
        private string _path;
        private string _delimiter;

        public InventoryRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
        }

        public Inventory Get(int id)
        {
            List<Inventory> invs = File.ReadAllLines(_path)
                                    .Select(ConvertCsvFormatToInventory)
                                    .ToList();
            foreach (Inventory inv in invs)
            {
                if (inv.Id == id)
                    return inv;
            }
            return null;
        }

        public IEnumerable<Inventory> GetAll()
        {
            return File.ReadAllLines(_path)
                .Select(ConvertCsvFormatToInventory)
                .ToList();
        }

        public Inventory GetById(int id)
        {
            return GetAll().ToList().SingleOrDefault(Inventory => Inventory.Id == id);
        }

        public Inventory GetByName(string name)
        {
            Inventory inv =  GetAll().ToList().SingleOrDefault(Inventory => Inventory.Name == name);
            if(inv == null)
            {
                return null;
            }
            else return inv;
        }

        public IEnumerable<Inventory> GetAllDynamic()
        {
            List<Inventory> allInventory = GetAll().ToList();
            List<Inventory> dynamicInventory = new List<Inventory>();

            foreach (Inventory inv in allInventory)
            {
                if (inv.Type == "D")
                {
                    dynamicInventory.Add(inv);
                }
            }

            return dynamicInventory;
        }


        private int GetMaxId(List<Inventory> inventories)
        {
            return inventories.Count() == 0 ? 0 : inventories.Max(inventory => inventory.Id);
        }

        public Inventory Create(Inventory inventory)
        {
            inventory.Id = GetMaxId(GetAll().ToList()) + 1;
            AppendLineToFile(_path, ConvertInventoryToCsvFormat(inventory));
            return inventory;
        }
        public Inventory AddAmount(string name, int amount)
        {
            List<Inventory> dynInv = GetAll().ToList();
            List<string> newFile = new List<string>();
            foreach (Inventory inv in dynInv)
            {
                if (inv.Name == name)
                {
                    inv.Amount = inv.Amount + amount;
                }
                newFile.Add(ConvertInventoryToCsvFormat(inv));
            }
            File.WriteAllLines(_path, newFile);
            return GetByName(name);
        }

        private Inventory ConvertCsvFormatToInventory(string inventoryCsvFormat)
        {
            var tokens = inventoryCsvFormat.Split(_delimiter.ToCharArray());
            return new Inventory(
                int.Parse(tokens[0]),
                int.Parse(tokens[1]),
                tokens[2],
                tokens[3],
                int.Parse(tokens[4]));
        }

        private string ConvertInventoryToCsvFormat(Inventory inventory)
        {
            return string.Join(_delimiter,
                inventory.Id,
                inventory.RoomId,
                inventory.Name,
                inventory.Type,
                inventory.Amount);
        }

        private void AppendLineToFile(String path, String line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }

        public bool Delete(int id)
        {
            List<Inventory> inventories = GetAll().ToList();
            List<string> newFile = new List<string>();
            bool isDeleted = false;
            foreach (Inventory i in inventories)
            {
                if (i.Id != id)
                {
                    newFile.Add(ConvertInventoryToCsvFormat(i));
                    isDeleted = true;
                }
            }
            File.WriteAllLines(_path, newFile);
            return isDeleted;
        }

        public Inventory Update(Inventory inventory)
        {
            List<Inventory> inventories = GetAll().ToList();
            List<string> newFile = new List<string>();
            foreach (Inventory i in inventories)
            {

                if (i.Id == inventory.Id)
                {
                    i.Name = inventory.Name;
                    i.RoomId = inventory.RoomId;
                    i.Amount = inventory.Amount;
                    i.Type = inventory.Type;
                }
                newFile.Add(ConvertInventoryToCsvFormat(i));
            }

            File.WriteAllLines(_path, newFile);
            return inventory;
        }
        public void UpdateAll(List<Inventory> inventories)
        {
            List<string> newFile = new List<string>();
            foreach (Inventory inventory in inventories)
            {
                newFile.Add(ConvertInventoryToCsvFormat(inventory));
            }
            File.WriteAllLines(_path, newFile);
        }
    }
}
