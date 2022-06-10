using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WpfApp1.View.Dialog
{
    /// <summary>
    /// Interaction logic for SecretaryAddDynamicEquipmentDialog.xaml
    /// </summary>
    public partial class SecretaryAddDynamicEquipmentDialog : Window
    {
        private InventoryController _inventoryController;

        private DynamicEquipmentRequestController _dynamicEquipmentRequestController;
        public SecretaryAddDynamicEquipmentDialog(int dynEqId)
        {
            InitializeComponent();
            DataContext = this;

            var app = Application.Current as App;
            InventoryController _inventoryController = app.InventoryController;

            idTB.Content = dynEqId.ToString();
            nameTB.Content = _inventoryController.GetById(dynEqId).Name;
        }

        private void Add_DynamicEquipment_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            DynamicEquipmentRequestController _dynamicReqController = app.DynamicEquipmentReqeustController;

            DynamicEquipmentRequest der = new DynamicEquipmentRequest(nameTB.Content.ToString(), int.Parse(amountTB.Text), DateTime.Now.AddSeconds(10));
            _dynamicReqController.Create(der);

            Close();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
