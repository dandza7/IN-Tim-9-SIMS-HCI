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
    /// Interaction logic for SecretaryAddGuestPatientDialog.xaml
    /// </summary>
    public partial class SecretaryAddGuestPatientDialog : Window
    {
        public SecretaryAddGuestPatientDialog()
        {
            InitializeComponent();
            DataContext = this;
        }
        private PatientController _patientController;
        private UserController _userController;
        private MedicalRecordController _medicalRecordController;

        private void AddGuestPatient_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _patientController = app.PatientController;
            _userController = app.UserController;
            _medicalRecordController = app.MedicalRecordController;

            User user = new User(

               nameTB.Text,
               surnameTB.Text,
               jmbgTB.Text,
               jmbgTB.Text,
               jmbgTB.Text,
               "",
               User.RoleType.patient

            );
            User u = _userController.Create(user);
            Console.WriteLine(u.Id);
            Patient patient = new Patient(
                u.Id,
                "",
                "",
                "",
                "",
                0,
                DateTime.Parse("01.01.2001. 07:00:00")
            );
            _patientController.Create(patient);
            Close();
        }
    }
}
