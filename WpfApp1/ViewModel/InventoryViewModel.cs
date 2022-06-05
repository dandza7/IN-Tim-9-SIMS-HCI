using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Controller;
using WpfApp1.Model.Preview;
using WpfApp1.View.Model.Executive;
using WpfApp1.View.Model.Executive.ExecutiveInventoryDialogs;
using WpfApp1.ViewModel.Commands.Executive;

namespace WpfApp1.ViewModel
{
    internal class InventoryViewModel : INotifyPropertyChanged
    {
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
        private List<InventoryPreview> _inventorySource;
        public List<InventoryPreview> InventorySource
        {
            get
            {
                return _inventorySource;
            }
            set
            {
                if (value != _inventorySource)
                {
                    _inventorySource = value;
                    OnPropertyChanged("InventorySource");
                }
            }
        }
        private ObservableCollection<InventoryPreview> _inventory;
        public ObservableCollection<InventoryPreview> Inventory
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
        private string _searchToken;
        public string SearchToken
        {
            get { return _searchToken; }
            set
            {
                if (value != _searchToken)
                {
                    _searchToken = value;
                    OnPropertyChanged("SearchToken");
                    FilterInventory();
                }
            }
        }

        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public ExecutiveInventoryPages ParentPage { get; set; }
        public List<string> SOPRooms { get; set; }
        private InventoryController _inventoryController;
        public InventoryController InventoryController { get { return _inventoryController; } }
        private InventoryMovingController _inventoryMovingController;
        public InventoryMovingController InventoryMovingController { get { return _inventoryMovingController; } }
        private RoomController _roomController;
        public RoomController RoomController { get { return _roomController; } }

        public InventoryFilter InventoryFilter { get; set; }
        public OpenNewStaticPage OpenNewStaticPage { get; set; }
        public OpenMoveStaticPage OpenMoveStaticPage { get; set; }
        public ConfirmProblem ConfirmProblem { get; set; }

        public InventoryViewModel(ExecutiveInventoryPages parentPage)
        {
            this.ParentPage = parentPage;
            var app = Application.Current as App;
            _inventoryController = app.InventoryController;
            _inventoryMovingController = app.InventoryMovingController;
            _roomController = app.RoomController;
            this.SOPRooms = new List<string>();
            this.Feedback = "";
            this.WrongSelection = "";
            this.InventorySource = _inventoryController.GetPreviews();
            this.Inventory = new ObservableCollection<InventoryPreview>();
            this.SearchToken = "";
            this.InventoryFilter = new InventoryFilter(this);
            this.OpenNewStaticPage = new OpenNewStaticPage(this);
            this.OpenMoveStaticPage = new OpenMoveStaticPage(this);
            this.ConfirmProblem = new ConfirmProblem(this);
            FilterInventory();
        }
        public void FilterInventory()
        {
            Inventory.Clear();
            foreach (InventoryPreview p in InventorySource)
            {
                if ((p.Type.Equals("D") && ParentPage.DynamicCB.IsChecked == true) || (p.Type.Equals("S") && ParentPage.StaticCB.IsChecked == true))
                {
                    if (SearchToken == "" || (p.Name.ToLower()).Contains(SearchToken.ToLower()))
                        Inventory.Add(p);
                }
            }
            ParentPage.RefreshDG.Begin();
        }
        public void OpenNewInventoryPage()
        {
            ParentPage.FormFrame.Content = new NewInventory(ParentPage);
            ParentPage.OpenFrame.Begin();
        }

        public void OpenMoveInventoryPage()
        {
            if (ParentPage.InventoryDG.SelectedItems.Count == 0)
            {
                WrongSelection = "You must select inventory for moving first!";
                ParentPage.WrongSelectionContainer.Visibility = Visibility.Visible;
                return;
            }
            InventoryPreview i = (InventoryPreview)ParentPage.InventoryDG.SelectedItems[0];
            if (i.Type.Equals("D"))
            {
                WrongSelection = "You can only move static inventory!";
                ParentPage.WrongSelectionContainer.Visibility = Visibility.Visible;
                return;
            }
            var app = Application.Current as App;
            app.Properties["CurrentRoomOfInventory"] =  i.Room;
            app.Properties["NameOfInventory"] = i.Name;
            app.Properties["IdOfInventory"] = i.Id;
            ParentPage.FormFrame.Content = new MoveInventory(ParentPage);
            ParentPage.OpenFrame.Begin();
        }
        public void ConfirmProblemF()
        {
            ParentPage.WrongSelectionContainer.Visibility = Visibility.Collapsed;
        }
        public void CloseFrameF()
        {
            ParentPage.CloseFrame.Begin();
        }
    }
}
