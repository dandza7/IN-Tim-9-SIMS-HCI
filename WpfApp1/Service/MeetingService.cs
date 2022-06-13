using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;
using WpfApp1.View.Converter;
using WpfApp1.View.Model.Patient;
using WpfApp1.View.Model.Secretary;

namespace WpfApp1.Service
{
    public class MeetingService
    {
        private readonly MeetingRepository _meetingRepo;
        private readonly AppointmentRepository _appointmentRepo;
        private readonly UserRepository _userRepo;
        private readonly DoctorRepository _doctorRepo;
        private readonly RoomRepository _roomRepo;
        private readonly RenovationRepository _renovationRepo;
        public MeetingService(MeetingRepository meetingRepo, AppointmentRepository appointmentRepo,
            RoomRepository roomRepo,
            RenovationRepository renovationRepo)
        {
            _meetingRepo = meetingRepo;
            _appointmentRepo = appointmentRepo;
            _roomRepo = roomRepo;
            _renovationRepo = renovationRepo;

        }
        public IEnumerable<Meeting> GetAll()
        {
            return _meetingRepo.GetAll();
        }

        public Meeting Create(Meeting meeting)
        {
            return _meetingRepo.Create(meeting);
        }
        public bool FindMeetingTerm(
            DateTime startOfInterval, DateTime endOfInterval, List<int> userIds)
        {
            TimeMenager interval = new TimeMenager(startOfInterval, endOfInterval);
            List<Appointment> appointments = new List<Appointment>();

            foreach (int id in userIds)
            {
                List<Appointment> appointmentsForDoctor = _appointmentRepo.GetAllAppointmentsInTimeIntervalForDoctor(interval.Beginning,
                    interval.Ending,id).ToList();
                appointments.AddRange(appointmentsForDoctor);   
            }
            if (appointments.Count == 0)
            {
                return true;
            }
            else return false;
        }
        public List<MeetingView> GetMeetings(DateTime startOfInterval, DateTime endOfInterval,
            Room room )
        {
            TimeMenager interval = new TimeMenager(startOfInterval, endOfInterval);
            TimeMenagerService timeMenagerService = new TimeMenagerService(interval);

            List<MeetingView> meetings = new List<MeetingView>();
            var attendees = new List<string>();
            while (timeMenagerService.GetIncrementedBeginning() <= interval.Ending)
            {

                bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, interval.Beginning, timeMenagerService.GetIncrementedBeginning());
                if (isRoomAvailable)
                {
                    Meeting meeting = new Meeting(interval.Beginning, timeMenagerService.GetIncrementedBeginning(), room.Id, attendees);
                    meetings.Add(MeetingConverter.ConvertMeetingToMeetingView(meeting,room));
                }
                timeMenagerService.IncrementBeginning();
            }
            return meetings;
        }
    } 
}
    

