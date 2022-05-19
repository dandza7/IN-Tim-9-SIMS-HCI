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
using System.Windows.Shapes;
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.View.Converter;
using WpfApp1.View.Model;
using WpfApp1.View.Model.Secretary;

namespace WpfApp1.View.Dialog
{
    /// <summary>
    /// Interaction logic for SecretaryUpdatePatientDialog.xaml
    /// </summary>
    public partial class SecretaryUpdatePatientDialog : Window, INotifyPropertyChanged
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

        private MedicalRecordController _mrController;

        private AllergyController _allergyController;

        private ObservableCollection<AllergyView> _allergies;

        private ObservableCollection<PatientView> Patients;
        public ObservableCollection<AllergyView> Allergies
        {
            get { return _allergies; }
            set
            {
                if (value != _allergies)
                {
                    _allergies = value;
                    OnPropertyChanged("Allergies");
                }
            }
        }
        public SecretaryUpdatePatientDialog(int patientId)
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;
            _patientController = app.PatientController;
            _userController = app.UserController;
            _mrController = app.MedicalRecordController;
            _allergyController = app.AllergyController;
            Patient p = this._patientController.GetById(patientId);
            MedicalRecord r = this._mrController.GetByPatientId(patientId);
            Console.WriteLine(patientId);
            Console.WriteLine(r.Id);
            Console.WriteLine(p.Username);
            Console.WriteLine(p.PhoneNumber);
            updateidTB.Text = patientId.ToString();
            updatenameTB.Text = p.Name;
            updatesurnameTB.Text = p.Surname;
            updatejmbgTB.Text = p.Jmbg;
            updateusernameTB.Text = p.Username;
            updatepasswordTB.Text = p.Password;
            updateemailTB.Text = p.Email;
            updatecontactTB.Text = p.PhoneNumber;
            updateaddressTB.Text = p.Street;
            updatecityTB.Text = p.City;
            updatecountryTB.Text = p.Country;
            Allergies = new ObservableCollection<AllergyView>(
                AllergyConverter.ConvertAllergyListToAllergyViewList(_allergyController.GetAllAllergiesForPatient(r.Id).ToList()));

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
                updatecontactTB.Text,
                updatejmbgTB.Text,
                User.RoleType.patient
                );
            Patient patient = new Patient(
                int.Parse(updateidTB.Text),
                updateemailTB.Text,
                updateaddressTB.Text,
                updatecityTB.Text,
                updatecountryTB.Text,
                0,
                DateTime.Parse("01.01.2001. 07:00:00")
                );


            _patientController.Update(patient);
            _userController.Update(user);
            DataGrid PatientsDataGrid = (DataGrid)app.Properties["PatientsDataGrid"];
            
            Patients = new ObservableCollection<PatientView>(
            PatientConverter.ConvertPatientListToPatientViewList(_userController.GetAllPatients().ToList()));


            PatientsDataGrid.ItemsSource = Patients;
            PatientsDataGrid.Items.Refresh();
            Close();
        }
        private void DeleteAllergy_Click(object sender, RoutedEventArgs e)
        {
            int allergyId = ((AllergyView)SecretaryAllergiesDataGrid.SelectedItem).Id;
            var app = Application.Current as App;
            _allergyController = app.AllergyController;
            _mrController = app.MedicalRecordController;
            int medicalRecordId = _mrController.GetByPatientId(Int32.Parse(updateidTB.Text)).Id;
            _allergyController.Delete(allergyId);

        }
        private void Manage_Allergies_Click(object sender, RoutedEventArgs e)
        {
            int patientId = Int32.Parse(updateidTB.Text);
            SecretaryManageAllergiesDialog s = new SecretaryManageAllergiesDialog(patientId);
            s.Show();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            UpdateButton.Visibility = Visibility.Visible;
            CloseButton.IsEnabled = false;
            EditButton.IsEnabled = false;
            ViewButton.IsEnabled = true;
            AllergyButton.Visibility = Visibility.Visible;
            CloseButton.Visibility = Visibility.Hidden;
            updatenameTB.IsEnabled = true;
            updatesurnameTB.IsEnabled = true;
            updateaddressTB.IsEnabled = true;
            updatecityTB.IsEnabled = true;
            updatecountryTB.IsEnabled = true;
            updatejmbgTB.IsEnabled = true;
            updateusernameTB.IsEnabled = true;
            updatecontactTB.IsEnabled = true;
            updateemailTB.IsEnabled = true;
            
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            UpdateButton.Visibility = Visibility.Hidden;
            CloseButton.IsEnabled = true;
            ViewButton.IsEnabled = false;
            EditButton.IsEnabled = true;
            AllergyButton.Visibility = Visibility.Hidden;
            CloseButton.Visibility = Visibility.Visible;
            updatenameTB.IsEnabled = false;
            updatesurnameTB.IsEnabled = false;
            updateaddressTB.IsEnabled = false;
            updatecityTB.IsEnabled = false;
            updatecountryTB.IsEnabled = false;
            updatejmbgTB.IsEnabled = false;
            updateusernameTB.IsEnabled = false;
            updatecontactTB.IsEnabled = false;
            updateemailTB.IsEnabled = false;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SecretaryAllergiesDataGrid_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }


}