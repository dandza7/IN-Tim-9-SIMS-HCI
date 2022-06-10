using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository.Interface;

namespace WpfApp1.Repository
{
    public class InventoryMovingRepository : IInventoryMovingRepository
    {
        private const string NOT_FOUND_ERROR = "Inventory moving with {0}:{1} can not be found!";
        private string _path;
        private string _delimiter;

        public InventoryMovingRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
        }

        public InventoryMoving GetById(int id)
        {
            List<InventoryMoving> invMovs = File.ReadAllLines(_path)
                .Select(ConvertCsvFormatToInventoryMoving)
                .ToList();
            foreach (InventoryMoving invMov in invMovs)
            {
                if (invMov.Id == id)
                    return invMov;
            }
            return null;
        }

        public IEnumerable<InventoryMoving> GetAll()
        {
            return File.ReadAllLines(_path)
                .Select(ConvertCsvFormatToInventoryMoving)
                .ToList();
        }

        private int GetMaxId(List<InventoryMoving> invMovs)
        {
            return invMovs.Count() == 0 ? 0 : invMovs.Max(invMov => invMov.Id);
        }

        public InventoryMoving Create(InventoryMoving invMov)
        {
            invMov.Id = GetMaxId(GetAll().ToList()) + 1;
            AppendLineToFile(_path, ConvertInventoryMovingToCsvFormat(invMov));
            return invMov;
        }

        private InventoryMoving ConvertCsvFormatToInventoryMoving(string inventoryMovingCsvFormat)
        {
            var tokens = inventoryMovingCsvFormat.Split(_delimiter.ToCharArray());
            string format = "dd/MM/yyyy H:mm:ss";
            return new InventoryMoving(
                int.Parse(tokens[0]),
                int.Parse(tokens[1]),
                int.Parse(tokens[2]),
                DateTime.ParseExact(tokens[3], format,
                CultureInfo.InvariantCulture));
        }

        private string ConvertInventoryMovingToCsvFormat(InventoryMoving invMov)
        {
            return string.Join(_delimiter,
                invMov.Id,
                invMov.InventoryId,
                invMov.RoomId,
                invMov.MovingDate);
        }

        private void AppendLineToFile(String path, String line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }

        public bool Delete(int id)
        {
            List<InventoryMoving> invMovs = GetAll().ToList();
            List<string> newFile = new List<string>();
            bool isDeleted = false;
            foreach (InventoryMoving i in invMovs)
            {
                if (i.Id != id)
                {
                    newFile.Add(ConvertInventoryMovingToCsvFormat(i));
                    isDeleted = true;
                }
            }
            File.WriteAllLines(_path, newFile);
            return isDeleted;
        }

       
    }
}
