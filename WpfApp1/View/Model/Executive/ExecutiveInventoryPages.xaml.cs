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

        public String _wrongSelection;
        public string WrongSelection
        {
            get
            {
                return _wrongSelection;
            }
            set
            {
                if (value != _wrongSelection)
                {
                    _wrongSelection = value;
                    OnPropertyChanged("WrongSelection");
                }
            }
        }
        private List<InventoryPreview> _inventory;
        public List<InventoryPreview> Inventory
        {
            get
            {
                return _inventory;
            }
            set
            {
                if (value != _inventory)
                {
                    _inventory = value;
                    OnPropertyChanged("Inventory");
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
        
        public List<string> SOPRooms { get; set; }
        private InventoryController _inventoryController;
        public InventoryController InventoryController { get { return _inventoryController; } }
        private InventoryMovingController _inventoryMovingController;
        public InventoryMovingController InventoryMovingController { get { return _inventoryMovingController; } }
        private RoomController _roomController;
        public RoomController RoomController { get { return _roomController; } }
        public int SelectedId { get; set; }
        public string SelectedRoomName { get; set; }
        public string SelectedInventoryName { get; set; }
        public Storyboard FrameAnimation { get; set; }
        public Storyboard CloseFrame { get; set; }
        //--------------------------------------------------------------------------------------------------------
        //          Constructor code:
        //--------------------------------------------------------------------------------------------------------
        public ExecutiveInventoryPages()
        {
            InitializeComponent();
            this.DataContext = this;
            var app = Application.Current as App;
            _inventoryController = app.InventoryController;
            _inventoryMovingController = app.InventoryMovingController;
            _roomController = app.RoomController;
            this.SOPRooms = new List<string>();
            this.Inventory = _inventoryController.GetPreviews();
            this.Feedback = "";
            this.WrongSelection = "";
            SelectedId = -1;
            SelectedRoomName = "";
            SelectedInventoryName = "";
            this.FrameAnimation = FindResource("FormFrameAnimation") as Storyboard;
            CloseFrame = FindResource("CloseFrame") as Storyboard;
        }


        private void AddNewStaticEquipment_Click(object sender, RoutedEventArgs e)
        {
            FormFrame.Content = new NewInventory(this);
            FrameAnimation.Begin();
        }


        //--------------------------------------------------------------------------------------------------------
        //          Static Equipment Moving code:
        //--------------------------------------------------------------------------------------------------------

        private void MoveStaticInventory_Click(object sender, RoutedEventArgs e)
        {
            if (InventoryDG.SelectedItems.Count == 0)
            {
                WrongSelection = "You must select inventory for moving first!";
                WrongSelectionContainer.Visibility = Visibility.Visible;
                return;
            }
            InventoryPreview i = (InventoryPreview)InventoryDG.SelectedItems[0];
            if (i.Type.Equals("D"))
            {
                WrongSelection = "You can only move static inventory!";
                WrongSelectionContainer.Visibility = Visibility.Visible;
                return;
            }
            SelectedRoomName = i.Room;
            SelectedInventoryName = i.Name;
            SelectedId = i.Id;
            FormFrame.Content = new MoveInventory(this);
            FrameAnimation.Begin();

        }
        private void WrongSelectionOK_Click(object sender, RoutedEventArgs e)
        {
            WrongSelectionContainer.Visibility = Visibility.Collapsed;
        }
        private void CloseFrame_Completed(object sender, EventArgs e)
        {
            FormFrame.Content = null;
            FormFrame.Opacity = 1;
        }
    }
}
