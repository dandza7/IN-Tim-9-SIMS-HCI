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

        public AppointmentService(AppointmentRepository appointmentRepo, 
            DoctorRepository doctorRepository, 
            PatientRepository patientRepo,
            RoomRepository roomRepository,
            UserRepository userRepository)
        {
            _appointmentRepo = appointmentRepo;
            _doctorRepo = doctorRepository;
            _patientRepo = patientRepo;
            _roomRepo = roomRepository;
            _userRepo = userRepository;
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

        public List<AppointmentView> GetAvailableAppointmentOptions(string priority, DateTime startOfInterval, DateTime endOfInterval, int doctorId, int patientId)
        {
            if (priority.Equals("Doctor")) return GetAppointmentsWithPriorityOfDoctor(startOfInterval, endOfInterval, doctorId, patientId);
            return GetAppointmentsWithPriorityOfTime(startOfInterval, endOfInterval, doctorId, patientId);
        }

        private List<AppointmentView> GetAppointmentsWithPriorityOfDoctor(DateTime startOfInterval, DateTime endOfInterval, int doctorId, int patientId)
        {
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
                    if (appointments.Count == 10) break;
                    Appointment freeAppointment = new Appointment(startOfInterval, startOfInterval.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                    appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                    startOfInterval = startOfInterval.AddHours(1);
                }
                return appointments;
            }

            //U Slučaju da u traženom vremenskom intervalu već postoje termini željenog doktora
            while (startOfInterval.AddHours(1) <= endOfInterval)
            {
                foreach (Appointment appointment in appointmentsForDoctor)
                {
                        // Ako je pocetak appointmenta nakon startOfInterval + 1 sat (trajanje termina je jedan sat)
                        // onda imamo slobodan termin za novi appointment
                        while (startOfInterval.AddHours(1) <= appointment.Beginning)
                        {
                            if (appointments.Count == 10) break;
                            Appointment freeAppointment = new Appointment(startOfInterval, startOfInterval.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                            appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
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
                    if (appointments.Count == 10) break;
                    Appointment freeAppointment = new Appointment(startOfInterval, startOfInterval.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                    appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                    startOfInterval = startOfInterval.AddHours(1);
                }
            }
            
            // U slučaju da nismo pronašli nijedan slobodan termin u željenom vremenskom intervalu kod željenog doktora
            // onda treba da reaguje prioritet i ponudi pacijentu prvih 5 slobodnih termina kod njega
            if(appointments.Count == 0)
            {
                List<Appointment> doctorsAppointments = _appointmentRepo.GetAllAppointmentsForDoctor(doctorId).ToList();
                DateTime startTime = DateTime.Today.AddHours(7);
                while (appointments.Count < 5)
                {
                    foreach(Appointment appointment in doctorsAppointments)
                    {
                        while(startTime.AddHours(1) <= appointment.Beginning)
                        {
                            if (appointments.Count == 5) break;
                            Appointment freeAppointment = new Appointment(startTime, startTime.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                            appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                            startTime = startTime.AddHours(1);
                        }
                        startTime = appointment.Ending;
                    }

                    while (appointments.Count < 5)
                    {
                        Appointment freeAppointment2 = new Appointment(startTime, startTime.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                        appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment2, doctorUser, room));
                        startTime = startTime.AddHours(1);
                    }
                }
            }
            return appointments;
        }

        private List<AppointmentView> GetAppointmentsWithPriorityOfTime(DateTime startOfInterval, DateTime endOfInterval, int doctorId, int patientId)
        {
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
                    if (appointments.Count == 10) break;
                    Appointment freeAppointment = new Appointment(startOfInterval, startOfInterval.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                    appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                    startOfInterval = startOfInterval.AddHours(1);
                }
                return appointments;
            }

            while (startOfInterval.AddHours(1) <= endOfInterval)
            {
                foreach (Appointment appointment in appointmentsOfDoctor)
                {
                    // Ako je pocetak appointmenta nakon startOfInterval + 1 sat (trajanje termina je jedan sat)
                    // onda imamo slobodan termin za novi appointment
                    while (startOfInterval.AddHours(1) <= appointment.Beginning)
                    {
                        if (appointments.Count == 10) break;
                        Appointment freeAppointment = new Appointment(startOfInterval, startOfInterval.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                        appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
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
                    if (appointments.Count == 10) break;
                    Appointment freeAppointment = new Appointment(startOfInterval, startOfInterval.AddHours(1), Appointment.AppointmentType.regular, false, doctor.Id, patientId, doctor.RoomId);
                    appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, doctorUser, room));
                    startOfInterval = startOfInterval.AddHours(1);
                }
            }

            // Ukoliko nije pronađen nijedan slobodan termin kod doktora u željenom vremenskom intervalu
            // treba da reaguje prioritet i ponudi termine u željenom vremenskom intervalu kod bilo kod doktora
            if (appointments.Count == 0)
            {
                startOfInterval = originalStartOfInterval;
                List<Doctor> generalPracticioners = _doctorRepo.GetALlGeneralPracticioners().ToList();
                List<Appointment> appointmentsInInterval = _appointmentRepo.GetAllAppointmentsInTimeInterval(startOfInterval, endOfInterval).ToList();

                while(startOfInterval.AddHours(1) <= endOfInterval)
                {
                    foreach(Doctor generalPracticioner in generalPracticioners)
                    {
                        if (startOfInterval.AddHours(1) > endOfInterval) break;
                        if (appointments.Count == 10) break;

                        List<Appointment> generalPracticionersAppointments = _appointmentRepo.GetAllAppointmentsInTimeIntervalForDoctor(startOfInterval, startOfInterval.AddHours(1), generalPracticioner.Id).ToList();
                        // Prioritetni happy case, postoji slobodan termin već na početku traženog intervala
                        if(generalPracticionersAppointments.Count == 0)
                        {
                            User generalPracticionerUser = _userRepo.GetById(generalPracticioner.Id);
                            Room generalPracitionerRoom = _roomRepo.Get(generalPracticioner.RoomId);

                            Appointment freeAppointment = new Appointment(startOfInterval, startOfInterval.AddHours(1), Appointment.AppointmentType.regular, false, generalPracticioner.Id, patientId, generalPracticioner.RoomId);
                            appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, generalPracticionerUser, generalPracitionerRoom));
                            startOfInterval = startOfInterval.AddHours(1);
                        }
                    }
                    // Ako su svi zauzeti na početku traženog intervala
                    if(appointments.Count == 0)
                    {
                        foreach(Appointment appointmentInInterval in appointmentsInInterval)
                        {
                            // Ako bismo time što nađemo appointment probili vremenski interval onda ne možemo da ga nađemo
                            if (startOfInterval.AddHours(1) > endOfInterval) break;
                            if (startOfInterval >= appointmentInInterval.Ending) continue;
                            // Pomijeramo početni interval na kraj appointmenta
                            startOfInterval = appointmentInInterval.Ending;
                            foreach (Doctor generalPracticioner in generalPracticioners)
                            {
                                if (appointments.Count == 10) break;
                                if (startOfInterval.AddHours(1) > endOfInterval) break;

                                List<Appointment> generalPracticionersAppointments = _appointmentRepo.GetAllAppointmentsInTimeIntervalForDoctor(startOfInterval, startOfInterval.AddHours(1), generalPracticioner.Id).ToList();
                                // Prioritetni happy case, postoji slobodan termin na početku traženog intervala
                                if (generalPracticionersAppointments.Count == 0)
                                {
                                    User generalPracticionerUser = _userRepo.GetById(generalPracticioner.Id);
                                    Room generalPracitionerRoom = _roomRepo.Get(generalPracticioner.RoomId);
                                    Appointment freeAppointment = new Appointment(startOfInterval, startOfInterval.AddHours(1), Appointment.AppointmentType.regular, false, generalPracticioner.Id, patientId, generalPracticioner.RoomId);
                                    appointments.Add(AppointmentConverter.ConvertAppointmentAndDoctorToAppointmentView(freeAppointment, generalPracticionerUser, generalPracitionerRoom));
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
                if(appointment.PatientId == patientId)
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


    }
}
