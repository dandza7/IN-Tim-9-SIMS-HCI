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

namespace WpfApp1.View.Dialog
{
    /// <summary>
    /// Interaction logic for SecretaryViewAppointmentsDialog.xaml
    /// </summary>
    public partial class SecretaryViewAppointmentsDialog : Window
    {
        private AppointmentController _appointmentController;
        


        private UserController _userController;

        public SecretaryViewAppointmentsDialog(int appointmentId)
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            _userController = app.UserController;
            Appointment a = this._appointmentController.GetById(appointmentId);
            idTB.Text = appointmentId.ToString();
            patientTB.Text = this._userController.GetById(a.PatientId).Name + " " + this._userController.GetById(a.PatientId).Surname;
            doctorTB.Text = this._userController.GetById(a.DoctorId).Name + " " + this._userController.GetById(a.DoctorId).Surname;
            startingDTB.Text = a.Beginning.ToString();
            endingDTB.Text = a.Ending.ToString();
            roomTB.Text = a.RoomId.ToString();
            urgencyTB.Text = a.IsUrgent.ToString();
            typeTB.Text = a.Type.ToString();

        }

        private void Move_Appointment_Click(object sender, RoutedEventArgs e)
        {
            Appointment a = this._appointmentController.GetById(int.Parse(idTB.Text));
            SecretaryMoveAppointmentDialog s = new SecretaryMoveAppointmentDialog(a);
            s.Show();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
