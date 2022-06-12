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
    public class DynamicEquipmentRequestRepository : IDynamicEquipmentRequestRepository
    {
        private string _path;
        private string _delimiter;

        public DynamicEquipmentRequestRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
        }

        public DynamicEquipmentRequest GetById(int id)
        {
            List<DynamicEquipmentRequest> requests = File.ReadAllLines(_path)
                .Select(ConvertCsvFormatToDynamicEquipmentRequest)
                .ToList();
            foreach (DynamicEquipmentRequest request in requests)
            {
                if (request.Id == id)
                    return request;
            }
            return null;
        }

        public List<DynamicEquipmentRequest> GetAllForUpdating()
        {
            List<DynamicEquipmentRequest> requests = File.ReadAllLines(_path)
                .Select(ConvertCsvFormatToDynamicEquipmentRequest)
                .ToList();

            List<DynamicEquipmentRequest> requestForMoving = new List<DynamicEquipmentRequest>();

            foreach (DynamicEquipmentRequest request in requests)
            {
                if (request.ArrivalDate <= DateTime.Now)
                {
                    requestForMoving.Add(request);
                }   
            }
            return requestForMoving;
        }

        public List<DynamicEquipmentRequest> GetAll()
        {
            return File.ReadAllLines(_path)
                .Select(ConvertCsvFormatToDynamicEquipmentRequest)
                .ToList();
        }

        private int GetMaxId(List<DynamicEquipmentRequest> requests)
        {
            return requests.Count() == 0 ? 0 : requests.Max(request => request.Id);
        }

        public DynamicEquipmentRequest Create(DynamicEquipmentRequest request)
        {
            request.Id = GetMaxId(GetAll()) + 1;
            AppendLineToFile(_path, ConvertDynamicEquipmentRequestToCsvFormat(request));
            return request;
        }

        private DynamicEquipmentRequest ConvertCsvFormatToDynamicEquipmentRequest(string RequestCsvFormat)
        {
            var tokens = RequestCsvFormat.Split(_delimiter.ToCharArray());
            return new DynamicEquipmentRequest(
                int.Parse(tokens[0]),
                tokens[1],
                int.Parse(tokens[2]),
                DateTime.Parse(tokens[3]));
        }

        private string ConvertDynamicEquipmentRequestToCsvFormat(DynamicEquipmentRequest request)
        {
            return string.Join(_delimiter,
                request.Id,
                request.Name,
                request.Amount,
                request.ArrivalDate);
        }

        private void AppendLineToFile(String path, String line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }

        public bool Delete(int id)
        {
            List<DynamicEquipmentRequest> requests = GetAll().ToList();
            List<string> newFile = new List<string>();
            bool isDeleted = false;
            foreach (DynamicEquipmentRequest request in requests)
            {
                if (request.Id != id)
                {
                    newFile.Add(ConvertDynamicEquipmentRequestToCsvFormat(request));
                    isDeleted = true;
                }
            }
            File.WriteAllLines(_path, newFile);
            return isDeleted;
        }
    }
}