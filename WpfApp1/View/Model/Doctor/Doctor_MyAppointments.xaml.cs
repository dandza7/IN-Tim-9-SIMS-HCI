using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.View.Converter;

namespace WpfApp1.View.Model.Doctor
{
    /// <summary>
    /// Interaction logic for Doctor_MyAppointments.xaml
    /// </summary>
    public partial class Doctor_MyAppointments : Page
    {
        AppointmentController _appointmentController;
        DoctorController _doctorController;
        PatientController _patientController;
        public ObservableCollection<DoctorAppointmentView> futureAppointments = new ObservableCollection<DoctorAppointmentView>();
        public ObservableCollection<DoctorAppointmentView> pastAppointments = new ObservableCollection<DoctorAppointmentView>();

        public int userId = -1;
        public Doctor_MyAppointments()
        {
            InitializeComponent();
            this.DataContext = this;
            var app = Application.Current as App;
            userId = int.Parse(app.Properties["userId"].ToString());
            _appointmentController = app.AppointmentController;
            _doctorController = app.DoctorController;
            _patientController = app.PatientController;
            foreach (Appointment a in _appointmentController.GetAllByDoctorId(userId))
            {//ID DOKTORA


                if (a.Beginning >= DateTime.Now)
                    this.futureAppointments.Add(
                            AppointmentConverter.ConvertAppointmentToDoctorAppointmentView(
                                a,
                                _doctorController.GetById(a.DoctorId),
                                _patientController.GetById(a.PatientId)
                                )
                        );
                else this.pastAppointments.Add(
                            AppointmentConverter.ConvertAppointmentToDoctorAppointmentView(
                                a,
                                _doctorController.GetById(a.DoctorId),
                                _patientController.GetById(a.PatientId)
                                )
                        );
            }
            PastAppointmentsGrid.ItemsSource = pastAppointments;
            FutureAppointmentsGrid.ItemsSource = futureAppointments;

        }
    }
}
