using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class AppointmentController
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public IEnumerable<Appointment> GetAll()
        {
            //Console.WriteLine("nesto\n");
            return _appointmentService.GetAll();
        }

        public Appointment Create(Appointment appointment)
        {
            return _appointmentService.Create(appointment);
        }

        public Appointment Update(Appointment appointment)
        {
            return _appointmentService.Update(appointment);
        }

        public bool Delete(int id)
        {
            return  _appointmentService.Delete(id);
        }

        public List<Appointment> UpdateAppointments()
        {
            return _appointmentService.UpdateAppointments();
        }
    }
}
