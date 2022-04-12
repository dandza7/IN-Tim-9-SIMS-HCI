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
using WpfApp1.View.Converter;
using WpfApp1.View.Dialog;

namespace WpfApp1.View.Model
{
    /// <summary>
    /// Interaction logic for SecretaryPatientsView.xaml
    /// </summary>
    public partial class SecretaryPatientsView : UserControl
    {
        private PatientController _patientController;
        public ObservableCollection<UserControl> Patients { get; set; }

        public SecretaryPatientsView()
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;
            _patientController = app.PatientController;

            Patients = new ObservableCollection<UserControl>(
                PatientConverter.ConvertPatientListToPatientViewList(_patientController.GetAll().ToList()));
        }
        private void AddPatient_Click(object sender, RoutedEventArgs e)
        {
            var s = new SecretaryAddPatientDialog();
            s.Show();
        }
        private void DeletePatient_Click(object sender, RoutedEventArgs e)
        {
            int patientId = ((PatientView)SecretaryPatientsDataGrid.SelectedItem).Id;
            var app = Application.Current as App;
            _patientController = app.PatientController;

            _patientController.Delete(patientId);

        }
        private void UpdatePatient_Click(object sender, RoutedEventArgs e)
        {
            int patientId = ((PatientView)SecretaryPatientsDataGrid.SelectedItem).Id;
            string patientName = ((PatientView)SecretaryPatientsDataGrid.SelectedItem).Name;
            string patientSurname = ((PatientView)SecretaryPatientsDataGrid.SelectedItem).Surname;
            string patientJMBG = ((PatientView)SecretaryPatientsDataGrid.SelectedItem).JMBG;
            string patientUsername = ((PatientView)SecretaryPatientsDataGrid.SelectedItem).Username;
            string patientPassword = ((PatientView)SecretaryPatientsDataGrid.SelectedItem).Password;
            SecretaryUpdatePatientDialog s = new SecretaryUpdatePatientDialog(patientId, patientName, patientSurname, patientJMBG, patientUsername, patientPassword);
            s.Show();
        }
    }
}
