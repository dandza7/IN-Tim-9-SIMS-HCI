using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository
{
    public class AppointmentRepository
    {
        private const string NOT_FOUND_ERROR = "Appointment with {0}:{1} can not be found!";
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
            /*return File.ReadAllLines(_path)
                .Select(ConvertCSVFormatToAppointment)
                .ToList();*/
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
                }
                newFile.Add(ConvertAppointmentToCSVFormat(a));
            }
            File.WriteAllLines(_path, newFile);
            return appointment;
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

        private Appointment ConvertCSVFormatToAppointment(string appointmentCSVFormat)
        {
            var tokens = appointmentCSVFormat.Split(_delimiter.ToCharArray());
            return new Appointment(int.Parse(tokens[0]), DateTime.Parse(tokens[1]), DateTime.Parse(tokens[2]), int.Parse(tokens[3]));
        }
        private string ConvertAppointmentToCSVFormat(Appointment appointment)
        {
            return string.Join(_delimiter,
                appointment.Id,
                appointment.Beginning.ToString(_datetimeFormat),
                appointment.Ending.ToString(_datetimeFormat),
                appointment.DoctorId.ToString());
        }

        private void AppendLineToFile(string path, string line)
        {
            Console.WriteLine("Linija koju dodajem je:\n{0}", line);
            File.AppendAllText(path, line + Environment.NewLine);
        }

        public List<Appointment> UpdateAppointments()
        {
            return GetAll().ToList(); 
        }
    }
}
