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
using System.Windows.Shapes;
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.View.Converter;
using WpfApp1.View.Model.Secretary;

namespace WpfApp1.View.Dialog
{
    /// <summary>
    /// Interaction logic for SecretaryUpdatePatientDialog.xaml
    /// </summary>
    public partial class SecretaryUpdatePatientDialog : Window
    {
        private PatientController _patientController;

        private UserController _userController;

        private MedicalRecordController _mrController;

        private AllergyController _allergyController;

        public ObservableCollection<UserControl> Allergies { get; set; }
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
            updateidTB.Text = patientId.ToString();
            updatenameTB.Text = p.Name;
            updatesurnameTB.Text = p.Surname;
            updatejmbgTB.Text = p.Jmbg;
            updateusernameTB.Text = p.Username;
            updatepasswordTB.Text = p.Password;
            updateemailTB.Text = p.Email;
            updatebrtelTB.Text = p.PhoneNumber;
            Allergies = new ObservableCollection<UserControl>(
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
        private void DeleteAllergy_Click(object sender, RoutedEventArgs e)
        {
            int allergyId = ((AllergyView)SecretaryAllergiesDataGrid.SelectedItem).Id;
            var app = Application.Current as App;
            _allergyController = app.AllergyController;
            _mrController = app.MedicalRecordController;
            int medicalRecordId = _mrController.GetByPatientId(Int32.Parse(updateidTB.Text)).Id;
            _allergyController.Delete(allergyId);
            Allergies = new ObservableCollection<UserControl>(
            AllergyConverter.ConvertAllergyListToAllergyViewList(_allergyController.GetAllAllergiesForPatient(medicalRecordId).ToList()));

            SecretaryAllergiesDataGrid.ItemsSource = Allergies;
            SecretaryAllergiesDataGrid.Items.Refresh();
        }
        private void Manage_Allergies_Click(object sender, RoutedEventArgs e)
        {
            int patientId = Int32.Parse(updateidTB.Text);
            SecretaryManageAllergiesDialog s = new SecretaryManageAllergiesDialog(patientId);
            s.Show();
        }


        
    }


}