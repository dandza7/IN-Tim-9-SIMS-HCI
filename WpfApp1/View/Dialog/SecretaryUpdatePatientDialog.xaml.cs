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
        public SecretaryUpdatePatientDialog(int patientId, string patientName, string patientSurname, string patientJMBG, string patientUsername, string patientPassword)
        {
            InitializeComponent();
            DataContext = this;
            updateidTB.Text = patientId.ToString();
            updatenameTB.Text = patientName;
            updatesurnameTB.Text = patientSurname;
            updatejmbgTB.Text = patientJMBG;
            updateusernameTB.Text = patientUsername;
            updatepasswordTB.Text = patientPassword;
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _patientController = app.PatientController;
            Patient patient = new Patient(
                int.Parse(updateidTB.Text),
                updatenameTB.Text,
                updatesurnameTB.Text,
                updatejmbgTB.Text,
                updateusernameTB.Text,
                updatepasswordTB.Text


                );

            _patientController.Update(patient);
            Close();
        }
    }
}
