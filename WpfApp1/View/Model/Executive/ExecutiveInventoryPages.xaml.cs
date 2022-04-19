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

namespace WpfApp1.View.Model.Executive
{
    /// <summary>
    /// Interaction logic for ExecutiveInventoryPage.xaml
    /// </summary>
    public partial class ExecutiveInventoryPages : Page, INotifyPropertyChanged
    {
        //--------------------------------------------------------------------------------------------------------
        //          INotifyPropertyChanged fields:
        //--------------------------------------------------------------------------------------------------------
        #region NotifyProperties
        public String _feedback;
        public string Feedback
        {
            get
            {
                return _feedback;
            }
            set
            {
                if (value != _feedback)
                {
                    _feedback = value;
                    OnPropertyChanged("Feedback");
                }
            }
        }

        #endregion
        #region PropertyChangedNotifier
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        //--------------------------------------------------------------------------------------------------------
        //          Basic fields
        //--------------------------------------------------------------------------------------------------------
        public List<InventoryPreview> Inventory { get; set; }
        public List<string> SOPRooms { get; set; }
        private InventoryController _inventoryController;

        //--------------------------------------------------------------------------------------------------------
        //          Constructor code:
        //--------------------------------------------------------------------------------------------------------
        public ExecutiveInventoryPages()
        {
            InitializeComponent();
            this.DataContext = this;
            var app = Application.Current as App;
            _inventoryController = app.InventoryController;
            this.SOPRooms = new List<string>();
            this.Inventory = _inventoryController.GetPreviews();
        }
        //--------------------------------------------------------------------------------------------------------
        //          Global methods code:
        //--------------------------------------------------------------------------------------------------------
        public void RefreshRooms()
        {
            this.SOPRooms = _inventoryController.GetSOPRooms();
            AddRooms.ItemsSource = SOPRooms;
            AddRooms.Items.Refresh();
        }
        public void RefreshInventory()
        {
            this.Inventory = _inventoryController.GetPreviews();
            InventoryDG.ItemsSource = Inventory;
            InventoryDG.Items.Refresh();
        }
        //--------------------------------------------------------------------------------------------------------
        //          Static Equipment Adding code:
        //--------------------------------------------------------------------------------------------------------

        private void AddNewStaticEquipment_Click(object sender, RoutedEventArgs e)
        {
            DialogContainer.Visibility = Visibility.Visible;
            AddContainer.Visibility = Visibility.Visible;
            AddRooms.Text = "";
            AddName.Text = "";
            Feedback = "";
            RefreshRooms();
        }

        private void AddConfirm_Click(object sender, RoutedEventArgs e)
        {
            if(AddRooms.Text == "" || AddName.Text == "")
            {
                Feedback = "*you must fill all fields!";
                return;
            }
            if (AddName.Text.Contains(";"))
            {
                Feedback = "*you can't use semicolon (;) in name!";
                return;
            }
            _inventoryController.Create(new Inventory(0, 0, AddName.Text, "S", 1), AddRooms.Text);
            DialogContainer.Visibility = Visibility.Collapsed;
            AddContainer.Visibility = Visibility.Collapsed;
            AddRooms.Text = "";
            AddName.Text = "";
            Feedback = "";
            RefreshInventory();
        }

        private void XAddButton_Click(object sender, RoutedEventArgs e)
        {
            DialogContainer.Visibility = Visibility.Collapsed;
            AddContainer.Visibility = Visibility.Collapsed;
            AddRooms.Text = "";
            AddName.Text = "";
            Feedback = "";
        }

        //--------------------------------------------------------------------------------------------------------
        //          Static Equipment Moving code:
        //--------------------------------------------------------------------------------------------------------

        private void MoveStaticInventory_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
