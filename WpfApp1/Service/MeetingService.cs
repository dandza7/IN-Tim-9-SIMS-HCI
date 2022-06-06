using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;
using WpfApp1.View.Converter;
using WpfApp1.View.Model.Patient;

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
        public List<AppointmentView> GetAvailableMeetingOptions(
            DateTime startOfInterval, DateTime endOfInterval, List<int> doctorIds)
        {
            TimeMenager interval = new TimeMenager(startOfInterval, endOfInterval);
           
            List<AppointmentView> appointments = new List<AppointmentView>();

                int doctorId = doctorIds[0];
            Console.WriteLine(doctorId);
            List<Appointment> appointmentsForDoctor = _appointmentRepo.GetAllAppointmentsInTimeIntervalForDoctor(interval.Beginning,
                    interval.Ending,
                    doctorId).ToList();

                Doctor doctor = _doctorRepo.GetById(doctorId);
                Room room = _roomRepo.Get(doctor.RoomId);
                User doctorUser = _userRepo.GetById(doctorId);

                if (appointmentsForDoctor.Count == 0)
                {
                Console.WriteLine("ttt");
                    return GetAppointmentsForFreeTimeInterval(interval.Beginning, interval.Ending, appointments, room, doctor, doctorUser);
                }
                
            

            return appointments;
        }
        private List<AppointmentView> GetAppointmentsForFreeTimeInterval(DateTime startOfInterval, DateTime endOfInterval,
    List<AppointmentView> appointments, Room room, Doctor doctor, User doctorUser)
        {
            TimeMenager interval = new TimeMenager(startOfInterval, endOfInterval);
            while (interval.GetIncrementedBeginning() <= interval.Ending)
            {
                if (interval.AreAvailableAppointmentsCollected(appointments)) return appointments;

                bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, interval.Beginning, interval.GetIncrementedBeginning());
                if (isRoomAvailable)
                {
                    Appointment freeAppointment = new Appointment(interval.Beginning, interval.GetIncrementedBeginning(), Appointment.AppointmentType.regular, false, doctor.Id, 5, doctor.RoomId);
                    appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                }
                interval.IncrementBeginning();
            }
            return appointments;
        }
    } 
}
    

