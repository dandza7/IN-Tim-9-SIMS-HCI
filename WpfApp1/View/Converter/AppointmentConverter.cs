using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.View.Model;

namespace WpfApp1.View.Converter
{
    class AppointmentConverter : AbstractConverter
    {
        public static AppointmentView ConvertAppointmentToAppointmentView(Appointment appointment)
            => new AppointmentView
            {
                Id = appointment.Id,
                Beginning = appointment.Beginning,
                Ending = appointment.Ending
            };

        public static IList<AppointmentView> ConvertAppointmentListToAppointmentViewList(IList<Appointment> appointments)
            => ConvertEntityListToViewList(appointments, ConvertAppointmentToAppointmentView);
    }
}
