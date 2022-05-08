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
            List<DynamicEquipmentRequest> dynEqRequests = _dynamicEquipmentRequestController.GetAll().ToList();
            ObservableCollection<DynamicEquipmentView> views = new ObservableCollection<DynamicEquipmentView>();
            foreach(DynamicEquipmentRequest dynreq in dynEqRequests)
            {
                if (dynreq.MovingDate <= DateTime.Now)
                {
                    _inventoryController.AddAmount(dynreq.InventoryId,dynreq.Amount);
                    _dynamicEquipmentRequestController.Delete(dynreq.Id);

                }
            }
            List<Inventory> dynamicInventory = _inventoryController.GetAllDynamic().ToList();
            foreach (Inventory inv in dynamicInventory)
            {
                Console.WriteLine(inv.Name);
                
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
    }
}
//            _inventoryMovingController.NewMoving(new InventoryMoving(0, SelectedId, _roomController.GetIdByNametag(MoveNewRoom.Text), DateTime.Parse(MoveDate.Text)));