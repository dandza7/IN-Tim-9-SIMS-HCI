using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository
{
    public class InventoryRepository
    {
        private const string NOT_FOUND_ERROR = "Inventory with {0}:{1} can not be found!";
        private string _path;
        private string _delimiter;
        private int _inventoryMaxId;

        public InventoryRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
            _inventoryMaxId = GetMaxId(GetAll());
        }

        public Inventory Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Inventory> GetAll()
        {
            return File.ReadAllLines(_path)
                .Select(ConvertCsvFormatToInventory)
                .ToList();
        }

        private int GetMaxId(List<Inventory> inventories)
        {
            return inventories.Count() == 0 ? 0 : inventories.Max(inventory => inventory.Id);
        }

        public Inventory Create(Inventory inventory)
        {
            inventory.Id = ++_inventoryMaxId;
            AppendLineToFile(_path, ConvertInventoryToCsvFormat(inventory));
            return inventory;
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
                }
                newFile.Add(ConvertInventoryToCsvFormat(i));
            }
            File.WriteAllLines(_path, newFile);
            return inventory;
        }
    }
}
