﻿using System;
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
        private int _appointmentMaxId;

        public AppointmentRepository(string path, string delimiter, string datetimeFormat)
        {
            _path = path;
            _delimiter = delimiter;
            _datetimeFormat = datetimeFormat;
        }

        private long GetMaxId(IEnumerable<Appointment> appointments)
        {
            return appointments.Count() == 0 ? 0 : appointments.Max(transaction => transaction.Id);
        }

        public IEnumerable<Appointment> GetAll()
        {
            return File.ReadAllLines(_path)
                .Select(ConvertCSVFormatToAppointment)
                .ToList();
        }
        public Appointment Create(Appointment appointment)
        {
            appointment.Id = ++_appointmentMaxId;
            AppendLineToFile(_path, ConvertAppointmentToCSVFormat(appointment));
            return appointment;
        }

        private Appointment ConvertCSVFormatToAppointment(string appointmentCSVFormat)
        {
            var tokens = appointmentCSVFormat.Split(_delimiter.ToCharArray());
            //Console.Write("ID pregleda: {0} Datum pocetka: {1} Datum kraja: {2}\n", int.Parse(tokens[0]), DateTime.Parse(tokens[1]), DateTime.Parse(tokens[2]));
            return new Appointment(int.Parse(tokens[0]), DateTime.Parse(tokens[1]), DateTime.Parse(tokens[2]));
        }
        private string ConvertAppointmentToCSVFormat(Appointment appointment)
        {
            return string.Join(_delimiter,
                appointment.Id,
                appointment.Beginning.ToString(_datetimeFormat),
                appointment.Ending.ToString(_datetimeFormat));
        }

        private void AppendLineToFile(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }
    }
}
