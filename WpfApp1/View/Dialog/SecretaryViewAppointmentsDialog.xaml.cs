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
            nameTB.Text = this._userController.GetById(a.PatientId).Name;
            surnameTB.Text = this._userController.GetById(a.PatientId).Surname;
            dnameTB.Text = this._userController.GetById(a.DoctorId).Name.ToString();
            dsurnameTB.Text = this._userController.GetById(a.DoctorId).Surname.ToString();
            bdateTB.Text = a.Beginning.ToString();
            edateTB.Text = a.Ending.ToString();
            roomTB.Text = a.RoomId.ToString();
            urgencyTB.Text = a.IsUrgent.ToString();
            typeTB.Text = a.Type.ToString();

        }

        private void Move_Appointment_Click(object sender, RoutedEventArgs e)
        {
            int appointmentId = Int32.Parse(idTB.Text);
            SecretaryMoveAppointmentDialog s = new SecretaryMoveAppointmentDialog(appointmentId);
            s.Show();
        }
    }
}
