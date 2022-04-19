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
using WpfApp1.Model;

namespace WpfApp1.View.Model.Patient
{
    /// <summary>
    /// Interaction logic for PatientProfileView.xaml
    /// </summary>
    public partial class PatientProfileView : Page
    {
        private PatientController _patientController;
        /*private TherapyController _therapyController;
        private DrugController _drugController;*/

        public ObservableCollection<Notification> Notifications { get; set; }
        public PatientProfileView()
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;
            /*_patientController = app.PatientController;
            _therapyController = app.TherapyController;
            _drugController = app.DrugController;*/

            Notifications = new ObservableCollection<Notification>(_patientController.GetPatientsNotifications(3));
            /*List<Therapy> therapies = _patientController.GetPatientsTherapies(3).ToList();
            List<Drug> drugs = new List<Drug>();
            therapies.ForEach(therapy => drugs.Add(_drugController.GetById(therapy.DrugId)));
            Console.WriteLine("Terapije koje uzima pacijent sa Id-em 3 su:");
            foreach(Drug drug in drugs)
            {
                Console.WriteLine(drug.Name);
            }*/
        }
    }
}
