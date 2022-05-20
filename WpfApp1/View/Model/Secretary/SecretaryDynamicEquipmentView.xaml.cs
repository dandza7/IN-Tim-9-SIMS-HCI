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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.View.Converter;
using WpfApp1.View.Dialog;

namespace WpfApp1.View.Model.Secretary
{
    /// <summary>
    /// Interaction logic for SecretaryDynamicEquipmentView.xaml
    /// </summary>
    public partial class SecretaryDynamicEquipmentView : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        private InventoryController _inventoryController;
        private DynamicEquipmentRequestController _dynamicEquipmentRequestController;

        public ObservableCollection<DynamicEquipmentView> DynamicEquipment { get; set; }

        public SecretaryDynamicEquipmentView()
        {
            InitializeComponent();
            DataContext = this;

            var app = Application.Current as App;
            InventoryController _inventoryController = app.InventoryController;
            DynamicEquipmentRequestController _dynamicEquipmentRequestController = app.DynamicEquipmentReqeustController;
            Add_New_Dynamic_Equipment.Focus();
            _dynamicEquipmentRequestController.UpdateDynamicEquipment();
            List<Inventory> dynamicInventory = _inventoryController.GetAllDynamic().ToList();
            ObservableCollection<DynamicEquipmentView> views = new ObservableCollection<DynamicEquipmentView>();

            foreach (Inventory inv in dynamicInventory)
            {
                views.Add(DynamicEquipmentConverter.ConvertDynEqToDynEqView(inv));
            }

            DynamicEquipment = views;
        }
        private void Add_Dynamic_Equipment_Click(object sender, RoutedEventArgs e)
        {
            int invId = ((DynamicEquipmentView)SecretaryDynamicEquipmentDataGrid.SelectedItem).Id;
            var s = new SecretaryAddDynamicEquipmentDialog(invId);
            s.Show();
        }

        private void Add_New_Dynamic_Equipment_Click(object sender, RoutedEventArgs e)
        {
            var s = new SecretaryAddNewDynamicEquipmentDialog();
            s.Show();
        }

    }

}