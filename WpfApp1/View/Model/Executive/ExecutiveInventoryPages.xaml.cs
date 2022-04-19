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
using WpfApp1.Controller;
using WpfApp1.Model.Preview;

namespace WpfApp1.View.Model.Executive
{
    /// <summary>
    /// Interaction logic for ExecutiveInventoryPage.xaml
    /// </summary>
    public partial class ExecutiveInventoryPages : Page
    {
        public List<InventoryPreview> Inventory { get; set; }
        private InventoryController _inventoryController;
        public ExecutiveInventoryPages()
        {
            InitializeComponent();
            this.DataContext = this;
            var app = Application.Current as App;
            _inventoryController = app.InventoryController;
            this.Inventory = _inventoryController.GetPreviews();
        }
        private void AddNewStaticEquipment_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
