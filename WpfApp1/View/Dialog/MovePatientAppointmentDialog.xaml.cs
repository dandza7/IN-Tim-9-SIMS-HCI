using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.View.Model;

namespace WpfApp1.View.Dialog
{
    /// <summary>
    /// Interaction logic for MovePatientAppointmentDialog.xaml
    /// </summary>
    public partial class MovePatientAppointmentDialog : Page
    {

        public MovePatientAppointmentDialog()
        {
            InitializeComponent();
            DataContext = this;
        }

        private AppointmentController _appointmentController;
        private DoctorController _doctorController;
        public DateTime Beginning;
        public DateTime Ending;
        public int Id;

        private void MoveAppointment_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            if (BeginningDTP.Text == null || EndingDTP.Text == null) return;
            Doctor doctor = _doctorController.GetById(1);
            Id = (int)app.Properties["appointmentId"];
            Beginning = DateTime.Parse(BeginningDTP.Text);
            Ending = DateTime.Parse(EndingDTP.Text);
            _appointmentController.Update(new Appointment(Id, Beginning, Ending, doctor));
            DataGrid dataView = (DataGrid)app.Properties["DataView"];
            dataView.ItemsSource = null;
            dataView.ItemsSource = _appointmentController.UpdateAppointments();
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientAppointmentsView();
        }
    }
}
