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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.View.Model.Executive.ExecutiveDrugsDialogs;

namespace WpfApp1.View.Model.Doctor
{
    /// <summary>
    /// Interaction logic for DoctorDrugValidationPage.xaml
    /// </summary>
    /// 

    public partial class DoctorDrugValidationPage : Page
    {
    public DrugController _drugController;
        public ObservableCollection<Drug> DrugsToValidate;
        public DoctorDrugValidationPage()
        {
            InitializeComponent();
            var app = Application.Current as App;
            _drugController = app.DrugController;
            DrugsToValidate = new ObservableCollection<Drug>();
            foreach (Drug drug in _drugController.GetAll()) if (!drug.IsVerified && !drug.IsRejected)DrugsToValidate.Add(drug);
            DrugValidationGrid.ItemsSource = DrugsToValidate;
            this.DataContext = this;
        }

    }
}
