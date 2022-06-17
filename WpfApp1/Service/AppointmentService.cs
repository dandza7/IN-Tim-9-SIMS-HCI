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

        public IEnumerable<Appointment> GetAllBySpecialization(string spec)
        {
            List<Appointment> appointments = _appointmentRepo.GetAll().ToList();
            List<Appointment> appointmentsBySpecailization = new List<Appointment>();
            foreach (Appointment a in appointments)
            {
                if(_doctorRepo.GetById(a.DoctorId).Specialization.ToString() == spec)
                {
                    appointmentsBySpecailization.Add(a);
                }
            }
            return appointmentsBySpecailization;

        }

        public IEnumerable<Appointment> GetAllByMonth(int month)
        {
            List<Appointment> appointments = _appointmentRepo.GetAll().ToList();
            List<Appointment> appointmentsByMonth = new List<Appointment>();
            foreach (Appointment a in appointments)
            {
                if (a.Beginning.Month == month)
                {
                    appointmentsByMonth.Add(a);
                }
            }
            return appointmentsByMonth;

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
            UpdateCancellationCounter(patientId);
            return _appointmentRepo.Delete(appointmentId);
        }

        private void UpdateCancellationCounter(int patientId)
        {
            Patient patient = _patientRepo.GetById(patientId);
            DateTime lastCancellationDate = patient.LastCancellationDate;

            if (lastCancellationDate.AddMonths(1) < DateTime.Now)
            {
                patient.NumberOfCancellations = 1;
                patient.LastCancellationDate = DateTime.Now;
                _patientRepo.Update(patient);

            }
            else
            {
                patient.NumberOfCancellations += 1;
                patient.LastCancellationDate = DateTime.Now;
                _patientRepo.Update(patient);
            }
        }

        internal List<Appointment> GetAllByDoctorId(int id)
        {
            return _appointmentRepo.GetAllByDoctorId(id);
        }

        internal IEnumerable<Appointment> GetAllByPatientId(int patientId)
        {
            return _appointmentRepo.GetAllByPatientId(patientId);
        }

        private List<AppointmentView> GetAppointmentsForFreeTimeInterval(DateTime startOfInterval, DateTime endOfInterval,
            List<AppointmentView> appointments, Room room, Doctor doctor, User doctorUser, int patientId)
        {
            TimeMenager interval = new TimeMenager(startOfInterval, endOfInterval);
            TimeMenagerService timeMenagerService = new TimeMenagerService(interval);

            while (timeMenagerService.GetIncrementedBeginning() <= interval.Ending)
            {
                if (timeMenagerService.AreAvailableAppointmentsCollected(appointments)) return appointments;

                bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, interval.Beginning, timeMenagerService.GetIncrementedBeginning());
                if (isRoomAvailable)
                {
                    Appointment freeAppointment = new Appointment(interval.Beginning, timeMenagerService.GetIncrementedBeginning(), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                    appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                }
                timeMenagerService.IncrementBeginning();
            }
            return appointments;
        }

        private List<AppointmentView> GetAppointmentsHappyCase(DateTime startOfInterval, DateTime endOfInterval,
            List<Appointment> appointmentsOfDoctor, List<AppointmentView> appointments, Room room, Doctor doctor, User doctorUser, int patientId)
        {
            TimeMenager interval = new TimeMenager(startOfInterval, endOfInterval);
            TimeMenagerService timeMenagerService = new TimeMenagerService(interval);

            if (timeMenagerService.AreAvailableAppointmentsCollected(appointments)) return appointments;

            foreach (Appointment appointment in appointmentsOfDoctor)
            {
                while (timeMenagerService.GetIncrementedBeginning() <= appointment.Beginning)
                {
                    if (timeMenagerService.AreAvailableAppointmentsCollected(appointments)) return appointments;
                    bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, interval.Beginning, timeMenagerService.GetIncrementedBeginning());
                    if (isRoomAvailable)
                    {
                        Appointment freeAppointment = new Appointment(interval.Beginning, timeMenagerService.GetIncrementedBeginning(), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                        appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                    }
                    timeMenagerService.IncrementBeginning();
                }
                interval.Beginning = appointment.Ending;
            }

            while (timeMenagerService.GetIncrementedBeginning() <= interval.Ending)
            {
                if (timeMenagerService.AreAvailableAppointmentsCollected(appointments)) return appointments;
                bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, interval.Beginning, timeMenagerService.GetIncrementedBeginning());
                if (isRoomAvailable)
                {
                    Appointment freeAppointment = new Appointment(interval.Beginning, timeMenagerService.GetIncrementedBeginning(), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                    appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                }
                timeMenagerService.IncrementBeginning();
            }
            return appointments;
        }

        private (DateTime, DateTime) AdjustSearchingTimeInterval(DateTime startOfInterval, DateTime endOfInterval, int oldAppointmentId)
        {
            TimeMenager interval = new TimeMenager(startOfInterval, endOfInterval);
            TimeMenagerService timeMenagerService = new TimeMenagerService(interval);

            timeMenagerService.TrimExcessiveTime();

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
            TimeMenagerService timeMenagerService = new TimeMenagerService(interval);

            foreach (Doctor doctor in doctorsOfThatSpec)
            {
                if (timeMenagerService.AreAvailableAppointmentsCollected(appointments)) return appointments;

                List<Appointment> doctorsAppointments = _appointmentRepo.GetAllAppointmentsInTimeIntervalForDoctor(interval.Beginning, timeMenagerService.GetIncrementedBeginning(), doctor.Id).ToList();
                if (doctorsAppointments.Count == 0)
                {
                    if (timeMenagerService.AreAvailableAppointmentsCollected(appointments)) return appointments;

                    User doctorUser = _userRepo.GetById(doctor.Id);
                    Room doctorRoom = _roomRepo.GetById(doctor.RoomId);
                    bool isRoomAvailable = _renovationRepo.IsRoomAvailable(doctorRoom.Id, interval.Beginning, timeMenagerService.GetIncrementedBeginning());

                    if (isRoomAvailable)
                    {
                        Appointment freeAppointment = new Appointment(interval.Beginning, timeMenagerService.GetIncrementedBeginning(), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                        appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, doctorRoom));
                    }
                    timeMenagerService.IncrementBeginning();
                }
            }
            return appointments;
        }

        public List<AppointmentView> GetAvailableAppointmentOptions(string priority,
            DateTime startOfInterval, DateTime endOfInterval, int doctorId, SpecType specialization, int patientId, int oldAppointmentId)
        {

            (startOfInterval, endOfInterval) = AdjustSearchingTimeInterval(startOfInterval, endOfInterval, oldAppointmentId);
            TimeMenager interval = new TimeMenager(startOfInterval, endOfInterval);
            TimeMenagerService timeMenagerService = new TimeMenagerService(interval);

            List<AppointmentView> appointments = new List<AppointmentView>();
            List<Appointment> appointmentsForDoctor = _appointmentRepo.GetAllAppointmentsInTimeIntervalForDoctor(interval.Beginning,
                interval.Ending,
                doctorId).ToList();

            Doctor doctor = _doctorRepo.GetById(doctorId);
            Room room = _roomRepo.GetById(doctor.RoomId);
            User doctorUser = _userRepo.GetById(doctorId);

            if (appointmentsForDoctor.Count == 0 && timeMenagerService.GetIncrementedBeginning() <= interval.Ending)
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
            TimeMenagerService timeMenagerService = new TimeMenagerService(interval);

            timeMenagerService.MoveStartOfIntervalToTheNextDay();
                
            foreach(Appointment appointment in doctorsAppointments)
            {
                while(timeMenagerService.GetIncrementedBeginning() <= appointment.Beginning)
                {
                    if (timeMenagerService.AreAvailableAppointmentsCollected(appointments)) return appointments;
                    bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, interval.Beginning, timeMenagerService.GetIncrementedBeginning());

                    if (isRoomAvailable)
                    {
                        Appointment freeAppointment = new Appointment(interval.Beginning, timeMenagerService.GetIncrementedBeginning(), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                        appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                    }
                    timeMenagerService.IncrementBeginning();
                }
                if (appointment.Beginning > interval.Beginning) interval.Beginning = appointment.Ending;
            }
            while (appointments.Count < 5)
            {
                timeMenagerService.MoveStartOfIntervalIfNeeded();
                bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, interval.Beginning, timeMenagerService.GetIncrementedBeginning());
                if (isRoomAvailable)
                {
                    Appointment freeAppointment2 = new Appointment(interval.Beginning, timeMenagerService.GetIncrementedBeginning(), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                    appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment2, doctorUser, room));
                }
                timeMenagerService.IncrementBeginning();
            }
            return appointments;
        }

        private List<AppointmentView> GetAppointmentsWithPriorityOfTime(SpecType specialization, DateTime startOfInterval, DateTime endOfInterval, int doctorId, int patientId, int oldAppointmentId)
        {
            List<AppointmentView> appointments = new List<AppointmentView>();
            List<Doctor> doctorsBySpec = _doctorRepo.GetAllDoctorsBySpecialization(specialization).ToList();
            List<Appointment> appointmentsInInterval = _appointmentRepo.GetAllAppointmentsInTimeInterval(startOfInterval, endOfInterval).ToList();

            TimeMenager interval = new TimeMenager(startOfInterval, endOfInterval);
            TimeMenagerService timeMenagerService = new TimeMenagerService(interval);

            if (timeMenagerService.AreAvailableAppointmentsCollected(appointments)) return appointments;

            appointments = GetAppointmentFromDoctorOfSpec(appointments, interval.Beginning, timeMenagerService.GetIncrementedBeginning(), doctorsBySpec, patientId);

            if (appointments.Count == 0)
            {
                foreach(Appointment appointmentInInterval in appointmentsInInterval)
                {
                    if (timeMenagerService.AreAvailableAppointmentsCollected(appointments)) return appointments;

                    interval.Beginning = appointmentInInterval.Ending;
                    if (timeMenagerService.GetIncrementedBeginning() > interval.Ending) return appointments;

                    appointments = GetAppointmentFromDoctorOfSpec(appointments, interval.Beginning, timeMenagerService.GetIncrementedBeginning(), doctorsBySpec, patientId);
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
                    Room room = _roomRepo.GetById(doctor.RoomId);
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
                    Room room = _roomRepo.GetById(doctor.RoomId);
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
                    Room room = _roomRepo.GetById(doctor.RoomId);
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


         public List<SecretaryAppointmentView> GetSecretaryAppointmentViewsInTimeInterval(DateTime startOfInterval, DateTime endOfInterval)
        {
            List<SecretaryAppointmentView> appointmentViews = new List<SecretaryAppointmentView>();
            List<Appointment> appointments = _appointmentRepo.GetAll().ToList();
            foreach (Appointment appointment in appointments)
            {
                if (appointment.Beginning >= startOfInterval & appointment.Ending <= endOfInterval)
                {
                    User doctor = _userRepo.GetById(_doctorRepo.GetById(appointment.DoctorId).Id);
                    User patient = _userRepo.GetById(_patientRepo.GetById(appointment.PatientId).Id);
                    appointmentViews.Add(AppointmentConverter.ConvertSecretaryAppointmentSecretaryAppointmentView(appointment, doctor, patient));
                }
            }
            return appointmentViews;
        }
        public int FindFreeDoctor(SpecType spec, DateTime startOfInterval, DateTime endOfInterval)
        {
            List<Doctor> doctors = (List<Doctor>)_doctorRepo.GetAllDoctorsBySpecialization(spec);
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

            return freedoctorId;    
        }




        public bool CreateUrgentAppointment(int patientId, SpecType spec, DateTime startOfInterval)
        {

            DateTime endOfInterval = startOfInterval.AddHours(1);

            int freeDoctorId = FindFreeDoctor(spec, startOfInterval, endOfInterval);

            if (freeDoctorId != -1)
            {
                Appointment a = new Appointment(startOfInterval, endOfInterval, Appointment.AppointmentType.regular, true,
                    freeDoctorId, patientId, _doctorRepo.GetById(freeDoctorId).RoomId);
                _appointmentRepo.Create(a);

                string notificationTitle = "You have new urgent appointment";
                string notificationContent = "You have new urgent appointment on  " + " " + startOfInterval;

                Notification notification = new Notification(DateTime.Now, notificationContent, notificationTitle, freeDoctorId, false, false);
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
            Dictionary<int, DateTime> nearestFreeTerms = new Dictionary<int, DateTime>();

            foreach (Appointment appointment in movableAppointments)
            {
                    DateTime nearestFreeTerm = GetNearestFreeTerm(appointment.Id);
                nearestFreeTerms.Add(appointment.Id, nearestFreeTerm);
            }

            List<KeyValuePair<int, DateTime>> nearestFreeTermList = nearestFreeTerms.ToList();
            nearestFreeTermList.Sort((x, y) => x.Value.CompareTo(y.Value));

            foreach (KeyValuePair<int, DateTime> nearestFreeTerm in nearestFreeTermList)
            {
                Appointment appointmentForMoving = _appointmentRepo.GetById(nearestFreeTerm.Key);
                appointmentViews.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(appointmentForMoving,
                    _userRepo.GetById(appointmentForMoving.DoctorId), _roomRepo.GetById(appointmentForMoving.RoomId)));
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
        public void CancelAppointments(int roomId)
        {
            List<Appointment> appointments = this._appointmentRepo.GetAll().ToList();
            foreach (Appointment appointment in appointments)
            {
                if (appointment.RoomId == roomId)
                    _appointmentRepo.Delete(appointment.Id);
            }

        }
    }
}
