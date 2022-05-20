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
using static WpfApp1.Model.Appointment;
using static WpfApp1.Model.Doctor;

namespace WpfApp1.Service
{
    public class AppointmentService
    {
        private readonly AppointmentRepository _appointmentRepo;
        private readonly DoctorRepository _doctorRepo;
        private readonly PatientRepository _patientRepo;
        private readonly RoomRepository _roomRepo;
        private readonly UserRepository _userRepo;
        private readonly RenovationRepository _renovationRepo;
        private readonly NotificationRepository _notificationRepo;
        public AppointmentService(AppointmentRepository appointmentRepo, 
            DoctorRepository doctorRepository, 
            PatientRepository patientRepo,
            RoomRepository roomRepo,
            UserRepository userRepo,
            RenovationRepository renovationRepo,
            NotificationRepository notificationRepo
            )
        {
            _appointmentRepo = appointmentRepo;
            _doctorRepo = doctorRepository;
            _patientRepo = patientRepo;
            _roomRepo = roomRepo;
            _userRepo = userRepo;
            _renovationRepo = renovationRepo;
            _notificationRepo = notificationRepo;
        }

        public IEnumerable<Appointment> GetAll()
        {
            return _appointmentRepo.GetAll();
        }

        public Appointment Create(Appointment appointment)
        {
            return _appointmentRepo.Create(appointment);

        }

        public Appointment Update(Appointment appointment)
        {
            return _appointmentRepo.Update(appointment);
        }

        public bool Delete(int appointmentId)
        {
            return _appointmentRepo.Delete(appointmentId);
        }

        public Appointment GetById(int appointmentId)
        {
            return _appointmentRepo.GetById(appointmentId);
        }

        public bool AppointmentCancellationByPatient(int patientId, int appointmentId)
        {
            Patient patient = _patientRepo.GetById(patientId);
            DateTime lastCancellationDate = patient.LastCancellationDate;

            if (lastCancellationDate.AddMonths(1) < DateTime.Now)
            {
                patient.NumberOfCancellations = 1;
                patient.LastCancellationDate = DateTime.Now;
                _patientRepo.Update(patient);

            } else {
                patient.NumberOfCancellations += 1;
                patient.LastCancellationDate = DateTime.Now;
                _patientRepo.Update(patient);
            }

            return _appointmentRepo.Delete(appointmentId);
        }

        internal List<Appointment> GetAllByDoctorId(int id)
        {
            return _appointmentRepo.GetAllByDoctorId(id);
        }

        private List<AppointmentView> GetAppointmentsForFreeTimeInterval(DateTime startOfInterval, DateTime endOfInterval,
            List<AppointmentView> appointments, Room room, Doctor doctor, User doctorUser, int patientId)
        {
            TimeMenager interval = new TimeMenager(startOfInterval, endOfInterval);
            while (interval.GetIncrementedBeginning() <= interval.Ending)
            {
                interval.MoveStartOfIntervalIfNeeded();
                if (interval.IncrementBeginning() > interval.Ending) return appointments;
                if (appointments.Count == 10) return appointments;
                bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, interval.Beginning, interval.GetIncrementedBeginning());
                if (isRoomAvailable)
                {
                    Appointment freeAppointment = new Appointment(interval.Beginning, interval.GetIncrementedBeginning(), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                    appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                }
                interval.IncrementBeginning();
            }
            return appointments;
        }

        private List<AppointmentView> GetAppointmentsHappyCase(DateTime startOfInterval, DateTime endOfInterval,
            List<Appointment> appointmentsOfDoctor, List<AppointmentView> appointments, Room room, Doctor doctor, User doctorUser, int patientId)
        {
            TimeMenager interval = new TimeMenager(startOfInterval, endOfInterval);
            interval.MoveStartOfIntervalIfNeeded();
            if (interval.GetIncrementedBeginning() > interval.Ending) return appointments;

            foreach (Appointment appointment in appointmentsOfDoctor)
            {
                while (interval.GetIncrementedBeginning() <= appointment.Beginning)
                {
                    interval.MoveStartOfIntervalIfNeeded();
                    if (interval.GetIncrementedBeginning() > interval.Ending) return appointments;
                    if (appointments.Count == 10) return appointments;
                    bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, interval.Beginning, interval.GetIncrementedBeginning());
                    if (isRoomAvailable)
                    {
                        Appointment freeAppointment = new Appointment(interval.Beginning, interval.GetIncrementedBeginning(), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                        appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                    }
                    interval.IncrementBeginning();
                }
                interval.Beginning = appointment.Ending;
            }

            while (interval.GetIncrementedBeginning() <= interval.Ending)
            {
                interval.MoveStartOfIntervalIfNeeded();
                if (interval.GetIncrementedBeginning() > interval.Ending) return appointments;
                if (appointments.Count == 10) return appointments;
                bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, interval.Beginning, interval.GetIncrementedBeginning());
                if (isRoomAvailable)
                {
                    Appointment freeAppointment = new Appointment(interval.Beginning, interval.GetIncrementedBeginning(), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                    appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                }
                interval.IncrementBeginning();
            }
            return appointments;
        }

        private (DateTime, DateTime) AdjustSearchingTimeInterval(DateTime startOfInterval, DateTime endOfInterval, int oldAppointmentId)
        {
            TimeMenager interval = new TimeMenager(startOfInterval, endOfInterval);
            interval.TrimExcessiveTime();

            if (oldAppointmentId != -1)
            {
                Appointment oldAppointment = _appointmentRepo.GetById(oldAppointmentId);
                if (oldAppointment.Ending.AddDays(4) < interval.Ending) interval.Ending = oldAppointment.Ending.AddDays(4);
                if (oldAppointment.Beginning.AddDays(-4) > interval.Beginning) interval.Beginning = oldAppointment.Beginning.AddDays(-4);
            }

            return (interval.Beginning, interval.Ending);
        }

        private List<AppointmentView> GetAppointmentFromDoctorOfSpec(List<AppointmentView> appointments, DateTime startOfInterval, DateTime endOfInterval, List<Doctor> doctorsOfThatSpec, int patientId)
        {
            TimeMenager interval = new TimeMenager(startOfInterval, endOfInterval);
            foreach (Doctor doctor in doctorsOfThatSpec)
            {
                interval.MoveStartOfIntervalIfNeeded();
                if (interval.GetIncrementedBeginning() > interval.Ending) return appointments;
                if (appointments.Count == 10) return appointments;

                List<Appointment> doctorsAppointments = _appointmentRepo.GetAllAppointmentsInTimeIntervalForDoctor(interval.Beginning, interval.GetIncrementedBeginning(), doctor.Id).ToList();
                if (doctorsAppointments.Count == 0)
                {
                    interval.MoveStartOfIntervalIfNeeded();
                    if (interval.GetIncrementedBeginning() > interval.Ending) return appointments;

                    User doctorUser = _userRepo.GetById(doctor.Id);
                    Room doctorRoom = _roomRepo.Get(doctor.RoomId);
                    bool isRoomAvailable = _renovationRepo.IsRoomAvailable(doctorRoom.Id, interval.Beginning, interval.GetIncrementedBeginning());

                    if (isRoomAvailable)
                    {
                        Appointment freeAppointment = new Appointment(interval.Beginning, interval.GetIncrementedBeginning(), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                        appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, doctorRoom));
                    }
                    interval.IncrementBeginning();
                }
            }
            return appointments;
        }

        // Ukoliko je riječ o dodavanju novog appointmenta onda pri pozivu funkcije treba proslijediti -1 za oldAppointmentId 
        // dok se pri pomjeranju postojećeg appointmenta za oldAppointmentId prosleđuje Id appointmenta koji se pomjera
        public List<AppointmentView> GetAvailableAppointmentOptions(string priority,
            DateTime startOfInterval, DateTime endOfInterval, int doctorId, SpecType specialization, int patientId, int oldAppointmentId)
        {

            (startOfInterval, endOfInterval) = AdjustSearchingTimeInterval(startOfInterval, endOfInterval, oldAppointmentId);
            TimeMenager interval = new TimeMenager(startOfInterval, endOfInterval);

            List<AppointmentView> appointments = new List<AppointmentView>();
            List<Appointment> appointmentsForDoctor = _appointmentRepo.GetAllAppointmentsInTimeIntervalForDoctor(interval.Beginning,
                interval.Ending,
                doctorId).ToList();

            Doctor doctor = _doctorRepo.GetById(doctorId);
            Room room = _roomRepo.Get(doctor.RoomId);
            User doctorUser = _userRepo.GetById(doctorId);

            if (appointmentsForDoctor.Count == 0 && interval.GetIncrementedBeginning() <= interval.Ending)
            {
                return GetAppointmentsForFreeTimeInterval(interval.Beginning, interval.Ending, appointments, room, doctor, doctorUser, patientId);
            }

            appointments = GetAppointmentsHappyCase(interval.Beginning, interval.Ending, appointmentsForDoctor, appointments, room, doctor, doctorUser, patientId);

            if (appointments.Count == 0 && priority.Equals("Doctor"))
            {
                appointments = GetAppointmentsWithPriorityOfDoctor(interval.Beginning, doctorId, patientId, oldAppointmentId, doctor, room, doctorUser);
            } else if (appointments.Count == 0 && priority.Equals("Time")) { 
                appointments = GetAppointmentsWithPriorityOfTime(specialization, interval.Beginning, interval.Ending, doctorId, patientId, oldAppointmentId);
            }

            return appointments;
        }

        private List<AppointmentView> GetAppointmentsWithPriorityOfDoctor(DateTime endOfInterval, int doctorId, int patientId, int oldAppointmentId, Doctor doctor, Room room, User doctorUser)
        {
            List<AppointmentView> appointments = new List<AppointmentView>();
            List<Appointment> doctorsAppointments = _appointmentRepo.GetAllAppointmentsForDoctor(doctorId).ToList();
            TimeMenager interval = new TimeMenager(endOfInterval, endOfInterval);
            interval.MoveStartOfIntervalToTheNextDay();
                
            foreach(Appointment appointment in doctorsAppointments)
            {
                while(interval.GetIncrementedBeginning() <= appointment.Beginning)
                {
                    interval.MoveStartOfIntervalIfNeeded();
                    if (appointments.Count == 5) return appointments;
                    bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, interval.Beginning, interval.GetIncrementedBeginning());

                    if (isRoomAvailable)
                    {
                        Appointment freeAppointment = new Appointment(interval.Beginning, interval.GetIncrementedBeginning(), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                        appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                    }
                    interval.IncrementBeginning();
                }
                if (appointment.Beginning > interval.Beginning) interval.Beginning = appointment.Ending;
            }
            while (appointments.Count < 5)
            {
                interval.MoveStartOfIntervalIfNeeded();
                bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, interval.Beginning, interval.GetIncrementedBeginning());
                if (isRoomAvailable)
                {
                    Appointment freeAppointment2 = new Appointment(interval.Beginning, interval.GetIncrementedBeginning(), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                    appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment2, doctorUser, room));
                }
                interval.IncrementBeginning();
            }
            return appointments;
        }

        private List<AppointmentView> GetAppointmentsWithPriorityOfTime(SpecType specialization, DateTime startOfInterval, DateTime endOfInterval, int doctorId, int patientId, int oldAppointmentId)
        {
            List<AppointmentView> appointments = new List<AppointmentView>();
            List<Doctor> doctorsBySpec = _doctorRepo.GetAllDoctorsBySpecialization(specialization).ToList();
            List<Appointment> appointmentsInInterval = _appointmentRepo.GetAllAppointmentsInTimeInterval(startOfInterval, endOfInterval).ToList();

            TimeMenager interval = new TimeMenager(startOfInterval, endOfInterval);
            interval.MoveStartOfIntervalIfNeeded();
            if (interval.GetIncrementedBeginning() > interval.Ending) return appointments;

            appointments = GetAppointmentFromDoctorOfSpec(appointments, interval.Beginning, interval.GetIncrementedBeginning(), doctorsBySpec, patientId);

            if (appointments.Count == 0)
            {
                foreach(Appointment appointmentInInterval in appointmentsInInterval)
                {
                    interval.MoveStartOfIntervalIfNeeded();
                    if (interval.GetIncrementedBeginning() > interval.Ending) return appointments;

                    interval.Beginning = appointmentInInterval.Ending;
                    if (interval.GetIncrementedBeginning() > interval.Ending) return appointments;

                    appointments = GetAppointmentFromDoctorOfSpec(appointments, interval.Beginning, interval.GetIncrementedBeginning(), doctorsBySpec, patientId);
                }
            }
            return appointments;
        }

        public List<AppointmentView> GetPatientsAppointmentsView(int patientId)
        {
            List<AppointmentView> appointmentViews = new List<AppointmentView>();
            List<Appointment> appointments = _appointmentRepo.GetAll().ToList();
            foreach (Appointment appointment in appointments)
            {
                if (appointment.PatientId == patientId && appointment.Beginning > DateTime.Now)
                {
                    Doctor doctor = _doctorRepo.GetById(appointment.DoctorId);
                    User user = _userRepo.GetById(doctor.Id);
                    Room room = _roomRepo.Get(doctor.RoomId);
                    appointmentViews.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(appointment, user, room));
                }
            }
            return appointmentViews.OrderBy(appointment => appointment.Beginning).ToList(); ;
        }

        public List<AppointmentView> GetPatientsReportsView(int patientId)
        {
            List<AppointmentView> appointmentViews = new List<AppointmentView>();
            List<Appointment> appointments = _appointmentRepo.GetAll().ToList();
            foreach (Appointment appointment in appointments)
            {
                if (appointment.PatientId == patientId && appointment.Ending < DateTime.Now)
                {
                    Doctor doctor = _doctorRepo.GetById(appointment.DoctorId);
                    User user = _userRepo.GetById(doctor.Id);
                    Room room = _roomRepo.Get(doctor.RoomId);
                    appointmentViews.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(appointment, user, room));
                }
            }
            return appointmentViews.OrderBy(appointment => appointment.Beginning).ToList(); 
        }

        public List<AppointmentView> GetPatientsReportsInTimeInterval(int patientId, DateTime startOfInterval, DateTime endOfInterval)
        {
            List<AppointmentView> appointmentViews = new List<AppointmentView>();
            List<Appointment> appointments = _appointmentRepo.GetAll().ToList();
            foreach (Appointment appointment in appointments)
            {
                if (appointment.PatientId == patientId && appointment.Ending < DateTime.Now 
                    && appointment.Beginning > startOfInterval && appointment.Ending < endOfInterval)
                {
                    Doctor doctor = _doctorRepo.GetById(appointment.DoctorId);
                    User user = _userRepo.GetById(doctor.Id);
                    Room room = _roomRepo.Get(doctor.RoomId);
                    appointmentViews.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(appointment, user, room));
                }
            }
            return appointmentViews.OrderBy(appointment => appointment.Beginning).ToList();
        }

        public List<SecretaryAppointmentView> GetSecretaryAppointmentViews()
        {
            List<SecretaryAppointmentView> appointmentViews = new List<SecretaryAppointmentView>();
            List<Appointment> appointments = _appointmentRepo.GetAll().ToList();
            foreach (Appointment appointment in appointments)
            {
                User doctor = _userRepo.GetById(_doctorRepo.GetById(appointment.DoctorId).Id);
                User patient = _userRepo.GetById(_patientRepo.GetById(appointment.PatientId).Id);
                appointmentViews.Add(AppointmentConverter.ConvertSecretaryAppointmentSecretaryAppointmentView(appointment, doctor, patient));
            }
            return appointmentViews;
        }

        public bool CreateUrgentAppointment(int patientId, SpecType spec, DateTime startOfInterval)
        {

            List<Doctor> doctors = (List<Doctor>)_doctorRepo.GetAllDoctorsBySpecialization(spec);
            DateTime endOfInterval = startOfInterval.AddHours(1);
            int freedoctorId = -1;

            foreach (Doctor doctor in doctors)
            {
                List<Appointment> appointmentsOfDoctor = _appointmentRepo.GetAllAppointmentsInTimeIntervalForDoctor(startOfInterval,
                    endOfInterval, doctor.Id).ToList();

                if (appointmentsOfDoctor.Count() == 0)
                {
                    freedoctorId = doctor.Id;
                }
            }

            if (freedoctorId != -1)
            {
                Appointment a = new Appointment(startOfInterval, endOfInterval, Appointment.AppointmentType.regular, true,
                    freedoctorId, patientId, _doctorRepo.GetById(freedoctorId).RoomId);
                _appointmentRepo.Create(a);

                string notificationTitle = "You have new urgent appointment";
                string notificationContent = "You have new urgent appointment on  " + " " + startOfInterval;

                Notification notification = new Notification(DateTime.Now, notificationContent, notificationTitle, freedoctorId, false, false);
                _notificationRepo.Create(notification);

                return true;
            }
            else return false;
        }

        public List<Appointment> GetMovableAppointments(DateTime startOfInterval, DateTime endOfInterval, SpecType specialization)
        {
            List<Appointment> movableAppointments = new List<Appointment>();
            List<Doctor> doctors = (List<Doctor>)_doctorRepo.GetAllDoctorsBySpecialization(specialization);

            foreach (Doctor doctor in doctors)
            {
                List<Appointment> movableAppointmentsOfDoctor = _appointmentRepo.GetDoctorsMovableAppointments(startOfInterval,
                    endOfInterval, doctor.Id).ToList();

                movableAppointments.AddRange(movableAppointmentsOfDoctor);

            }

            return movableAppointments;
        } 

        public List<AppointmentView> SortMovableAppointments(List<Appointment> movableAppointments)
        {
            List<AppointmentView> appointmentViews = new List<AppointmentView>();
            Dictionary<int, DateTime> nearestFreeTermDictionary = new Dictionary<int, DateTime>();

            foreach (Appointment appointment in movableAppointments)
            {
                    DateTime nearestFreeTerm = GetNearestFreeTerm(appointment.Id);
                nearestFreeTermDictionary.Add(appointment.Id, nearestFreeTerm);
            }

            List<KeyValuePair<int, DateTime>> nearestFreeTermList = nearestFreeTermDictionary.ToList();
            nearestFreeTermList.Sort((x, y) => x.Value.CompareTo(y.Value));

            foreach (KeyValuePair<int, DateTime> nearestFreeTerm in nearestFreeTermList)
            {
                Appointment appointmentForMoving = _appointmentRepo.GetById(nearestFreeTerm.Key);
                appointmentViews.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(appointmentForMoving,
                    _userRepo.GetById(appointmentForMoving.DoctorId), _roomRepo.Get(appointmentForMoving.RoomId)));
            }

            return appointmentViews;
        }

        public DateTime GetNearestFreeTerm(int appId)
        {
            Appointment appointment = _appointmentRepo.GetById(appId);
            Doctor doctor = _doctorRepo.GetById(appointment.DoctorId);
            List<AppointmentView> MoveAppointmentOptions = GetAvailableAppointmentOptions("No Priority",
                 appointment.Beginning, appointment.Ending.AddDays(4), appointment.DoctorId, doctor.Specialization, appointment.PatientId, -1).ToList();
            
            // Vraca prvi moguci termin za pomeranje
            return MoveAppointmentOptions[0].Beginning;
        }

        public List<AppointmentView> GetSortedMovableAppointments(SpecType specialization, DateTime startOfInterval)
        {
            DateTime endOfInterval = startOfInterval.AddHours(1);
            List<AppointmentView> appointmentViews = new List<AppointmentView>();

            List<Appointment> possibleOptionsForMoving = GetMovableAppointments(startOfInterval, endOfInterval, specialization);

            appointmentViews = SortMovableAppointments(possibleOptionsForMoving);

            return appointmentViews;
        }
    }
}
