using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using WpfApp1.View.Converter;
using WpfApp1.View.Dialog.PatientDialog;
using WpfApp1.View.Model;

namespace WpfApp1.View.Dialog
{
    /// <summary>
    /// Interaction logic for SecretaryAddPatientDialog.xaml
    /// </summary>
    public partial class SecretaryAddPatientDialog : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    
        public SecretaryAddPatientDialog()
        {
            InitializeComponent();
            DataContext = this;
        }
        private PatientController _patientController;
        private UserController _userController;
        private MedicalRecordController _medicalRecordController;
        private ObservableCollection<PatientView> Patients;
        private string _emailValidation;
        private double _jmbgValidation;
        public string Email_Validation
        {
            get
            {
                return _emailValidation;
            }
            set
            {
                if (value != _emailValidation)
                {
                    _emailValidation = value;
                    OnPropertyChanged("Email_Validation");
                }
            }
        }
        public double Jmbg_Validation
        {
            get
            {
                return _jmbgValidation;
            }
            set
            {
                if (value != _jmbgValidation)
                {
                    _jmbgValidation = value;
                    OnPropertyChanged("Jmbg_Validation");
                }
            }
        }
        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _patientController = app.PatientController;
            _userController = app.UserController;
            _medicalRecordController = app.MedicalRecordController;

            if (_patientController.GetByUsername(usernameTB.Text) == null)
            {

                User user = new User(

                   nameTB.Text,
                   surnameTB.Text,
                   jmbgTB.Text,
                   usernameTB.Text,
                   passwordTB.Text,
                   contactTB.Text,
                   User.RoleType.patient
                );

                User u = _userController.Create(user);
                Console.WriteLine(u.Id);
                Patient patient = new Patient(
                    u.Id,
                    emailTB.Text,
                    addressTB.Text,
                    cityTB.Text,
                    countryTB.Text,
                    0,
                    DateTime.Parse("01.01.2001. 07:00:00")
                );
                _patientController.Create(patient);
                DataGrid PatientsDataGrid = (DataGrid)app.Properties["PatientsDataGrid"];

                Patients = new ObservableCollection<PatientView>(
                PatientConverter.ConvertPatientListToPatientViewList(_userController.GetAllPatients().ToList()));


                PatientsDataGrid.ItemsSource = Patients;
                PatientsDataGrid.Items.Refresh();
                Close();
            }
            else
            {
                MessageBox.Show("Username you entered is already taken.","Username taken", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void CloseCommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Close?", "Close", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                this.Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TextValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z]");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void JMBGValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]{13}$");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
