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
using WpfApp1.View.Model.Secretary;

namespace WpfApp1.View.Dialog
{
    /// <summary>
    /// Interaction logic for SecretaryManageAllergiesDialog.xaml
    /// </summary>
    public partial class SecretaryManageAllergiesDialog : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private MedicalRecordController _mrController;

        private AllergyController _allergyController;

        private ObservableCollection<AllergyView> _allergies;

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

        public SecretaryManageAllergiesDialog(int patientId)
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;
            _mrController = app.MedicalRecordController;
            _allergyController = app.AllergyController;
            patientIdInvisible.Text = patientId.ToString();
            MedicalRecord mr = this._mrController.GetByPatientId(patientId);


            Allergies = new ObservableCollection<AllergyView>(
    AllergyConverter.ConvertAllergyListToAllergyViewList(_allergyController.GetAllAllergiesForPatient(mr.Id).ToList()));

        }
        private void AddAllergy_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _allergyController = app.AllergyController;
            _mrController = app.MedicalRecordController;
            MedicalRecord mr = this._mrController.GetByPatientId(Int32.Parse(patientIdInvisible.Text));
            int mrId = mr.Id;
            Allergy allergy = new Allergy(
                mrId,
                allergynameTB.Text
                );

            _allergyController.Create(allergy);
            SecretaryAllergiesDataGrid.ItemsSource = new ObservableCollection<AllergyView>(
    AllergyConverter.ConvertAllergyListToAllergyViewList(_allergyController.GetAllAllergiesForPatient(mr.Id).ToList()));
            SecretaryAllergiesDataGrid.Items.Refresh();

        }
        private void DeleteAllergy_Click(object sender, RoutedEventArgs e)
        {
            int allergyId = ((AllergyView)SecretaryAllergiesDataGrid.SelectedItem).Id;
            var app = Application.Current as App;
            _allergyController = app.AllergyController;
            _mrController = app.MedicalRecordController;
            int medicalRecordId = _mrController.GetByPatientId(Int32.Parse(patientIdInvisible.Text)).Id;
            _allergyController.Delete(allergyId);
            SecretaryAllergiesDataGrid.ItemsSource = new ObservableCollection<AllergyView>(
AllergyConverter.ConvertAllergyListToAllergyViewList(_allergyController.GetAllAllergiesForPatient(medicalRecordId).ToList()));
            SecretaryAllergiesDataGrid.Items.Refresh();
        }

    }
}
