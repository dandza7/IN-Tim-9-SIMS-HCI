using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.View.Model;

namespace WpfApp1.View.Converter
{
    internal class AppointmentConverter: AbstractConverter
    {
        public static AppointmentView ConvertAppointmentAndDoctorToAppointmentView(Appointment appointment, Doctor doctor)
        => new AppointmentView
        {
            Id = appointment.Id,
            Beginning = appointment.Beginning,
            Username = doctor.Username
        };
    }
}
