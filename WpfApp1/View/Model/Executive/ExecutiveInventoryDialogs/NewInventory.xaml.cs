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
using WpfApp1.ViewModel;

namespace WpfApp1.View.Model.Executive.ExecutiveInventoryDialogs
{
    /// <summary>
    /// Interaction logic for NewInventory.xaml
    /// </summary>
    
    public partial class NewInventory : Page
        { 
        public NewInventory(ExecutiveInventoryPages parent)
        {
            InitializeComponent();
            this.DataContext = new NewInventoryViewModel(parent);
        }
    }
}
