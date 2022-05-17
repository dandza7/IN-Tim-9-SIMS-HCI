using System;
using System.Collections.Generic;
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

namespace WpfApp1.View.Model.Executive.ExecutiveDrugsDialogs
{
    /// <summary>
    /// Interaction logic for DrugsInfo.xaml
    /// </summary>
    public partial class DrugsInfo : Page
    {
        ExecutiveDrugsPages ParentPage;
        public DrugsInfo(ExecutiveDrugsPages parent)
        {
            InitializeComponent();
            this.DataContext = this;
            ParentPage = parent;
            DrugName.Text = ParentPage.SelectedDrug.Name;
            DrugStatus.Text = ParentPage.SelectedDrug.IsVerified ? "Verified" : ParentPage.SelectedDrug.IsRejected ? "Rejected" : "Waiting for verification";
            DrugInfo.Text = ParentPage.SelectedDrug.Info;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ParentPage.CloseFrame.Begin();
        }
    }
}
