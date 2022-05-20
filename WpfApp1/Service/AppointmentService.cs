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

        private DateTime CalculateWorkingHours(string type, DateTime interval)
        {
            int year = interval.Year;
            int month = interval.Month;
            int day = interval.Day;

            if (type.Equals("start")) return new DateTime(year, month, day, 7, 0, 0);

            return new DateTime(year, month, day, 20, 0, 0);
        }

        private List<AppointmentView> GetAppointmentsForFreeTimeInterval(DateTime startOfInterval, DateTime endOfInterval,
            List<AppointmentView> appointments, Room room, Doctor doctor, User doctorUser, int patientId)
        {
            TimeMenager interval = new TimeMenager(startOfInterval, endOfInterval);
            while (interval.GetIncrementedBeginning() <= interval.Ending)
            {
                if (interval.GetIncrementedBeginning().Hour >= 20)
                {
                    interval.MoveStartOfIntervalToTheNextDay();
                    if (interval.IncrementBeginning() > interval.Ending) return appointments;
                }
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
            if (interval.GetIncrementedBeginning().Hour >= 20)
            {
                interval.MoveStartOfIntervalToTheNextDay();
                if (interval.GetIncrementedBeginning() > interval.Ending) return appointments;
            }

            foreach (Appointment appointment in appointmentsOfDoctor)
            {
                while (interval.GetIncrementedBeginning() <= appointment.Beginning)
                {
                    if (interval.GetIncrementedBeginning().Hour >= 20)
                    {
                        interval.MoveStartOfIntervalToTheNextDay();
                        if (interval.GetIncrementedBeginning() > interval.Ending) return appointments;
                    }
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
                if (interval.GetIncrementedBeginning().Hour >= 20)
                {
                    interval.MoveStartOfIntervalToTheNextDay();
                    if (interval.GetIncrementedBeginning() > interval.Ending) return appointments;
                }
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
            DateTime startOfWorkingHours = CalculateWorkingHours("start", startOfInterval);
            if (startOfInterval.Hour < 7)
            {
                startOfInterval = startOfWorkingHours;
            }

            DateTime endOfWorkingHours = CalculateWorkingHours("end", endOfInterval);
            if (endOfInterval.Hour >= 20)
            {
                endOfInterval = endOfWorkingHours;
            }
            if (endOfInterval.Hour < 8)
            {
                endOfInterval = endOfWorkingHours.AddDays(-1);
            }

            if (oldAppointmentId != -1)
            {
                Appointment oldAppointment = _appointmentRepo.GetById(oldAppointmentId);
                if (oldAppointment.Ending.AddDays(4) < endOfInterval) endOfInterval = oldAppointment.Ending.AddDays(4);
                if (oldAppointment.Beginning.AddDays(-4) > startOfInterval) startOfInterval = oldAppointment.Beginning.AddDays(-4);
            }

            return (startOfInterval, endOfInterval);
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

        public Appointment GetById(int appointmentId)
        {
            return _appointmentRepo.GetById(appointmentId);
        }

        public List<AppointmentView> CreateUrgentAppointement(int patientId, SpecType spec, DateTime startOfInterval)
        {
            DateTime endOfInterval = startOfInterval.AddHours(1);

            List<Doctor> doctors = (List<Doctor>)_doctorRepo.GetAllDoctorsBySpecialization(spec);
            List<AppointmentView> appointmentViews = new List<AppointmentView>();
            List<AppointmentView> emptylist = new List<AppointmentView>();
            List<Appointment> appointmentsOfDoctors = new List<Appointment>();
            int freedoctorId = -1;

            foreach (Doctor d in doctors)
            {
                List<Appointment> appointmentsOfDoctor = _appointmentRepo.GetAllAppointmentsInTimeIntervalForDoctor(startOfInterval,
                    endOfInterval,d.Id).ToList();

                if (appointmentsOfDoctor.Count() == 0) { 
                    freedoctorId = d.Id;
                }

                appointmentsOfDoctors.AddRange(appointmentsOfDoctor);

            }

            if (freedoctorId != -1)
            {
                Appointment a = new Appointment(startOfInterval, endOfInterval, Appointment.AppointmentType.regular, true, 
                    freedoctorId, patientId, _doctorRepo.GetById(freedoctorId).RoomId);
                _appointmentRepo.Create(a);


                string title = "You have new urgent appointment";
                string content = "You have new urgent appointment on  " + " " + startOfInterval;

                Notification notification = new Notification(DateTime.Now, content, title, freedoctorId, false, false);
                _notificationRepo.Create(notification);
                return emptylist;
            }
            else
            {
                Dictionary<int, DateTime> sortedList = new Dictionary<int, DateTime>();
                for (int i = 0; i < appointmentsOfDoctors.Count(); i++)
                {
                    if (appointmentsOfDoctors[i].Type != Appointment.AppointmentType.surgery && appointmentsOfDoctors[i].IsUrgent != true)
                    {
                        DateTime nearestMoving = GetNearestMoving(appointmentsOfDoctors[i].Id);
                        sortedList.Add(i, nearestMoving);
                    }
                }

                List<KeyValuePair<int, DateTime>> myList = sortedList.ToList();
                myList.Sort((x, y) => x.Value.CompareTo(y.Value));

                foreach (KeyValuePair<int, DateTime> app in myList)
                {
                    appointmentViews.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(appointmentsOfDoctors[app.Key], 
                        _userRepo.GetById(appointmentsOfDoctors[app.Key].DoctorId), _roomRepo.Get(appointmentsOfDoctors[app.Key].RoomId)));
                }

                return appointmentViews;
            }
        }

        public DateTime GetNearestMoving(int appId)
        {
            Appointment appointment = _appointmentRepo.GetById(appId);
            Doctor doctor = _doctorRepo.GetById(appointment.DoctorId);
            List<AppointmentView> MoveAppointmentOptions = GetAvailableAppointmentOptions("No Priority",
                 appointment.Beginning, appointment.Ending.AddDays(4), appointment.DoctorId, doctor.Specialization, appointment.PatientId, -1).ToList();
            
            // Vraca prvi moguci termin za pomeranje
            Console.WriteLine(MoveAppointmentOptions[0].Beginning);
            return MoveAppointmentOptions[0].Beginning;
        }
    }
}
