using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository.Interfaces;
using static WpfApp1.Model.Appointment;

namespace WpfApp1.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private string _path;
        private string _delimiter;
        private readonly string _datetimeFormat;

        public AppointmentRepository(string path, string delimiter, string datetimeFormat)
        {
            _path = path;
            _delimiter = delimiter;
            _datetimeFormat = datetimeFormat;
        }

        private int GetMaxId(IEnumerable<Appointment> appointments)
        {
            return appointments.Count() == 0 ? 0 : appointments.Max(appointment => appointment.Id);
        }

        public IEnumerable<Appointment> GetAll()
        {
            List<string> lines = File.ReadAllLines(_path).ToList();
            List<Appointment> appointments = new List<Appointment>();
            foreach (string line in lines)
            {
                if (line == "") continue;
                appointments.Add(ConvertCSVFormatToAppointment(line));
            }
            return appointments;
        }

        public IEnumerable<Appointment> GetAllAppointmentsInTimeInterval(DateTime startOfInterval, DateTime endOfInterval)
        {
            List<Appointment> allAppointments = GetAll().ToList();
            List<Appointment> appointmentsInTimeInterval = new List<Appointment>();

            foreach (Appointment appointment in allAppointments)
            {
                if (appointment.Ending <= startOfInterval || appointment.Beginning >= endOfInterval) continue;
                appointmentsInTimeInterval.Add(appointment);
            }

            return appointmentsInTimeInterval.OrderBy(appointment => appointment.Beginning).ToList();
        }

        public IEnumerable<Appointment> GetAllAppointmentsInTimeIntervalForDoctor(DateTime startOfInterval, DateTime endOfInterval, int doctorId)
        {
            List<Appointment> appointmentsInTimeInterval = GetAllAppointmentsInTimeInterval(startOfInterval, endOfInterval).ToList();
            List<Appointment> doctorsAppointmentsInTimeInterval = new List<Appointment>();

            foreach (Appointment appointment in appointmentsInTimeInterval)
            {
                if (appointment.DoctorId == doctorId)
                {
                    doctorsAppointmentsInTimeInterval.Add(appointment);
                }
            }

            return doctorsAppointmentsInTimeInterval.OrderBy(appointment => appointment.Beginning).ToList();
        }
        public IEnumerable<Appointment> GetDoctorsMovableAppointments(DateTime startOfInterval, DateTime endOfInterval, int doctorId)
        {
            List<Appointment> allAppointments = GetAllAppointmentsInTimeInterval(startOfInterval, endOfInterval).ToList();
            List<Appointment> doctorsAppointments = new List<Appointment>();

            foreach (Appointment appointment in allAppointments)
            {
                if (appointment.DoctorId == doctorId && appointment.IsUrgent != true && appointment.Type != AppointmentType.surgery)
                {
                    doctorsAppointments.Add(appointment);
                }
            }

            return doctorsAppointments.ToList();
        }

        public IEnumerable<Appointment> GetAllAppointmentsForDoctor(int doctorId)
        {
            List<Appointment> allAppointments = GetAll().ToList();
            List<Appointment> appointments = new List<Appointment>();

            foreach (Appointment appointment in allAppointments)
            {
                if (appointment.DoctorId == doctorId)
                {
                    appointments.Add(appointment);
                }
            }

            return appointments.OrderBy(appointment => appointment.Beginning).ToList();
        }

        public IEnumerable<Appointment> GetAllAppointmentsForPatient(int patientId)
        {
            List<Appointment> allAppointments = GetAll().ToList();
            List<Appointment> appointmentsForPatient = new List<Appointment>();

            foreach (Appointment appointment in allAppointments)
            {
                if (appointment.PatientId == patientId)
                {
                    appointmentsForPatient.Add(appointment);
                }
            }

            return appointmentsForPatient.OrderBy(appointment => appointment.Beginning).ToList();
        }

        public IEnumerable<Appointment> GetAllByPatientId(int patientId)
        {
            List<Appointment> appointmentsForDoctor = new List<Appointment>();

            foreach (Appointment appointment in GetAll().ToList())
            {
                if (appointment.PatientId == patientId)
                {
                    appointmentsForDoctor.Add(appointment);
                }
            }

            return (IEnumerable<Appointment>)appointmentsForDoctor;
        }

        public Appointment Create(Appointment appointment)
        {
            int maxId = GetMaxId(GetAll());
            appointment.Id = ++maxId;
            AppendLineToFile(_path, ConvertAppointmentToCSVFormat(appointment));
            return appointment;
        }

        public Appointment Update(Appointment appointment)
        {
            List<Appointment> appointments = GetAll().ToList();
            List<string> newFile = new List<string>();
            foreach(Appointment a in appointments)
            {

                if(a.Id == appointment.Id)
                {
                    a.Beginning = appointment.Beginning;
                    a.Ending = appointment.Ending;
                    a.DoctorId = appointment.DoctorId;
                    a.PatientId = appointment.PatientId;
                    a.IsUrgent = appointment.IsUrgent;
                }
                newFile.Add(ConvertAppointmentToCSVFormat(a));
            }
            File.WriteAllLines(_path, newFile);
            return appointment;
        }

        public List<Appointment> GetAllByDoctorId(int id)
        {
            List<Appointment> doctorsAppointments = new List<Appointment>();
            foreach (Appointment a in GetAll())
            {
                if (a.DoctorId == id) doctorsAppointments.Add(a);
            }

            return doctorsAppointments;
        }

        public bool Delete(int id)
        {
            List<Appointment> appointments = GetAll().ToList();
            List<string> newFile = new List<string>();
            bool isDeleted = false;
            foreach (Appointment a in appointments)
            {
                if (a.Id != id)
                {
                    newFile.Add(ConvertAppointmentToCSVFormat(a));
                    isDeleted = true;
                }
            }
            File.WriteAllLines(_path, newFile);
            return isDeleted;
        }
        public Appointment GetById(int appointmentId)
        {
            return GetAll().ToList().SingleOrDefault(appointment => appointment.Id == appointmentId);
        }

        private Appointment ConvertCSVFormatToAppointment(string appointmentCSVFormat)
        {
            var tokens = appointmentCSVFormat.Split(_delimiter.ToCharArray());
            Enum.TryParse(tokens[3], true, out AppointmentType type);
            return new Appointment(int.Parse(tokens[0]), 
                DateTime.Parse(tokens[1]), 
                DateTime.Parse(tokens[2]), 
                type, 
                bool.Parse(tokens[4]), 
                int.Parse(tokens[5]), 
                int.Parse(tokens[6]), 
                int.Parse(tokens[7]));
        }
        private string ConvertAppointmentToCSVFormat(Appointment appointment)
        {
            return string.Join(_delimiter,
                appointment.Id,
                appointment.Beginning.ToString(_datetimeFormat),
                appointment.Ending.ToString(_datetimeFormat),
                appointment.Type.ToString(),
                appointment.IsUrgent.ToString(),
                appointment.DoctorId.ToString(),
                appointment.PatientId.ToString(),
                appointment.RoomId.ToString());
        }

        private void AppendLineToFile(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }
    }
}