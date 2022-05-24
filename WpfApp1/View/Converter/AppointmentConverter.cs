using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.View.Model.Doctor;
using WpfApp1.View.Model.Patient;
using WpfApp1.View.Model.Secretary;

namespace WpfApp1.View.Converter
{
    internal class AppointmentConverter: AbstractConverter
    {
        public static AppointmentView ConvertAppointmentAndDoctorToAppointmentView(Appointment appointment, User user, Room room)
        => new AppointmentView
        {
            Id = appointment.Id,
            Beginning = appointment.Beginning,
            Username = user.Username,
            NameTag = room.Nametag
        };

        public static SecretaryAppointmentView ConvertSecretaryAppointmentSecretaryAppointmentView(Appointment appointment, User doctor, User patient)
        => new SecretaryAppointmentView
        {
            Id = appointment.Id,
            Beginning = appointment.Beginning,
            Patient = patient.Name + " " + patient.Surname,
            Doctor = doctor.Name + " " + doctor.Surname

        };
        public static DoctorAppointmentView ConvertAppointmentToDoctorAppointmentView(Appointment appointment, User doctor, User patient)
        => new DoctorAppointmentView
        {
            Id = appointment.Id,
            Beginning = appointment.Beginning,
            Ending = appointment.Ending,
            Type = appointment.Type.ToString(),
            Urgent = appointment.IsUrgent.ToString(),
            Patient = patient.Name + " " + patient.Surname,
            Doctor = doctor.Name + " " + doctor.Surname,
            PatientId = appointment.PatientId,
            DoctorId = appointment.DoctorId

        };


    }
}
