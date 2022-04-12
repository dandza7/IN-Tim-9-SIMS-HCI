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
            Patients = new ObservableCollection<UserControl>(
PatientConverter.ConvertPatientListToPatientViewList(_patientController.GetAll().ToList()));
            SecretaryPatientsDataGrid.ItemsSource = Patients;
            SecretaryPatientsDataGrid.Items.Refresh();
        }
        private void UpdatePatient_Click(object sender, RoutedEventArgs e)
        {
            int patientId = ((PatientView)SecretaryPatientsDataGrid.SelectedItem).Id;

            SecretaryUpdatePatientDialog s = new SecretaryUpdatePatientDialog(patientId);
            s.Show();
        }

        private void MakeGuest_Click(object sender, RoutedEventArgs e)
        {
            var s = new SecretaryAddGuestPatientDialog();
            s.Show();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Patients = new ObservableCollection<UserControl>(
    PatientConverter.ConvertPatientListToPatientViewList(_patientController.GetAll().ToList()));
            SecretaryPatientsDataGrid.ItemsSource = Patients;
            SecretaryPatientsDataGrid.Items.Refresh();
        }
    }
}
