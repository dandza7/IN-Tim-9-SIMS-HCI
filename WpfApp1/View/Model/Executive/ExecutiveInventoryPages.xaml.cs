using System;
using System.Collections.Generic;
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
using WpfApp1.Model;
using WpfApp1.Controller;
using WpfApp1.Model.Preview;
using WpfApp1.View.Model.Executive.ExecutiveInventoryDialogs;
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;
using WpfApp1.ViewModel;

namespace WpfApp1.View.Model.Executive
{
    /// <summary>
    /// Interaction logic for ExecutiveInventoryPage.xaml
    /// </summary>
    public partial class ExecutiveInventoryPages : Page
    {  
        public Storyboard OpenFrame { get; set; }
        public Storyboard CloseFrame { get; set; }
        public Storyboard RefreshDG { get; set; }

        public ExecutiveInventoryPages()
        {
            InitializeComponent();
            OpenFrame = FindResource("OpenFrame") as Storyboard;
            CloseFrame = FindResource("CloseFrame") as Storyboard;
            RefreshDG = FindResource("RefreshDG") as Storyboard;
            this.DataContext = new InventoryViewModel(this);
        }

        private void CloseFrame_Completed(object sender, EventArgs e)
        {
            FormFrame.Content = null;
            FormFrame.Opacity = 1;
        }

    }
}
