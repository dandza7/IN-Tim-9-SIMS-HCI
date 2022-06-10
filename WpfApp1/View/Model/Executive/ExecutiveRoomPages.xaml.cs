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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.View.Model.Executive.ExecutiveRoomDialogs;

namespace WpfApp1.View.Model.Executive
{
    /// <summary>
    /// Interaction logic for ExecutiveRoomPages.xaml
    /// </summary>
    public partial class ExecutiveRoomPages : Page, INotifyPropertyChanged
    {
        //--------------------------------------------------------------------------------------------------------
        //          INotifyPropertyChanged fields:
        //--------------------------------------------------------------------------------------------------------
        #region NotifyProperties

        public String _selectionProblem;
        public string SelectionProblem
        {
            get
            {
                return _selectionProblem;
            }
            set
            {
                if (value != _selectionProblem)
                {
                    _selectionProblem = value;
                    OnPropertyChanged("SelectionProblem");
                }
            }
        }

        private List<Room> _rooms;
        public List<Room> Rooms
        {
            get
            {
                return _rooms;
            }
            set
            {
                if (value != _rooms)
                {
                    _rooms = value;
                    OnPropertyChanged("Rooms");
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

        private RoomController _roomController;
        public RoomController RoomController
        {
            get { return _roomController; }
        }
        private RenovationController _renovationController;
        public RenovationController RenovationController
        {
            get { return _renovationController; }
        }

        public List<String> RoomTypes { get; set; }
        public List<String> Beginnings { get; set; }
        public List<String> Endings { get; set; }
        public int SelectedId { get; set; }
        public String SelectedNametag { get; set; }
        public String SelectedType { get; set; }
        public Storyboard FrameAnimation { get; set; }
        public Storyboard CloseFrame { get; set; }
        public Storyboard HideDelete { get; set; }
        public Storyboard ShowDelete { get; set; }
        public Storyboard ShowRenovationPicker { get; set; }
        public Storyboard HideRenovationPicker { get; set; }


        //--------------------------------------------------------------------------------------------------------
        //          Constructor code:
        //--------------------------------------------------------------------------------------------------------

        public ExecutiveRoomPages()
        {
            InitializeComponent();
            var app = Application.Current as App;
            _roomController = app.RoomController;
            _renovationController = app.RenovationController;
            this.Rooms = _roomController.GetAll();
            this.RoomTypes = new List<String>() { "Storage", "Operating", "Office", "Meeting"};
            this.Beginnings = new List<String>();
            this.Endings = new List<String>();
            this.DataContext = this;
            SelectedNametag = "";
            SelectedType = "";
            SelectedId = 0;
            LoadAnimations();
            

        }
        public void LoadAnimations()
        {
            FrameAnimation = FindResource("FormFrameAnimation") as Storyboard;
            CloseFrame = FindResource("CloseFrame") as Storyboard;
            HideDelete = FindResource("DeleteButtonHide") as Storyboard;
            ShowDelete = FindResource("DeleteButtonShow") as Storyboard;
            ShowRenovationPicker = FindResource("ShowRenovationPicker") as Storyboard;
            HideRenovationPicker = FindResource("HideRenovationPicker") as Storyboard;

        }

        //--------------------------------------------------------------------------------------------------------
        //          Global methods code:
        //--------------------------------------------------------------------------------------------------------
        public void ResetFields()
        {
            SelectedNametag = "";
            SelectedType = "";
            SelectedType = "";
            SelectedId = 0;
        }


        //--------------------------------------------------------------------------------------------------------
        //          Room Adding code:
        //--------------------------------------------------------------------------------------------------------

        private void AddRoomButton_Click(object sender, RoutedEventArgs e)
        {
            FormFrame.Content = new NewRoom(this);
            FrameAnimation.Begin();
        }

        //--------------------------------------------------------------------------------------------------------
        //          Room Editing code:
        //--------------------------------------------------------------------------------------------------------

        private void EditRoomButton_Click(object sender, RoutedEventArgs e)
        {
            if(RoomsDG.SelectedItems.Count == 0)
            {
                SelectionProblem = "You have to select room for editing first!";
                WrongSelectionContainer.Visibility = Visibility.Visible;
                return;
            }
            Room r = (Room)RoomsDG.SelectedItems[0];
            SelectedId = r.Id;
            if (SelectedId == 1 || SelectedId == 2)
            {
                SelectionProblem = "You can't edit this room!";
                WrongSelectionContainer.Visibility = Visibility.Visible;
                return;
            }
            SelectedNametag = r.Nametag;
            SelectedType = r.Type;
            FormFrame.Content = new EditRoom(this);
            FrameAnimation.Begin();
        }






        //--------------------------------------------------------------------------------------------------------
        //          Room Deleting code:
        //--------------------------------------------------------------------------------------------------------

        private void DeleteRoomButton_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsDG.SelectedItems.Count == 0)
            {
                SelectionProblem = "You have to select room for deleting first!";
                WrongSelectionContainer.Visibility = Visibility.Visible;
                return;
            }
            Room r = (Room)RoomsDG.SelectedItems[0];
            SelectedId = r.Id;
            if (SelectedId == 1 || SelectedId == 2)
            {
                SelectionProblem = "You can't delete this room!";
                WrongSelectionContainer.Visibility = Visibility.Visible;
                return;
            }
            DeleteConfirmButton.Visibility = Visibility.Visible;
            HideDelete.Begin();
            
        }

        private void DeleteConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            this.RoomController.Delete(SelectedId);
            this.Rooms = this.RoomController.GetAll();
            ResetFields();
            DeleteRoomButton.Visibility = Visibility.Visible;
            ShowDelete.Begin();


        }
        private void DeleteConfirmButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ResetFields();
            DeleteRoomButton.Visibility = Visibility.Visible;
            ShowDelete.Begin();
        }
        private void DeleteButtonHide_Completed(object sender, EventArgs e)
        {
            DeleteRoomButton.Visibility = Visibility.Collapsed;
            DeleteRoomButton.Opacity = 1;
        }
        private void DeleteButtonShow_Completed(object sender, EventArgs e)
        {
            DeleteConfirmButton.Visibility = Visibility.Collapsed;
            DeleteRoomButton.Opacity = 1;
        }

        //--------------------------------------------------------------------------------------------------------
        //          Renovating button manipulation code:
        //--------------------------------------------------------------------------------------------------------

        private void RenovationButton_MouseEnter(object sender, MouseEventArgs e)
        {
            BasicRenovationButton.Visibility = Visibility.Visible;
            AdvancedRenovationButton.Visibility = Visibility.Visible;
            ShowRenovationPicker.Begin();

        }


        private void BARenovationButton_MouseLeave(object sender, MouseEventArgs e)
        {
            HideRenovationPicker.Begin();

        }
        private void ShowRenovationPicker_Completed(object sender, EventArgs e)
        {

        }

        private void HideRenovationPicker_Completed(object sender, EventArgs e)
        {
            BasicRenovationButton.Visibility = Visibility.Collapsed;
            AdvancedRenovationButton.Visibility = Visibility.Collapsed;
        }

        //--------------------------------------------------------------------------------------------------------
        //          Basic Room Renovating code:
        //--------------------------------------------------------------------------------------------------------
        private void BasicRenovationButton_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsDG.SelectedItems.Count == 0)
            {
                SelectionProblem = "You have to select room for renovation first!";
                WrongSelectionContainer.Visibility = Visibility.Visible;
                return;
            }
            Room r = (Room)RoomsDG.SelectedItems[0];
            SelectedId = r.Id;
            SelectedNametag = r.Nametag;
            SelectedType = r.Type;
            this.Beginnings = _renovationController.GetDaysAvailableForRenovation(new List<int>() { SelectedId });
            if(Beginnings.Count == 0)
            {
                SelectionProblem = "*there are no free days for renovation for this room in next 14 days!";
                WrongSelectionContainer.Visibility = Visibility.Visible;
                return;
            }
            FormFrame.Content = new BasicRenovation(this);
            FrameAnimation.Begin();

        }


        //--------------------------------------------------------------------------------------------------------
        //          Advanced Room Renovating code:
        //--------------------------------------------------------------------------------------------------------
        private void AdvancedRenovationButton_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsDG.SelectedItems.Count == 0)
            {
                SelectionProblem = "You have to select room for renovation first!";
                WrongSelectionContainer.Visibility = Visibility.Visible;
                return;
            }
            Room r = (Room)RoomsDG.SelectedItems[0];
            SelectedId = r.Id;
            SelectedNametag = r.Nametag;
            SelectedType = r.Type;
            if (SelectedId == 1 || SelectedId == 2)
            {
                SelectionProblem = "You can't do advanced renovation in this room!";
                WrongSelectionContainer.Visibility = Visibility.Visible;
                return;
            }
            this.Beginnings = _renovationController.GetDaysAvailableForRenovation(new List<int>() { SelectedId });
            if (Beginnings.Count == 0)
            {
                SelectionProblem = "*there are no free days for renovation for this room in next 14 days!";
                WrongSelectionContainer.Visibility = Visibility.Visible;
                return;
            }
            FormFrame.Content = new AdvancedRenovation(this);
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

        private void BusynessDetails_Click(object sender, RoutedEventArgs e)
        {
            FormFrame.Content = new ExportBusynessPDF(this);
            FrameAnimation.Begin();
        }
    }
}
