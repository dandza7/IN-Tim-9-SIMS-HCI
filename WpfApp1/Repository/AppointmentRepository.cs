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
        private string _path;
        private string _delimiter;

        public AppointmentRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
        }
        public IEnumerable<Appointment> GetAll()
        {
            return File.ReadAllLines(_path)
                .Select(ConvertCSVFormatToAppointment)
                .ToList();
        }
        private Appointment ConvertCSVFormatToAppointment(string appointmentCSVFormat)
        {
            var tokens = appointmentCSVFormat.Split(_delimiter.ToCharArray());
            return new Appointment(int.Parse(tokens[0]), DateTime.Parse(tokens[1]), DateTime.Parse(tokens[2]));
        }
    }
}
