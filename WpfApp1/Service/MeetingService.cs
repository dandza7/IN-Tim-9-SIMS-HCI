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
            DoctorRepository doctorRepository,
            PatientRepository patientRepo,
            RoomRepository roomRepo,
            UserRepository userRepo,
            RenovationRepository renovationRepo)
        {
            _meetingRepo = meetingRepo;
            _appointmentRepo = appointmentRepo;
            _userRepo = userRepo;
            _doctorRepo = doctorRepository;
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
        public List<MeetingView> GetAvailableMeetingOptions(
            DateTime startOfInterval, DateTime endOfInterval, List<int> userIds, Room room)
        {
            TimeMenager interval = new TimeMenager(startOfInterval, endOfInterval);
            List<MeetingView> meetings = new List<MeetingView>();
            List<Appointment> appointments = new List<Appointment>();

            foreach (int id in userIds)
            {
                List<Appointment> appointmentsForDoctor = _appointmentRepo.GetAllAppointmentsInTimeIntervalForDoctor(interval.Beginning,
                    interval.Ending,id).ToList();
                appointments.AddRange(appointmentsForDoctor);   
            }
            if (appointments.Count == 0)
            {
                return GetMeetings(interval.Beginning, interval.Ending, meetings, room);
            }
            else return meetings;
        }
        private List<MeetingView> GetMeetings(DateTime startOfInterval, DateTime endOfInterval,
            List<MeetingView> meetings, Room room )
        {
            TimeMenager interval = new TimeMenager(startOfInterval, endOfInterval);
            var attendees = new List<string>();
            while (interval.GetIncrementedBeginning() <= interval.Ending)
            {

                bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, interval.Beginning, interval.GetIncrementedBeginning());
                if (isRoomAvailable)
                {
                    Meeting meeting = new Meeting(interval.Beginning, interval.GetIncrementedBeginning(), room.Id, attendees);
                    meetings.Add(MeetingConverter.ConvertMeetingToMeetingView(meeting,room));
                }
                interval.IncrementBeginning();
            }
            return meetings;
        }
    } 
}
    

