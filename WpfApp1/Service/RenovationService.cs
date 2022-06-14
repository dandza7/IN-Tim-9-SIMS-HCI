using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Model.Preview;
using WpfApp1.Repository;
using WpfApp1.Repository.Interface;
using WpfApp1.Repository.Interfaces;

namespace WpfApp1.Service
{
    public class RenovationService
    {
        public readonly IRenovationRepository _renovationRepository;
        public readonly IAppointmentRepository _appointmentRepository;
        public readonly IRoomRepository _roomRepository;

        public RenovationService(IRenovationRepository renovationRepository, IAppointmentRepository appointmentRepository, IRoomRepository roomRepository)
        {
            _renovationRepository = renovationRepository;
            _appointmentRepository = appointmentRepository;
            _roomRepository = roomRepository;

        }

        public Renovation Create(Renovation renovation)
        {
            return _renovationRepository.Create(renovation);
        }



        public List<String> GetDaysAvailableForRenovation(List<int> roomsIds, string beginning = "")
        {
            List<String> days = new List<String>();
            List<Appointment> appointments = FilterAppointmentsByRooms(_appointmentRepository.GetAll().ToList(), roomsIds);
            List<Renovation> renovations = FilterRenovationsByRooms(_renovationRepository.GetAll().ToList(), roomsIds);
            DateTime checker = beginning == "" ? DateTime.Today : DateTime.Parse(beginning);
            for (int i = 0; i < 14; i++)
            {
                if (CheckForOverlapingForAppointments(appointments, checker) && CheckForOverlapingForRenovations(renovations, checker))
                {
                    days.Add(checker.ToShortDateString());
                }
                else if (beginning != "")
                {
                    break;
                }

                checker = checker.AddDays(1);
            }
            return days;
        }
        public bool CheckForOverlapingForAppointments(List<Appointment> appointments, DateTime day)
        {
            foreach(Appointment a in appointments)
            {
                DateTime beginning = DateTime.Parse(a.Beginning.ToShortDateString());
                if (DateTime.Compare(beginning, day) == 0)
                {
                    return false;
                }
            }
            return true;
        }
        public bool CheckForOverlapingForRenovations(List<Renovation> renovations, DateTime day)
        {
            foreach (Renovation r in renovations)
            {
                DateTime beginning = DateTime.Parse(r.Beginning.ToShortDateString());
                DateTime ending = DateTime.Parse(r.Ending.ToShortDateString());
                if (DateTime.Compare(day, beginning) >= 0
                    && DateTime.Compare(day, ending) <= 0)
                {
                    return false;
                }
            }
            return true;
        }
        private List<Renovation> FilterRenovationsByRooms(List<Renovation> list, List<int> ids)
        {
            List<Renovation> retVal = new List<Renovation>();
            foreach (Renovation renovation in list)
            {
                var isc = ids.Intersect(renovation.RoomsIds);
                if (isc.Count() != 0)
                {
                    retVal.Add(renovation);
                }
            }
            return retVal;
        }
        private List<Appointment> FilterAppointmentsByRooms(List<Appointment> list, List<int> ids)
        {
            List<Appointment> retVal = new List<Appointment>();
            foreach (Appointment appointment in list)
            {

                if (ids.Contains(appointment.RoomId))
                {
                    retVal.Add(appointment);
                }
            }
            return retVal;
        }
        public void ExecuteFinishedAdvancedRenovations()
        {
            List<Renovation> renovations = _renovationRepository.GetAll().ToList();
            foreach (Renovation renovation in renovations)
            {
                if (renovation.Type == "A" && DateTime.Compare(DateTime.Today, DateTime.Parse(renovation.Ending.ToShortDateString())) >= 0)
                {
                    foreach (int id in renovation.RoomsIds)
                    {
                        Room r = _roomRepository.GetById(id);
                        if (!r.IsActive)
                        {
                            r.IsActive = true;
                            _roomRepository.Update(r);
                        }
                        else
                        {
                            _roomRepository.Delete(r.Id);
                        }
                    }
                    _renovationRepository.Delete(renovation.Id);
                }
            }
        }
        public void CancelRenovations(int roomId)
        {
            List<Renovation> renovations = this._renovationRepository.GetAll().ToList();
            foreach (Renovation renovation in renovations)
            {
                if (renovation.RoomsIds.Contains(roomId))
                    _renovationRepository.Delete(renovation.Id);
            }
        }
        //KOD ZA HCI, SIMONA, NE GLEDAJTE OVO :)

        public List<BusynessPreview> GetBusynessPreview()
        {
            List<Appointment> apps = _appointmentRepository.GetAll().ToList();
            List<Renovation> rens = _renovationRepository.GetAll().ToList();
            List<BusynessPreview> retVal = new List<BusynessPreview>();
            foreach (Appointment appointment in apps)
            {
                retVal.Add(new BusynessPreview(_roomRepository.GetById(appointment.RoomId).Nametag, "Appointment", appointment.Beginning, appointment.Ending));
            }
            foreach (Renovation renovation in rens)
            {
                foreach (int id in renovation.RoomsIds)
                {
                    retVal.Add(new BusynessPreview(_roomRepository.GetById(id).Nametag, "Renovation", renovation.Beginning, renovation.Ending));
                }
            }
            return retVal;
        }

    }
}
