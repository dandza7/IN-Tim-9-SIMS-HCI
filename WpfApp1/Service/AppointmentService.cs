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
    public class AppointmentService
    {
        private readonly AppointmentRepository _appointmentRepo;
        private readonly DoctorRepository _doctorRepo;
        private readonly PatientRepository _patientRepo;
        private readonly RoomRepository _roomRepo;
        private readonly UserRepository _userRepo;
        private readonly RenovationRepository _renovationRepo;

        public AppointmentService(AppointmentRepository appointmentRepo, 
            DoctorRepository doctorRepository, 
            PatientRepository patientRepo,
            RoomRepository roomRepo,
            UserRepository userRepo,
            RenovationRepository renovationRepo)
        {
            _appointmentRepo = appointmentRepo;
            _doctorRepo = doctorRepository;
            _patientRepo = patientRepo;
            _roomRepo = roomRepo;
            _userRepo = userRepo;
            _renovationRepo = renovationRepo;
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

        public bool Delete(int id)
        {
            return _appointmentRepo.Delete(id);
        }

        public List<AppointmentView> GetAvailableAppointmentOptions(string priority, 
            DateTime startOfInterval, DateTime endOfInterval, int doctorId, int patientId, int oldAppointmentId)
        {
            if (priority.Equals("Doctor")) return GetAppointmentsWithPriorityOfDoctor(startOfInterval, endOfInterval, doctorId, patientId, oldAppointmentId);
            return GetAppointmentsWithPriorityOfTime(startOfInterval, endOfInterval, doctorId, patientId, oldAppointmentId);
        }

        private DateTime MoveStartOfIntervalToTheNextDay(DateTime startOfInterval)
        {
            int year = startOfInterval.Year;
            int month = startOfInterval.Month;
            int day = startOfInterval.Day;
            DateTime start = new DateTime(year, month, day, 20, 0, 0);
            startOfInterval = start.AddHours(11);

            return startOfInterval;
        }

        private List<AppointmentView> GetAppointmentsWithPriorityOfDoctor(DateTime startOfInterval, DateTime endOfInterval, int doctorId, int patientId, int oldAppointmentId)
        {
            int yearOfStart = startOfInterval.Year;
            int monthOfStart = startOfInterval.Month;
            int dayOfStart = startOfInterval.Day;
            DateTime startOfWorkingHours = new DateTime(yearOfStart, monthOfStart, dayOfStart, 7, 0, 0);
            // Kako radno vrijeme počinje u 7 ujutru ukoliko je selektovano ranije treba da se pomjeri
            if (startOfInterval.Hour < 7)
            {
                startOfInterval = startOfWorkingHours;
            }

            int yearOfEnd = endOfInterval.Year;
            int monthOfEnd = endOfInterval.Month;
            int dayOfEnd = endOfInterval.Day;
            DateTime endOfWorkingHours = new DateTime(yearOfEnd, monthOfEnd, dayOfEnd, 20, 0, 0);
            // >= je zbog toga što 20.45 počinje u 20 ali je nakon kraja radnog vremena
            if (endOfInterval.Hour >= 20)
            {
                endOfInterval = endOfWorkingHours;
            }
            // Ako je manje od 8 to znači da je između ponoći i 8 ujutru pa treba da se vrati na 20 sati, prethodnog dana
            if(endOfInterval.Hour < 8)
            {
                endOfInterval = endOfWorkingHours.AddDays(-1);
            }

            if (oldAppointmentId != -1)
            {
                Appointment oldAppointment = _appointmentRepo.GetById(oldAppointmentId);
                if (oldAppointment.Ending.AddDays(4) < endOfInterval) endOfInterval = oldAppointment.Ending.AddDays(4);
                if (oldAppointment.Beginning.AddDays(-4) > startOfInterval) startOfInterval = oldAppointment.Beginning.AddDays(-4); 
            }
            List<AppointmentView> appointments = new List<AppointmentView>();
            List<Appointment> appointmentsForDoctor = _appointmentRepo.GetAllAppointmentsInTimeIntervalForDoctor(startOfInterval, 
                endOfInterval, 
                doctorId).ToList();

            //Ovo je potrebno za converter iz appointmenta u appointment view
            Doctor doctor = _doctorRepo.GetById(doctorId);
            Room room = _roomRepo.Get(doctor.RoomId);
            User doctorUser = _userRepo.GetById(doctorId);

            // Happy Case
            if (appointmentsForDoctor.Count == 0)
            {
                // Provjeravam da li ću dodavanjem novog termina probiti željeni interval
                while(startOfInterval.AddHours(1) <= endOfInterval)
                {
                    // Ako bi se termin završio nakon 8 uveče onda ne može da se rezerviše
                    if(startOfInterval.AddHours(1).Hour >= 20)
                    {
                        // Pomjera se na 7 ujutru narednog dana
                        startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                        // Ako pomjeranje izaziva probijanje intervala onda ne može da se nađe termin
                        Console.WriteLine("Pomjereno radno vrijeme na " + startOfInterval);
                        if (startOfInterval.AddHours(1) > endOfInterval) break;
                    } 
                    if (appointments.Count == 10) break;
                    // Provjerava da li se soba renovira
                    bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, startOfInterval, startOfInterval.AddHours(1));
                    if (isRoomAvailable)
                    {
                        Appointment freeAppointment = new Appointment(startOfInterval, startOfInterval.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                        appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                    }
                    startOfInterval = startOfInterval.AddHours(1);
                }
                return appointments;
            }

            //U Slučaju da u traženom vremenskom intervalu već postoje termini željenog doktora
            while (startOfInterval.AddHours(1) <= endOfInterval)
            {
                if (startOfInterval.AddHours(1).Hour >= 20)
                {
                    startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                    if (startOfInterval.AddHours(1) > endOfInterval) break;
                }
                foreach (Appointment appointment in appointmentsForDoctor)
                {
                        // Ako je pocetak appointmenta nakon startOfInterval + 1 sat (trajanje termina je jedan sat)
                        // onda imamo slobodan termin za novi appointment
                        while (startOfInterval.AddHours(1) <= appointment.Beginning)
                        {
                            if (startOfInterval.AddHours(1).Hour >= 20)
                            {
                                startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                                if (startOfInterval.AddHours(1) > endOfInterval) break;
                            }
                            if (appointments.Count == 10) break;
                            bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, startOfInterval, startOfInterval.AddHours(1));
                            if (isRoomAvailable)
                            {
                                Appointment freeAppointment = new Appointment(startOfInterval, startOfInterval.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                                appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                            }
                            startOfInterval = startOfInterval.AddHours(1);
                        }
                        // Čim smo promašili ili ispali iz while-a došli smo do zauzetog termina željenog doktora
                        // te dalje možemo krenuti tražiti tek od kraja tog termina
                        startOfInterval = appointment.Ending;
                    
                }
                // Prošli smo sve termine željenog doktora koji su zakazani u željenom intervalu,
                // sada smo opet upali u happy case te pokušavamo da nađemo još slobodnih termina u željenom intervalu
                while (startOfInterval.AddHours(1) <= endOfInterval)
                {
                    if (startOfInterval.AddHours(1).Hour >= 20)
                    {
                        startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                        if (startOfInterval.AddHours(1) > endOfInterval) break;
                    }
                    if (appointments.Count == 10) break;
                    bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, startOfInterval, startOfInterval.AddHours(1));
                    if (isRoomAvailable)
                    {
                        Appointment freeAppointment = new Appointment(startOfInterval, startOfInterval.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                        appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                    }
                    startOfInterval = startOfInterval.AddHours(1);
                }
            }
            
            // U slučaju da nismo pronašli nijedan slobodan termin u željenom vremenskom intervalu kod željenog doktora
            // onda treba da reaguje prioritet i ponudi pacijentu prvih 5 slobodnih termina kod njega
            if(appointments.Count == 0)
            {
                List<Appointment> doctorsAppointments = _appointmentRepo.GetAllAppointmentsForDoctor(doctorId).ToList();
                DateTime startTime = endOfInterval.AddDays(1);
                while (appointments.Count < 5)
                {
                    foreach(Appointment appointment in doctorsAppointments)
                    {
                        // Dok god dodavanjem novog appointmenta ne upadamo u appointment
                        // traženog doktora možemo dodavati termine
                        while(startTime.AddHours(1) <= appointment.Beginning)
                        {
                            if (startTime.AddHours(1).Hour >= 20)
                            {
                                startTime = MoveStartOfIntervalToTheNextDay(startTime);
                            }
                            if (appointments.Count == 5) break;
                            bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, startTime, startTime.AddHours(1));
                            if (isRoomAvailable)
                            {
                                Appointment freeAppointment = new Appointment(startTime, startTime.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                                appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                            }
                            startTime = startTime.AddHours(1);
                        }
                        startTime = appointment.Ending;
                    }
                    // Ako smo prošli kroz sve appointmente koje je doktor imao i još nismo našli 5 termina onda
                    // možemo da ih dodajemo pod uslovom da je soba u kojoj se termin održava slobodna
                    // i da tada bolnica radi
                    while (appointments.Count < 5)
                    {
                        if (startTime.AddHours(1).Hour >= 20)
                        {
                            startTime = MoveStartOfIntervalToTheNextDay(startTime);
                        }
                        bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, startTime, startTime.AddHours(1));
                        if (isRoomAvailable)
                        {
                            Appointment freeAppointment2 = new Appointment(startTime, startTime.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                            appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment2, doctorUser, room));
                        }
                        startTime = startTime.AddHours(1);
                    }
                }
            }
            return appointments;
        }

        private List<AppointmentView> GetAppointmentsWithPriorityOfTime(DateTime startOfInterval, DateTime endOfInterval, int doctorId, int patientId, int oldAppointmentId)
        {
            int yearOfStart = startOfInterval.Year;
            int monthOfStart = startOfInterval.Month;
            int dayOfStart = startOfInterval.Day;
            DateTime startOfWorkingHours = new DateTime(yearOfStart, monthOfStart, dayOfStart, 7, 0, 0);
            // Kako radno vrijeme počinje u 7 ujutru ukoliko je selektovano ranije treba da se pomjeri
            if (startOfInterval.Hour < 7)
            {
                startOfInterval = startOfWorkingHours;
            }

            int yearOfEnd = endOfInterval.Year;
            int monthOfEnd = endOfInterval.Month;
            int dayOfEnd = endOfInterval.Day;
            DateTime endOfWorkingHours = new DateTime(yearOfEnd, monthOfEnd, dayOfEnd, 20, 0, 0);
            // >= je zbog toga što 20.45 počinje u 20 ali je nakon kraja radnog vremena
            if (endOfInterval.Hour >= 20)
            {
                endOfInterval = endOfWorkingHours;
            }
            // Ako je manje od 8 to znači da je između ponoći i 8 ujutru pa treba da se vrati na 20 sati, prethodnog dana
            if (endOfInterval.Hour < 8)
            {
                endOfInterval = endOfWorkingHours.AddDays(-1);
            }

            // U slučaju da je u pitanu pomjeranje treba listati samo opcije koje su u razmaku od 4 dana od starog termina
            if (oldAppointmentId != -1)
            {
                Appointment oldAppointment = _appointmentRepo.GetById(oldAppointmentId);
                if (oldAppointment.Ending.AddDays(4) < endOfInterval) endOfInterval = oldAppointment.Ending.AddDays(4);
                if (oldAppointment.Beginning.AddDays(-4) > startOfInterval) startOfInterval = oldAppointment.Beginning.AddDays(-4);
            }

            List<AppointmentView> appointments = new List<AppointmentView>();
            List<Appointment> appointmentsOfDoctor = _appointmentRepo.GetAllAppointmentsInTimeIntervalForDoctor(startOfInterval, endOfInterval, doctorId).ToList();
            
            //Ovo je potrebno za converter iz appointmenta u appointment view
            Doctor doctor = _doctorRepo.GetById(doctorId);
            Room room = _roomRepo.Get(doctor.RoomId);
            User doctorUser = _userRepo.GetById(doctorId);
            // Ovu promjenljivu čuvam u slučaju da prioritet treba da reaguje pa da znam koji je bio originalni početak intervala
            DateTime originalStartOfInterval = startOfInterval;

            //Happy Case
            if(appointmentsOfDoctor.Count == 0)
            {
                // Provjeravam da li ću dodavanjem novog termina probiti zadani interval
                while (startOfInterval.AddHours(1) <= endOfInterval)
                {
                    if (startOfInterval.AddHours(1).Hour >= 20)
                    {
                        startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                        if (startOfInterval.AddHours(1) > endOfInterval) break;
                    }
                    if (appointments.Count == 10) break;
                    bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, startOfInterval, startOfInterval.AddHours(1));
                    if (isRoomAvailable)
                    {
                        Appointment freeAppointment = new Appointment(startOfInterval, startOfInterval.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                        appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                    }
                    startOfInterval = startOfInterval.AddHours(1);
                }
                return appointments;
            }

            while (startOfInterval.AddHours(1) <= endOfInterval)
            {
                if (startOfInterval.AddHours(1).Hour >= 20)
                {
                    startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                    if (startOfInterval.AddHours(1) > endOfInterval) break;
                }
                foreach (Appointment appointment in appointmentsOfDoctor)
                {
                    // Ako je pocetak appointmenta nakon startOfInterval + 1 sat (trajanje termina je jedan sat)
                    // onda imamo slobodan termin za novi appointment
                    while (startOfInterval.AddHours(1) <= appointment.Beginning)
                    {
                        if (startOfInterval.AddHours(1).Hour >= 20)
                        {
                            startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                            if (startOfInterval.AddHours(1) > endOfInterval) break;
                        }
                        if (appointments.Count == 10) break;
                        bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, startOfInterval, startOfInterval.AddHours(1));
                        if (isRoomAvailable)
                        {
                            Appointment freeAppointment = new Appointment(startOfInterval, startOfInterval.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                            appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                        }
                        startOfInterval = startOfInterval.AddHours(1);
                    }
                    // Čim smo promašili ili ispali iz while-a došli smo do zauzetog termina željenog doktora
                    // te dalje možemo krenuti tražiti tek od kraja tog termina
                    startOfInterval = appointment.Ending;

                }
                // Prošli smo sve termine željenog doktora koji su zakazani u željenom intervalu,
                // sada smo opet upali u happy case te pokušavamo da nađemo još slobodnih termina u željenom intervalu
                while (startOfInterval.AddHours(1) <= endOfInterval)
                {
                    if (startOfInterval.AddHours(1).Hour >= 20)
                    {
                        startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                        if (startOfInterval.AddHours(1) > endOfInterval) break;
                    }
                    if (appointments.Count == 10) break;
                    bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, startOfInterval, startOfInterval.AddHours(1));
                    if (isRoomAvailable)
                    {
                        Appointment freeAppointment = new Appointment(startOfInterval, startOfInterval.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                        appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                    }
                    startOfInterval = startOfInterval.AddHours(1);
                }
            }

            // Ukoliko nije pronađen nijedan slobodan termin kod doktora u željenom vremenskom intervalu
            // treba da reaguje prioritet i ponudi termine u željenom vremenskom intervalu kod bilo kod doktora
            if (appointments.Count == 0)
            {
                startOfInterval = originalStartOfInterval;
                List<Doctor> generalPracticioners = _doctorRepo.GetAllGeneralPracticioners().ToList();
                List<Appointment> appointmentsInInterval = _appointmentRepo.GetAllAppointmentsInTimeInterval(startOfInterval, endOfInterval).ToList();

                while(startOfInterval.AddHours(1) <= endOfInterval)
                {
                    if (startOfInterval.AddHours(1).Hour >= 20)
                    {
                        startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                        if (startOfInterval.AddHours(1) > endOfInterval) break;
                    }
                    foreach (Doctor generalPracticioner in generalPracticioners)
                    {
                        if (startOfInterval.AddHours(1).Hour >= 20)
                        {
                            startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                            if (startOfInterval.AddHours(1) > endOfInterval) break;
                        }
                        if (startOfInterval.AddHours(1) > endOfInterval) break;
                        if (appointments.Count == 10) break;

                        List<Appointment> generalPracticionersAppointments = _appointmentRepo.GetAllAppointmentsInTimeIntervalForDoctor(startOfInterval, startOfInterval.AddHours(1), generalPracticioner.Id).ToList();
                        // Prioritetni happy case, postoji slobodan termin već na početku traženog intervala
                        if(generalPracticionersAppointments.Count == 0)
                        {
                            User generalPracticionerUser = _userRepo.GetById(generalPracticioner.Id);
                            Room generalPracitionerRoom = _roomRepo.Get(generalPracticioner.RoomId);
                            bool isRoomAvailable = _renovationRepo.IsRoomAvailable(generalPracitionerRoom.Id, startOfInterval, startOfInterval.AddHours(1));
                            if (isRoomAvailable)
                            {
                                Appointment freeAppointment = new Appointment(startOfInterval, startOfInterval.AddHours(1), Appointment.AppointmentType.regular, false, generalPracticioner.Id, patientId, generalPracticioner.RoomId);
                                appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, generalPracticionerUser, generalPracitionerRoom));
                            }
                            startOfInterval = startOfInterval.AddHours(1);
                        }
                    }
                    // Ako su svi zauzeti na početku traženog intervala
                    if(appointments.Count == 0)
                    {
                        foreach(Appointment appointmentInInterval in appointmentsInInterval)
                        {
                            if (startOfInterval.AddHours(1).Hour >= 20)
                            {
                                startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                                if (startOfInterval.AddHours(1) > endOfInterval) break;
                            }
                            // Ako bismo time što nađemo appointment probili vremenski interval onda ne možemo da ga nađemo
                            if (startOfInterval.AddHours(1) > endOfInterval) break;
                            if (startOfInterval >= appointmentInInterval.Ending) continue;
                            // Pomijeramo početni interval na kraj appointmenta
                            startOfInterval = appointmentInInterval.Ending;
                            foreach (Doctor generalPracticioner in generalPracticioners)
                            {
                                if (startOfInterval.AddHours(1).Hour >= 20)
                                {
                                    startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                                    if (startOfInterval.AddHours(1) > endOfInterval) break;
                                }
                                if (appointments.Count == 10) break;
                                if (startOfInterval.AddHours(1) > endOfInterval) break;

                                List<Appointment> generalPracticionersAppointments = _appointmentRepo.GetAllAppointmentsInTimeIntervalForDoctor(startOfInterval, startOfInterval.AddHours(1), generalPracticioner.Id).ToList();
                                // Prioritetni happy case, postoji slobodan termin na početku traženog intervala
                                if (generalPracticionersAppointments.Count == 0)
                                {
                                    if (startOfInterval.AddHours(1).Hour >= 20)
                                    {
                                        startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                                        if (startOfInterval.AddHours(1) > endOfInterval) break;
                                    }
                                    User generalPracticionerUser = _userRepo.GetById(generalPracticioner.Id);
                                    Room generalPracitionerRoom = _roomRepo.Get(generalPracticioner.RoomId);
                                    bool isRoomAvailable = _renovationRepo.IsRoomAvailable(generalPracitionerRoom.Id, startOfInterval, startOfInterval.AddHours(1));
                                    if (isRoomAvailable)
                                    {
                                        Appointment freeAppointment = new Appointment(startOfInterval, startOfInterval.AddHours(1), Appointment.AppointmentType.regular, false, generalPracticioner.Id, patientId, generalPracticioner.RoomId);
                                        appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, generalPracticionerUser, generalPracitionerRoom));
                                    }
                                    startOfInterval = startOfInterval.AddHours(1);
                                }
                            }
                        }
                    }
                    return appointments;
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
                // Odnosi se na termine koji su rezervisani pa tako termini u prošlosti nisu bitni
                if (appointment.PatientId == patientId && appointment.Beginning > DateTime.Now)
                {
                    Doctor doctor = _doctorRepo.GetById(appointment.DoctorId);
                    User user = _userRepo.GetById(doctor.Id);
                    Room room = _roomRepo.Get(doctor.RoomId);
                    appointmentViews.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(appointment, user, room));
                }
            }
            return appointmentViews;
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

    }
}
