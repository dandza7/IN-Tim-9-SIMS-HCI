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
    /// Interaction logic for SecretaryUpdatePatientDialog.xaml
    /// </summary>
    public partial class SecretaryUpdatePatientDialog : Window
    {
        private PatientController _patientController;

        private UserController _userController;
        public SecretaryUpdatePatientDialog(int patientId)
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;
            _patientController = app.PatientController;
            _userController = app.UserController;
            Patient p =  this._patientController.GetById(patientId);
            updateidTB.Text = patientId.ToString();
            updatenameTB.Text = p.Name;
            updatesurnameTB.Text = p.Surname;
            updatejmbgTB.Text = p.Jmbg;
            updateusernameTB.Text = p.Username;
            updatepasswordTB.Text = p.Password;
            updateemailTB.Text = p.Email;
            updatebrtelTB.Text = p.PhoneNumber;
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _patientController = app.PatientController;
            _userController = app.UserController;
            User user = new User(
                int.Parse(updateidTB.Text),
                updatenameTB.Text,
                updatesurnameTB.Text,
                updateusernameTB.Text,
                updatepasswordTB.Text,
                updatebrtelTB.Text,
                updatejmbgTB.Text,
                User.RoleType.patient
                );
            Patient patient = new Patient(
                int.Parse(updateidTB.Text),
                updateemailTB.Text
                );


            _patientController.Update(patient);
            _userController.Update(user);
            Close();
        }
    }
}