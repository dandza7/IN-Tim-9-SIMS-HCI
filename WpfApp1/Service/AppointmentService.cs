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

        public List<AppointmentView> SecretaryGetAvailableAppointmentOptions(string priority,
            DateTime startOfInterval, DateTime endOfInterval, int doctorId, int patientId, int oldAppointmentId, SpecType spec)
        {
            if (priority.Equals("Doctor")) return SecretaryGetAppointmentsWithPriorityOfDoctor(startOfInterval, endOfInterval, doctorId, patientId, oldAppointmentId, spec);
            return SecretaryGetAppointmentsWithPriorityOfTime(startOfInterval, endOfInterval, doctorId, patientId, oldAppointmentId, spec);
        }



        /*----------------------------------------------------------------*/
        /* POMOĆNE FUNKCIJE ZA DOBIJANJE SLOBODNIH TERMINA SA PRIORITETOM */
        /*----------------------------------------------------------------*/

        // Funkcija koja pomjera interval na početak radnog vremena narednog dana
        private DateTime MoveStartOfIntervalToTheNextDay(DateTime startOfInterval)
        {
            int year = startOfInterval.Year;
            int month = startOfInterval.Month;
            int day = startOfInterval.Day;
            DateTime start = new DateTime(year, month, day, 20, 0, 0);
            startOfInterval = start.AddHours(11);

            return startOfInterval;
        }

        // Funkcija za dobijanje radnog vremena tog dana, radni dan počinje u 7 ujutru a završava se u 8 naveče
        private DateTime CalculateWorkingHours(string type, DateTime interval)
        {
            int year = interval.Year;
            int month = interval.Month;
            int day = interval.Day;

            if (type.Equals("start")) return new DateTime(year, month, day, 7, 0, 0);

            return new DateTime(year, month, day, 20, 0, 0);
        }

        // Funkcija koja vraća prvih 10 appointmenta za slučaj da je traženi doktor u traženom intervalu slobodan 
        private List<AppointmentView> GetAppointmentsForFreeTimeInterval(DateTime startOfInterval, DateTime endOfInterval,
            List<AppointmentView> appointments, Room room, Doctor doctor, User doctorUser, int patientId)
        {
            // Provjeravam da li ću dodavanjem novog termina probiti željeni interval
            while (startOfInterval.AddHours(1) <= endOfInterval)
            {
                // Ako bi se termin završio nakon 8 uveče onda ne može da se rezerviše
                if (startOfInterval.AddHours(1).Hour >= 20)
                {
                    // Pomjera se na 7 ujutru narednog dana
                    startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                    // Ako pomjeranje izaziva probijanje intervala onda ne može da se nađe termin
                    if (startOfInterval.AddHours(1) > endOfInterval) return appointments;
                }
                if (appointments.Count == 10) return appointments;
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
        private List<AppointmentView> SecretaryGetAppointmentsForFreeTimeInterval(DateTime startOfInterval, DateTime endOfInterval,
    List<AppointmentView> appointments, Room room, Doctor doctor, User doctorUser, int patientId)
        {
            // Provjeravam da li ću dodavanjem novog termina probiti željeni interval
            while (startOfInterval.AddHours(1) <= endOfInterval)
            {
                // Ako bi se termin završio nakon 8 uveče onda ne može da se rezerviše
                if (startOfInterval.AddHours(1).Hour >= 20)
                {
                    // Pomjera se na 7 ujutru narednog dana
                    startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                    // Ako pomjeranje izaziva probijanje intervala onda ne može da se nađe termin
                    if (startOfInterval.AddHours(1) > endOfInterval) return appointments;
                }
                if (appointments.Count == 10) return appointments;
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

        // funkcija koja vraća sve appointmente koji upadaju u Happy Case
        private List<AppointmentView> GetAppointmentsHappyCase(DateTime startOfInterval, DateTime endOfInterval,
            List<Appointment> appointmentsOfDoctor, List<AppointmentView> appointments, Room room, Doctor doctor, User doctorUser, int patientId)
        {
            while (startOfInterval.AddHours(1) <= endOfInterval)
            {
                if (startOfInterval.AddHours(1).Hour >= 20)
                {
                    startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                    if (startOfInterval.AddHours(1) > endOfInterval) return appointments;
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
                            if (startOfInterval.AddHours(1) > endOfInterval) return appointments;
                        }
                        if (appointments.Count == 10) return appointments;
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
                // sada smo opet upali u slobodan vremenski interval te pokušavamo da nađemo
                // još slobodnih termina u željenom intervalu
                while (startOfInterval.AddHours(1) <= endOfInterval)
                {
                    if (startOfInterval.AddHours(1).Hour >= 20)
                    {
                        startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                        if (startOfInterval.AddHours(1) > endOfInterval) return appointments;
                    }
                    if (appointments.Count == 10) return appointments;
                    bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, startOfInterval, startOfInterval.AddHours(1));
                    if (isRoomAvailable)
                    {
                        Appointment freeAppointment = new Appointment(startOfInterval, startOfInterval.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                        appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                    }
                    startOfInterval = startOfInterval.AddHours(1);
                }
            }
            return appointments;
        }

        private List<AppointmentView> SecretaryGetAppointmentsHappyCase(DateTime startOfInterval, DateTime endOfInterval,
        List<Appointment> appointmentsOfDoctor, List<AppointmentView> appointments, Room room, Doctor doctor, User doctorUser, int patientId)
        {
            Console.WriteLine("ASD");
            while (startOfInterval.AddHours(1) <= endOfInterval)
            {
                if (startOfInterval.AddHours(1).Hour >= 20)
                {
                    startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                    if (startOfInterval.AddHours(1) > endOfInterval) break;
                }
                Console.WriteLine("DSD");
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
                        if (appointments.Count == 10) return appointments ;
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
                    if (appointment.Beginning > startOfInterval) startOfInterval = appointment.Ending;

                }
                Console.WriteLine("gSD");
                // Prošli smo sve termine željenog doktora koji su zakazani u željenom intervalu,
                // sada smo opet upali u slobodan vremenski interval te pokušavamo da nađemo
                // još slobodnih termina u željenom intervalu
                while (startOfInterval.AddHours(1) <= endOfInterval)
                {
                    if (startOfInterval.AddHours(1).Hour >= 20)
                    {
                        startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                        if (startOfInterval.AddHours(1) > endOfInterval) break;
                    }
                    if (appointments.Count == 10) return appointments;
                    bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, startOfInterval, startOfInterval.AddHours(1));
                    if (isRoomAvailable)
                    {
                        Appointment freeAppointment = new Appointment(startOfInterval, startOfInterval.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                        appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                    }
                    startOfInterval = startOfInterval.AddHours(1);
                }
            }
            return appointments;
        }

        // Funkcija koja će ukoliko je vremenski interval van dozvoljenog vratiti u dozvoljene granice, npr. pri pomjeranju appointmenta 
        // kraj intervala za pretragu je više od 4 dana a to ne smije da se desi
        private (DateTime, DateTime) AdjustSearchingTimeInterval(DateTime startOfInterval, DateTime endOfInterval, int oldAppointmentId)
        {
            DateTime startOfWorkingHours = CalculateWorkingHours("start", startOfInterval);
            // Kako radno vrijeme počinje u 7 ujutru ukoliko je selektovano ranije treba da se pomjeri
            if (startOfInterval.Hour < 7)
            {
                startOfInterval = startOfWorkingHours;
            }

            DateTime endOfWorkingHours = CalculateWorkingHours("end", endOfInterval);
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

            // Ako je pomjeranje postojećeg appointmenta onda treba obezbijediti da se appointmenta ne može pomjeriti više od 4 dana
            if (oldAppointmentId != -1)
            {
                Appointment oldAppointment = _appointmentRepo.GetById(oldAppointmentId);
                if (oldAppointment.Ending.AddDays(4) < endOfInterval) endOfInterval = oldAppointment.Ending.AddDays(4);
                if (oldAppointment.Beginning.AddDays(-4) > startOfInterval) startOfInterval = oldAppointment.Beginning.AddDays(-4);
            }

            return (startOfInterval, endOfInterval);
        }
        
        // Pošto se ovo koristi dosta puta, iako je samo 3 linije, dosta je čitljivije ako je to zasebna funkcija
        // Samo pomjera vremenski interval na naredni dan ukoliko je radno vrijeme za taj dan gotovo
        private DateTime MoveStartOfIntervalIfNeeded(DateTime startOfInterval)
        {
            if (startOfInterval.AddHours(1).Hour >= 20)
            {
                startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
            }
            return startOfInterval;
        }

        // Dodaje novi termin kod bilo kod doktora opšte prakse od startOfInterval ukoliko je taj doktor tada slobodan
        private List<AppointmentView> GetAppointmentFromAnyPracticioner(List<AppointmentView> appointments, DateTime startOfInterval, DateTime endOfInterval, List<Doctor> generalPracticioners, int patientId)
        {
            foreach (Doctor generalPracticioner in generalPracticioners)
            {
                startOfInterval = MoveStartOfIntervalIfNeeded(startOfInterval);
                if (startOfInterval.AddHours(1) > endOfInterval) return appointments;
                if (appointments.Count == 10) return appointments;

                List<Appointment> generalPracticionersAppointments = _appointmentRepo.GetAllAppointmentsInTimeIntervalForDoctor(startOfInterval, startOfInterval.AddHours(1), generalPracticioner.Id).ToList();

                // Ukoliko doktor nema termin u narednih sat vremena počevši od startOfInterval onda se kod njega može dodati jedan termin
                if (generalPracticionersAppointments.Count == 0)
                {
                    startOfInterval = MoveStartOfIntervalIfNeeded(startOfInterval);
                    if (startOfInterval.AddHours(1) > endOfInterval) return appointments;

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
            return appointments;
        }

        /*----------------------------------------------------------------*/
        /*----------------------------------------------------------------*/
        /*----------------------------------------------------------------*/


        // Ukoliko je riječ o dodavanju novog appointmenta onda pri pozivu funkcije treba proslijediti -1 za oldAppointmentId 
        // dok se pri pomjeranju postojećeg appointmenta za oldAppointmentId prosleđuje Id appointmenta koji se pomjera
        public List<AppointmentView> GetAvailableAppointmentOptions(string priority,
            DateTime startOfInterval, DateTime endOfInterval, int doctorId, int patientId, int oldAppointmentId)
        {

            (startOfInterval, endOfInterval) = AdjustSearchingTimeInterval(startOfInterval, endOfInterval, oldAppointmentId);

            List<AppointmentView> appointments = new List<AppointmentView>();
            List<Appointment> appointmentsForDoctor = _appointmentRepo.GetAllAppointmentsInTimeIntervalForDoctor(startOfInterval,
                endOfInterval,
                doctorId).ToList();

            //Ovo je potrebno za converter iz appointmenta u appointment view
            Doctor doctor = _doctorRepo.GetById(doctorId);
            Room room = _roomRepo.Get(doctor.RoomId);
            User doctorUser = _userRepo.GetById(doctorId);

            // Interval je slobodan
            if (appointmentsForDoctor.Count == 0 && startOfInterval.AddHours(1) <= endOfInterval)
            {
                return GetAppointmentsForFreeTimeInterval(startOfInterval, endOfInterval, appointments, room, doctor, doctorUser, patientId);
            }

            // Happy Case
            appointments = GetAppointmentsHappyCase(startOfInterval, endOfInterval, appointmentsForDoctor, appointments, room, doctor, doctorUser, patientId);

            // Prioritet mora da reaguje
            if (appointments.Count == 0 && priority.Equals("Doctor"))
            {
                appointments = GetAppointmentsWithPriorityOfDoctor(startOfInterval, doctorId, patientId, oldAppointmentId, doctor, room, doctorUser);
            } else if (appointments.Count == 0 && priority.Equals("Time")) { 
                appointments = GetAppointmentsWithPriorityOfTime(startOfInterval, endOfInterval, doctorId, patientId, oldAppointmentId);
            }

            return appointments;
        }

        private List<AppointmentView> GetAppointmentsWithPriorityOfDoctor(DateTime endOfInterval, int doctorId, int patientId, int oldAppointmentId, Doctor doctor, Room room, User doctorUser)
        {
            List<AppointmentView> appointments = new List<AppointmentView>();
            List<Appointment> doctorsAppointments = _appointmentRepo.GetAllAppointmentsForDoctor(doctorId).ToList();
            DateTime startTime = MoveStartOfIntervalToTheNextDay(endOfInterval);
                
            foreach(Appointment appointment in doctorsAppointments)
            {
                // Dok god dodavanjem novog appointmenta ne upadamo u appointment
                // traženog doktora možemo dodavati termine
                while(startTime.AddHours(1) <= appointment.Beginning)
                {
                    startTime = MoveStartOfIntervalIfNeeded(startTime);
                    if (appointments.Count == 5) return appointments;
                    bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, startTime, startTime.AddHours(1));

                    if (isRoomAvailable)
                    {
                        Appointment freeAppointment = new Appointment(startTime, startTime.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                        appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                    }
                    startTime = startTime.AddHours(1);
                }
                // Pomjeri se na kraj zauzetog termina ukoliko je on nakon početka intervala koji pretražujemo
                if (appointment.Beginning > startTime) startTime = appointment.Ending;
            }
            // Ako smo prošli kroz sve appointmente koje je doktor imao i još nismo našli 5 termina onda
            // možemo da ih dodajemo pod uslovom da je soba u kojoj se termin održava slobodna
            while (appointments.Count < 5)
            {
                startTime = MoveStartOfIntervalIfNeeded(startTime);
                bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, startTime, startTime.AddHours(1));
                if (isRoomAvailable)
                {
                    Appointment freeAppointment2 = new Appointment(startTime, startTime.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                    appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment2, doctorUser, room));
                }
                startTime = startTime.AddHours(1);
            }
            return appointments;
        }

        private List<AppointmentView> GetAppointmentsWithPriorityOfTime(DateTime startOfInterval, DateTime endOfInterval, int doctorId, int patientId, int oldAppointmentId)
        {
            List<AppointmentView> appointments = new List<AppointmentView>();
            List<Doctor> generalPracticioners = _doctorRepo.GetAllGeneralPracticioners().ToList();
            List<Appointment> appointmentsInInterval = _appointmentRepo.GetAllAppointmentsInTimeInterval(startOfInterval, endOfInterval).ToList();

            startOfInterval = MoveStartOfIntervalIfNeeded(startOfInterval);
            if (startOfInterval.AddHours(1) > endOfInterval) return appointments;

            appointments = GetAppointmentFromAnyPracticioner(appointments, startOfInterval, endOfInterval, generalPracticioners, patientId);

            // Ako su svi zauzeti na početku traženog intervala
            if (appointments.Count == 0)
            {
                foreach(Appointment appointmentInInterval in appointmentsInInterval)
                {
                    startOfInterval = MoveStartOfIntervalIfNeeded(startOfInterval);
                    if (startOfInterval.AddHours(1) > endOfInterval) return appointments;
                    // Pomijeramo početni interval na kraj appointmenta
                    startOfInterval = appointmentInInterval.Ending;
                    appointments = GetAppointmentFromAnyPracticioner(appointments, startOfInterval, endOfInterval, generalPracticioners, patientId);
                }
            }
            return appointments;
        }

        private List<AppointmentView> SecretaryGetAppointmentsWithPriorityOfDoctor(DateTime startOfInterval, DateTime endOfInterval, int doctorId, int patientId, int oldAppointmentId, SpecType spec)
        {
            DateTime startOfWorkingHours = CalculateWorkingHours("start", startOfInterval);
            // Kako radno vrijeme počinje u 7 ujutru ukoliko je selektovano ranije treba da se pomjeri
            if (startOfInterval.Hour < 7)
            {
                startOfInterval = startOfWorkingHours;
            }

            DateTime endOfWorkingHours = CalculateWorkingHours("end", endOfInterval);
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

            // Ako je pomjeranje postojećeg appointmenta onda treba obezbijediti da se appointmenta ne može pomjeriti više od 4 dana
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

            //Potrebno ukoliko treba da reaguje da bi se znao originalni početak intervala
            DateTime originalEndOfInterval = endOfInterval;
            //Ovo je potrebno za converter iz appointmenta u appointment view
            Doctor doctor = _doctorRepo.GetById(doctorId);
            Room room = _roomRepo.Get(doctor.RoomId);
            User doctorUser = _userRepo.GetById(doctorId);

            // Interval je slobodan
            if (appointmentsForDoctor.Count == 0 && startOfInterval.AddHours(1) <= endOfInterval)
            {
                appointments = SecretaryGetAppointmentsForFreeTimeInterval(startOfInterval, endOfInterval, appointments, room, doctor, doctorUser, patientId);
                return appointments;
            }
            // Happy Case
            appointments = SecretaryGetAppointmentsHappyCase(startOfInterval, endOfInterval, appointmentsForDoctor, appointments, room, doctor, doctorUser, patientId);
            // U slučaju da nismo pronašli nijedan slobodan termin u željenom vremenskom intervalu kod željenog doktora
            // onda treba da reaguje prioritet i ponudi pacijentu prvih 5 slobodnih termina kod njega
            if (appointments.Count == 0)
            {
                List<Appointment> doctorsAppointments = _appointmentRepo.GetAllAppointmentsForDoctor(doctorId).ToList();
                DateTime startTime = MoveStartOfIntervalToTheNextDay(originalEndOfInterval);
                while (appointments.Count < 5)
                {
                    foreach (Appointment appointment in doctorsAppointments)
                    {
                        // Dok god dodavanjem novog appointmenta ne upadamo u appointment
                        // traženog doktora možemo dodavati termine
                        while (startTime.AddHours(1) <= appointment.Beginning)
                        {
                            //Console.WriteLine("Start time je u " + startTime + " a appointment naredni počinje u " + appointment.Beginning);
                            if (startTime.AddHours(1).Hour >= 20)
                            {
                                startTime = MoveStartOfIntervalToTheNextDay(startTime);
                            }
                            if (appointments.Count == 5) break;
                            bool isRoomAvailable = _renovationRepo.IsRoomAvailable(room.Id, startTime, startTime.AddHours(1));
                            if (isRoomAvailable)
                            {
                                //Console.WriteLine("Dodajem termin od " + startTime + " do " + startTime.AddHours(1));
                                Appointment freeAppointment = new Appointment(startTime, startTime.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                                appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                            }
                            startTime = startTime.AddHours(1);
                        }
                        // Pomjeri se na kraj zauzetog termina ukoliko je on nakon početka intervala koji pretražujemo
                        if (appointment.Beginning > startTime) startTime = appointment.Ending;
                    }
                    // Ako smo prošli kroz sve appointmente koje je doktor imao i još nismo našli 5 termina onda
                    // možemo da ih dodajemo pod uslovom da je soba u kojoj se termin održava slobodna
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


        private List<AppointmentView> SecretaryGetAppointmentsWithPriorityOfTime(DateTime startOfInterval, DateTime endOfInterval, int doctorId, int patientId, int oldAppointmentId, SpecType spec)
        {
            Console.WriteLine("PROSAO ULAZ");
            DateTime startOfWorkingHours = CalculateWorkingHours("start", startOfInterval);
            // Kako radno vrijeme počinje u 7 ujutru ukoliko je selektovano ranije treba da se pomjeri
            if (startOfInterval.Hour < 7)
            {
                startOfInterval = startOfWorkingHours;
            }

            DateTime endOfWorkingHours = CalculateWorkingHours("end", endOfInterval);
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

            // Ako je pomjeranje postojećeg appointmenta onda treba obezbijediti da se appointmenta ne može pomjeriti više od 4 dana
            if (oldAppointmentId != -1)
            {
                Appointment oldAppointment = _appointmentRepo.GetById(oldAppointmentId);
            }

            List<AppointmentView> appointments = new List<AppointmentView>();
            List<Appointment> appointmentsOfDoctor = _appointmentRepo.GetAllAppointmentsInTimeIntervalForDoctor(startOfInterval, endOfInterval, doctorId).ToList();

            //Ovo je potrebno za converter iz appointmenta u appointment view
            Doctor doctor = _doctorRepo.GetById(doctorId);
            Room room = _roomRepo.Get(doctor.RoomId);
            User doctorUser = _userRepo.GetById(doctorId);
            // Ovu promjenljivu čuvam u slučaju da prioritet treba da reaguje pa da znam koji je bio originalni početak intervala
            DateTime originalStartOfInterval = startOfInterval;

            // Interval je slobodan
            if (appointmentsOfDoctor.Count == 0)
            {
                appointments = SecretaryGetAppointmentsForFreeTimeInterval(startOfInterval, endOfInterval, appointments, room, doctor, doctorUser, patientId);
                return appointments;
            }

            // Provjeravamo Happy Case
            appointments = SecretaryGetAppointmentsHappyCase(startOfInterval, endOfInterval, appointmentsOfDoctor, appointments, room, doctor, doctorUser, patientId);
            Console.WriteLine("PROSAO HAPPY CASE");
            // Ukoliko nije pronađen nijedan slobodan termin kod doktora u željenom vremenskom intervalu
            // treba da reaguje prioritet i ponudi termine u željenom vremenskom intervalu kod bilo kod doktora
            if (appointments.Count == 0)
            {
                startOfInterval = originalStartOfInterval;
                List<Doctor> doctors = _doctorRepo.GetAllDoctorsBySpecialization(spec).ToList();
                List<Appointment> appointmentsInInterval = _appointmentRepo.GetAllAppointmentsInTimeInterval(startOfInterval, endOfInterval).ToList();

                while (startOfInterval.AddHours(1) <= endOfInterval)
                {
                    if (startOfInterval.AddHours(1).Hour >= 20)
                    {
                        startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                        if (startOfInterval.AddHours(1) > endOfInterval) break;
                    }
                    foreach (Doctor doc in doctors)
                    {
                        if (startOfInterval.AddHours(1).Hour >= 20)
                        {
                            startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                            if (startOfInterval.AddHours(1) > endOfInterval) break;
                        }
                        if (startOfInterval.AddHours(1) > endOfInterval) break;
                        if (appointments.Count == 10) break;

                        List<Appointment> docsAppointments = _appointmentRepo.GetAllAppointmentsInTimeIntervalForDoctor(startOfInterval, startOfInterval.AddHours(1), doc.Id).ToList();
                        // Prioritetni happy case, postoji slobodan termin već na početku traženog intervala
                        if (docsAppointments.Count == 0)
                        {
                            User docUser = _userRepo.GetById(doc.Id);
                            Room generalPracitionerRoom = _roomRepo.Get(doc.RoomId);
                            bool isRoomAvailable = _renovationRepo.IsRoomAvailable(generalPracitionerRoom.Id, startOfInterval, startOfInterval.AddHours(1));
                            if (isRoomAvailable)
                            {
                                Appointment freeAppointment = new Appointment(startOfInterval, startOfInterval.AddHours(1), Appointment.AppointmentType.regular, false, doc.Id, patientId, doc.RoomId);
                                appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, docUser, generalPracitionerRoom));
                            }
                            startOfInterval = startOfInterval.AddHours(1);
                        }
                    }
                    // Ako su svi zauzeti na početku traženog intervala
                    if (appointments.Count == 0)
                    {
                        foreach (Appointment appointmentInInterval in appointmentsInInterval)
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
                            foreach (Doctor doc in doctors)
                            {
                                if (startOfInterval.AddHours(1).Hour >= 20)
                                {
                                    startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                                    if (startOfInterval.AddHours(1) > endOfInterval) break;
                                }
                                if (appointments.Count == 10) break;
                                if (startOfInterval.AddHours(1) > endOfInterval) break;

                                List<Appointment> docsAppointments = _appointmentRepo.GetAllAppointmentsInTimeIntervalForDoctor(startOfInterval, startOfInterval.AddHours(1), doc.Id).ToList();
                                // Prioritetni happy case, postoji slobodan termin na početku traženog intervala
                                if (docsAppointments.Count == 0)
                                {
                                    if (startOfInterval.AddHours(1).Hour >= 20)
                                    {
                                        startOfInterval = MoveStartOfIntervalToTheNextDay(startOfInterval);
                                        if (startOfInterval.AddHours(1) > endOfInterval) break;
                                    }
                                    User docUser = _userRepo.GetById(doc.Id);
                                    Room docRoom = _roomRepo.Get(doc.RoomId);
                                    bool isRoomAvailable = _renovationRepo.IsRoomAvailable(docRoom.Id, startOfInterval, startOfInterval.AddHours(1));
                                    if (isRoomAvailable)
                                    {
                                        Appointment freeAppointment = new Appointment(startOfInterval, startOfInterval.AddHours(1), Appointment.AppointmentType.regular, false, doc.Id, patientId, doc.RoomId);
                                        appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, docUser, docRoom));
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
            return appointmentViews.OrderBy(appointment => appointment.Beginning).ToList(); ;
        }

        public List<AppointmentView> GetPatientsReportsView(int patientId)
        {
            List<AppointmentView> appointmentViews = new List<AppointmentView>();
            List<Appointment> appointments = _appointmentRepo.GetAll().ToList();
            foreach (Appointment appointment in appointments)
            {
                // Odnosi se na termine koji su rezervisani pa tako termini u prošlosti nisu bitni
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
                // Odnosi se na termine koji su rezervisani pa tako termini u prošlosti nisu bitni
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

            List<AppointmentView> MoveAppointmentOptions = GetAvailableAppointmentOptions("No Priority",
                 appointment.Beginning, appointment.Ending.AddDays(4), appointment.DoctorId, appointment.PatientId, -1).ToList();
            
            // Vraca prvi moguci termin za pomeranje
            Console.WriteLine(MoveAppointmentOptions[0].Beginning);
            return MoveAppointmentOptions[0].Beginning;
        }

    }
}
