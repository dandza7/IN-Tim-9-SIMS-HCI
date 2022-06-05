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
    internal class MoveInventoryViewModel : INotifyPropertyChanged
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
        public String _date;
        public string Date
        {
            get
            {
                return _date;
            }
            set
            {
                if (value != _date)
                {
                    _date = value;
                    OnPropertyChanged("Date");
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
        public ConfirmMoving ConfirmMoving { get; set; }
        public string CurrentRoom { get; set; }
        public string InventoryName { get; set; }
        public MoveInventoryViewModel(ExecutiveInventoryPages parent)
        {
            var app = Application.Current as App;
            SOPRooms = app.InventoryController.GetSOPRooms();
            this.ParentPage = parent;
            this.ParentsDataContext = (InventoryViewModel)parent.DataContext;
            this.CloseFrame = new CloseFrame(ParentsDataContext);
            this.ConfirmMoving = new ConfirmMoving(this);
            CurrentRoom = (string)app.Properties["CurrentRoomOfInventory"]; 
            InventoryName = (string)app.Properties["NameOfInventory"];
            Feedback = "";
        }
        public void ConfirmMovingF()
        {
            if (NewRoom == "" || Date == "")
            {
                Feedback = "*you must fill all fields!";
                return;
            }
            if (DateTime.Compare(DateTime.Parse(Date), DateTime.Today) < 0)
            {
                Feedback = "*you must select date that is either today or in future!";
                return;
            }
            var app = Application.Current as App;
            app.InventoryMovingController.NewMoving(new InventoryMoving(0, (int)app.Properties["IdOfInventory"], app.RoomController.GetByNametag(NewRoom).Id, DateTime.Parse(Date)));
            Feedback = "";
            ParentsDataContext.InventorySource = app.InventoryController.GetPreviews();
            ParentsDataContext.FilterInventory();
            ParentsDataContext.CloseFrameF();
        }
    }
}
