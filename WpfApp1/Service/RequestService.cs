using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class RequestService
    {
        private readonly RequestRepository _requestRepository;
        private readonly DoctorRepository _doctorRepository;

        public RequestService(RequestRepository requestRepository, DoctorRepository doctorRepository)
        {
            this._requestRepository= requestRepository;
            this._doctorRepository = doctorRepository;

        }
        public IEnumerable<Request> GetAllByDoctorId(int id)
        {
            return _requestRepository.GetAllByDoctorId(id);
        }
        public IEnumerable<Request> GetAcceptedRequests()
        {
            List<Request> requests = new List<Request>();
            foreach (Request r in _requestRepository.GetAll()) if (r.Status == Request.RequestStatusType.Accepted) requests.Add(r);
            return requests;
        }
        public bool IsEligibleForAbsence(Request request)
        {
            Doctor doctor = new Doctor();
            doctor = _doctorRepository.GetById(request.DoctorId);
            int numberOfAvailableSpecialists=_doctorRepository.GetAllDoctorsBySpecialization(doctor.Specialization).Count();
            foreach (Request r in this.GetAcceptedRequests())
            {
                if (
                    !(request.Beginning.Date > r.Ending.Date || request.Ending.Date < r.Beginning.Date))
                    if(_doctorRepository.GetById(r.DoctorId).Specialization == doctor.Specialization
                    ) numberOfAvailableSpecialists--;
                if (numberOfAvailableSpecialists <= 1) return false;
            }
            return true;
        }


        internal Request Create(Request request)
        {
            if (IsEligibleForAbsence(request)) return _requestRepository.Create(request);
            else return null;
        }
    }
}
