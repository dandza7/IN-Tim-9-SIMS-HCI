using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class RenovationService
    {
        public readonly RenovationRepository _renovationRepository;
        public readonly AppointmentRepository _appointmentRepository;

        public RenovationService(RenovationRepository renovationRepository, AppointmentRepository appointmentRepository)
        {
            _renovationRepository = renovationRepository;
            _appointmentRepository = appointmentRepository;
        }

        public Renovation Create(Renovation renovation)
        {
            return _renovationRepository.Create(renovation);
        }

        public List<String> GetBeginnings(List<int> roomsIds)
        {
            List<String> beginnings = new List<String>();
            List<Appointment> appointments = FilterAppointments(_appointmentRepository.GetAll().ToList(), roomsIds);
            List<Renovation> renovations = FilterRenovations(_renovationRepository.GetAll().ToList(), roomsIds);
            DateTime checker = DateTime.Today;
            for(int i = 0; i < 14; i++)
            {
                bool isFree = true;
                foreach(Appointment appointment in appointments)
                {
                    if (checker.ToShortDateString().Equals(appointment.Beginning.ToShortDateString()))
                    {
                        isFree = false;
                        break;
                    }
                    
                    
                }
                foreach(Renovation renovation in renovations)
                {
                    if (DateTime.Compare(DateTime.Parse(checker.ToShortDateString()), DateTime.Parse(renovation.Beginning.ToShortDateString())) >= 0
                        && DateTime.Compare(DateTime.Parse(checker.ToShortDateString()), DateTime.Parse(renovation.Ending.ToShortDateString())) <= 0)
                    {
                        isFree = false;
                        break;
                    }
                }
                if (isFree)
                {
                    beginnings.Add(checker.ToShortDateString());
                }
                checker = checker.AddDays(1);
            }
            


            return beginnings;
        }
        public List<String> GetEndings(string beginning, List<int> roomsIds)
        {
            List<String> endings = new List<String>();
            List<Appointment> appointments = FilterAppointments(_appointmentRepository.GetAll().ToList(), roomsIds);
            List<Renovation> renovations = FilterRenovations(_renovationRepository.GetAll().ToList(), roomsIds);
            DateTime checker = DateTime.Parse(beginning);
            for (int i = 0; i < 14; i++)
            {
                bool isFree = true;
                foreach (Appointment appointment in appointments)
                {
                    if (checker.ToShortDateString().Equals(appointment.Beginning.ToShortDateString()))
                    {
                        isFree = false;
                        break;
                    }


                }
                foreach (Renovation renovation in renovations)
                {
                    if (DateTime.Compare(DateTime.Parse(checker.ToShortDateString()), DateTime.Parse(renovation.Beginning.ToShortDateString())) >= 0
                        && DateTime.Compare(DateTime.Parse(checker.ToShortDateString()), DateTime.Parse(renovation.Ending.ToShortDateString())) <= 0)
                    {
                        isFree = false;
                        break;
                    }
                }
                if (isFree)
                {
                    
                    endings.Add(checker.ToShortDateString());
                } 
                else
                {
                    
                    break;
                }
                
                checker = checker.AddDays(1);
            }
            return endings;
        }
        private List<Renovation> FilterRenovations(List<Renovation> list, List<int> ids)
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
        private List<Appointment> FilterAppointments(List<Appointment> list, List<int> ids)
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
    }
}
