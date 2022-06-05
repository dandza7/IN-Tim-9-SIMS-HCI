using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Model;
using WpfApp1.View.Model.Executive;
using WpfApp1.ViewModel.Commands.Executive;

namespace WpfApp1.ViewModel
{
    internal class NewInventoryViewModel : INotifyPropertyChanged
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
        public String _newName;
        public string NewName
        {
            get
            {
                return _newName;
            }
            set
            {
                if (value != _newName)
                {
                    _newName = value;
                    OnPropertyChanged("NewName");
                }
            }
        }
        public String _newRoom;
        public string NewRoom
        {
            get
            {
                return _newRoom;
            }
            set
            {
                if (value != _newRoom)
                {
                    _newRoom = value;
                    OnPropertyChanged("NewRoom");
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
        public InventoryViewModel ParentsDataContext { get; set; }
        public ExecutiveInventoryPages ParentPage { get; set; }
        public List<string> SOPRooms { get; set; }
        public CloseFrame CloseFrame { get; set; }
        public ConfirmAdding ConfirmAdding { get; set; }
        public NewInventoryViewModel(ExecutiveInventoryPages parent)
        {
            var app = Application.Current as App;
            SOPRooms = app.InventoryController.GetSOPRooms();
            this.ParentPage = parent;
            this.ParentsDataContext = (InventoryViewModel)parent.DataContext;
            this.CloseFrame = new CloseFrame(ParentsDataContext);
            this.ConfirmAdding = new ConfirmAdding(this);
            Feedback = "";
        }
        public void ConfirmAddingF()
        {
            if (NewRoom == "" || NewName == "")
            {
                Feedback = "*you must fill all fields!";
                return;
            }
            if (NewName.Contains(";"))
            {
                Feedback = "*you can't use semicolon (;) in name!";
                return;
            }
            var app = Application.Current as App;
            app.InventoryController.Create(new Inventory(0, 0, NewName, "S", 1), NewRoom);
            Feedback = "";
            ParentsDataContext.InventorySource = app.InventoryController.GetPreviews();
            ParentsDataContext.FilterInventory();
            ParentsDataContext.CloseFrameF();
        }
    }
}
