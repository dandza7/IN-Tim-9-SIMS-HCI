using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository.Interface;
using static WpfApp1.Model.Request;

namespace WpfApp1.Repository
{
    public class RequestRepository : IRequestRepository
    {

        private string _path;
        private string _delimiter;
        private readonly string _datetimeFormat;

        public RequestRepository(string path, string delimiter, string datetimeFormat)
        {
            _path = path;
            _delimiter = delimiter;
            _datetimeFormat = datetimeFormat;
        }
        private Request ConvertCSVFormatToRequest(string requestCSVFormat)
        {
            var tokens = requestCSVFormat.Split(_delimiter.ToCharArray());
            Enum.TryParse(tokens[3], true, out RequestStatusType type);
            //(int id, DateTime beginning, DateTime ending, AppointmentType type, bool isUrgent, int doctorId, int patientId, int roomId)
            return new Request(int.Parse(tokens[0]),
                DateTime.Parse(tokens[1]),
                DateTime.Parse(tokens[2]),
                type,
                int.Parse(tokens[4]),
                tokens[5],
                tokens[6],
                bool.Parse(tokens[7]),
                tokens[8]);

        }
        private string ConvertRequestToCSVFormat(Request request)
        {
            return string.Join(_delimiter,
                request.Id,
                request.Beginning.ToString(_datetimeFormat),
                request.Ending.ToString(_datetimeFormat),
                request.Status.ToString(),
                request.DoctorId,
                request.Title,
                request.Content,
                request.Urgnet,
                request.Comment);
        }

        public List<Request> GetAll()
        {
            List<string> lines = File.ReadAllLines(_path).ToList();
            List<Request> requests = new List<Request>();
            foreach (string line in lines)
            {
                if (line == "") continue;
                requests.Add(ConvertCSVFormatToRequest(line));
            }
            return requests;
        }
        public Request GetById(int id)
        {
            List<Request> requests = GetAll().ToList();
            return requests.FirstOrDefault(request => request.Id == id);
        }


        public IEnumerable<Request> GetAllByDoctorId(int id)
        {
            ObservableCollection<Request> doctorsRequests = new ObservableCollection<Request>();
            foreach (Request r in GetAll())
            {
                if (r.DoctorId == id) doctorsRequests.Add(r);
            }

            return doctorsRequests;
        }
        public IEnumerable<Request> GetAllPending()
        {
            ObservableCollection<Request> requests = new ObservableCollection<Request>();
            foreach (Request request in GetAll())
            {
                if (request.Status == RequestStatusType.Pending ) {requests.Add(request);}
            }

            return requests;
        }
        public Request Create(Request request)
        {
            int maxId = GetMaxId(GetAll());
            request.Id = ++maxId;
            AppendLineToFile(_path, ConvertRequestToCSVFormat(request));
            return request;
        }
        public Request Update(Request requestForUpdate)
        {
            List<Request> requests = GetAll().ToList();
            List<string> newFile = new List<string>();
            foreach (Request request in requests)
            {

                if (request.Id == requestForUpdate.Id)
                {
                    request.Status = requestForUpdate.Status;
                    request.Comment = requestForUpdate.Comment;
                }
                newFile.Add(ConvertRequestToCSVFormat(request));
            }
            File.WriteAllLines(_path, newFile);
            return requestForUpdate;
        }


        private int GetMaxId(IEnumerable<Request> requests)
        {
            return requests.Count() == 0 ? 0 : requests.Max(request => request.Id);
        }
        private void AppendLineToFile(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }
    }
}
