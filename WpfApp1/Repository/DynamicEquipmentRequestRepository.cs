using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository
{
    public class DynamicEquipmentRequestRepository
    {
        private const string NOT_FOUND_ERROR = "Inventory moving with {0}:{1} can not be found!";
        private string _path;
        private string _delimiter;

        public DynamicEquipmentRequestRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
        }

        public DynamicEquipmentRequest Get(int id)
        {
            List<DynamicEquipmentRequest> dyneqRequests = File.ReadAllLines(_path)
                .Select(ConvertCsvFormatToDynamicEquipmentRequest)
                .ToList();
            foreach (DynamicEquipmentRequest dynreq in dyneqRequests)
            {
                if (dynreq.Id == id)
                    return dynreq;
            }
            return null;
        }

        public List<DynamicEquipmentRequest> GetAllForUpdating()
        {
            List<DynamicEquipmentRequest> dyneqRequests = File.ReadAllLines(_path)
                .Select(ConvertCsvFormatToDynamicEquipmentRequest)
                .ToList();
            List<DynamicEquipmentRequest> dyneqRequestForMoving = new List<DynamicEquipmentRequest>();
            foreach (DynamicEquipmentRequest dynreq in dyneqRequests)
            {
                if (dynreq.RequestDate <= DateTime.Now)
                {
                    dyneqRequestForMoving.Add(dynreq);
                }   
            }
            return dyneqRequestForMoving;
        }

        public List<DynamicEquipmentRequest> GetAll()
        {
            return File.ReadAllLines(_path)
                .Select(ConvertCsvFormatToDynamicEquipmentRequest)
                .ToList();
        }

        private int GetMaxId(List<DynamicEquipmentRequest> dynReqs)
        {
            return dynReqs.Count() == 0 ? 0 : dynReqs.Max(dynReq => dynReq.Id);
        }

        public DynamicEquipmentRequest Create(DynamicEquipmentRequest dynReq)
        {
            Console.WriteLine("KREIRANJE");
            dynReq.Id = GetMaxId(GetAll()) + 1;
            AppendLineToFile(_path, ConvertDynamicEquipmentRequestToCsvFormat(dynReq));
            return dynReq;
        }

        private DynamicEquipmentRequest ConvertCsvFormatToDynamicEquipmentRequest(string DynamicEquipmentRequestCsvFormat)
        {
            var tokens = DynamicEquipmentRequestCsvFormat.Split(_delimiter.ToCharArray());
            string format = "dd/MM/yyyy H:mm:ss";
            return new DynamicEquipmentRequest(
                int.Parse(tokens[0]),
                tokens[1],
                int.Parse(tokens[2]),
                DateTime.Parse(tokens[3]));
        }

        private string ConvertDynamicEquipmentRequestToCsvFormat(DynamicEquipmentRequest dynReq)
        {
            return string.Join(_delimiter,
                dynReq.Id,
                dynReq.Name,
                dynReq.Amount,
                dynReq.RequestDate);
        }

        private void AppendLineToFile(String path, String line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }

        public bool Delete(int id)
        {
            List<DynamicEquipmentRequest> dynRequests = GetAll().ToList();
            List<string> newFile = new List<string>();
            bool isDeleted = false;
            foreach (DynamicEquipmentRequest der in dynRequests)
            {
                if (der.Id != id)
                {
                    newFile.Add(ConvertDynamicEquipmentRequestToCsvFormat(der));
                    isDeleted = true;
                }
            }
            File.WriteAllLines(_path, newFile);
            return isDeleted;
        }
    }
}