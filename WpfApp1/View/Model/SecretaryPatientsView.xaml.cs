using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using WpfApp1.View.Dialog;

namespace WpfApp1.View.Model
{
    /// <summary>
    /// Interaction logic for SecretaryPatientsView.xaml
    /// </summary>
    public partial class SecretaryPatientsView : Page, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        private PatientController _patientController;

        private UserController _userController;

        private ObservableCollection<PatientView> _patients;

        public ObservableCollection<PatientView> Patients
        {
            get { return _patients; }
            set
            {
                if (value != _patients)
                {
                    _patients = value;
                    OnPropertyChanged("Patients");
                }
            }
        }
        public SecretaryPatientsView()
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;
            _patientController = app.PatientController;
            _userController = app.UserController;
            Add_Patient_Button.Focus();
            app.Properties["PatientsDataGrid"] = SecretaryPatientsDataGrid;
            List<User> users = _userController.GetAllPatients().ToList();
            ObservableCollection<PatientView> views = new ObservableCollection<PatientView>();

            Patients = new ObservableCollection<PatientView>(
                PatientConverter.ConvertPatientListToPatientViewList(_userController.GetAllPatients().ToList()));
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
            
             Patients = new ObservableCollection<PatientView>(
            PatientConverter.ConvertPatientListToPatientViewList(_userController.GetAllPatients().ToList()));
       
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


    }
}
