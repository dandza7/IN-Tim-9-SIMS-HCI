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
            idTB.Text = dynEqId.ToString();
            nameTB.Text = _inventoryController.GetById(dynEqId).Name;
        }

        private void Add_DynamicEquipment_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            DynamicEquipmentRequestController _dynamicReqController = app.DynamicEquipmentReqeustController;
            DynamicEquipmentRequest der = new DynamicEquipmentRequest(int.Parse(idTB.Text), int.Parse(amountTB.Text), DateTime.Now.AddSeconds(10));
            _dynamicReqController.Create(der);
            Close();
        }
    }
}
