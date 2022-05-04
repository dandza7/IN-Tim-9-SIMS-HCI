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

        private void AddGuestPatient_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _patientController = app.PatientController;
            Patient patient = new Patient(

                nameTB.Text,
                surnameTB.Text,
                jmbgTB.Text,
                jmbgTB.Text,
                jmbgTB.Text,
                "",
                "",
                "",
                "",
                "",
                0
                );

            _patientController.Create(patient);
            Close();
        }
    }
}
