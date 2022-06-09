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
            foreach (Drug drug in _drugController.GetAll()) if (!drug.IsVerified && !drug.IsRejected) DrugsToValidate.Add(drug);
            DrugValidationGrid.ItemsSource = DrugsToValidate;
            this.DataContext = this;
        }

        private void DeclineBT_Click(object sender, RoutedEventArgs e)
        {
            CommentLabel.Visibility = Visibility.Visible;
            CommentExceptionLabel.Visibility = Visibility.Hidden;
            if (CommentTB.Text == "")
            {
                CommentLabel.Visibility = Visibility.Hidden;
                CommentExceptionLabel.Visibility = Visibility.Visible;
            }
            else
            {
                Drug drug = _drugController.GetById(((Drug)DrugValidationGrid.SelectedItems[0]).Id);
                drug.IsRejected = true;
                drug.Comment = CommentTB.Text;
                _drugController.Update(drug);
                NameLabel.Content = "";
                InformationTB.Clear();
                CommentTB.Clear();
            }
        }
        private void VerifyBT_Click(object sender, RoutedEventArgs e)
        {

            Drug drug = _drugController.GetById(((Drug)DrugValidationGrid.SelectedItems[0]).Id);
            drug.IsVerified = true;
            _drugController.Update(drug);
            NameLabel.Content = "";
            InformationTB.Clear();
            CommentTB.Clear();

        }

        private void DrugValidationGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Drug drug = _drugController.GetById(((Drug)DrugValidationGrid.SelectedItems[0]).Id);
            NameLabel.Content = drug.Name;
            InformationTB.Text = drug.Info;
        }
    }
}
