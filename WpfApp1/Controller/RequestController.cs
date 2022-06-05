using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class RequestController
    {
        RequestService _requestService ;
        public RequestController(RequestService requestService)
        {
            this._requestService = requestService;

        }

        public IEnumerable<Request> GetAllByDoctorId(int id)
        {
            return _requestService.GetAllByDoctorId(id);
        }

        internal Request Create(Request request)
        {
            return _requestService.Create(request);
        }
        public IEnumerable<Request> GetAllPending()
        {
            return _requestService.GetAllPending();
        }

        public Request GetById(int id)
        {
            return _requestService.GetById(id);
        }
        public Request Update(Request request)
        {
            return _requestService.Update(request);
        }

    }
}
