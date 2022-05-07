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
        public String _selectedBeginning;
        public string SelectedBeginning
        {
            get
            {
                return _selectedBeginning;
            }
            set
            {
                if (value != _selectedBeginning)
                {
                    _selectedBeginning = value;
                    OnPropertyChanged("SelectedBeginning");
                    FindPotentialEndings(SelectedBeginning);
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
            Feedback = "";
            SelectedBeginning = "";
            SelectedNametag = "";
            SelectedType = "";
            SelectedType = "";
            SelectedId = 0;

        }

        //--------------------------------------------------------------------------------------------------------
        //          Global methods code:
        //--------------------------------------------------------------------------------------------------------
        public void ResetFields()
        {
            Feedback = "";
            SelectedBeginning = "";
            SelectedNametag = "";
            SelectedType = "";
            SelectedType = "";
            SelectedId = 0;
        }
        public void RefreshSource()
        {
            this.Rooms = this._roomController.GetAll();
            RoomsDG.ItemsSource = Rooms;
            RoomsDG.Items.Refresh();
        }


        //--------------------------------------------------------------------------------------------------------
        //          Room Adding code:
        //--------------------------------------------------------------------------------------------------------

        private void AddRoomButton_Click(object sender, RoutedEventArgs e)
        {
            FormFrame.Content = new NewRoom(this);
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
            DeleteRoomButton.Visibility = Visibility.Collapsed;
            DeleteConfirmButton.Visibility = Visibility.Visible;
            
        }

        private void DeleteConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            this.RoomController.Delete(SelectedId);
            this.Rooms = this.RoomController.GetAll();
            ResetFields();
            DeleteRoomButton.Visibility = Visibility.Visible;
            DeleteConfirmButton.Visibility = Visibility.Collapsed;


        }
        private void DeleteConfirmButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ResetFields();
            DeleteRoomButton.Visibility = Visibility.Visible;
            DeleteConfirmButton.Visibility = Visibility.Collapsed;
        }

        //--------------------------------------------------------------------------------------------------------
        //          Renovating button manipulation code:
        //--------------------------------------------------------------------------------------------------------

        private void RenovationButton_MouseEnter(object sender, MouseEventArgs e)
        {
            BasicRenovationButton.Visibility = Visibility.Visible;
            AdvancedRenovationButton.Visibility = Visibility.Visible;
        }


        private void BARenovationButton_MouseLeave(object sender, MouseEventArgs e)
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
            ResetFieldsBR();
            BREnding.IsEnabled = false;
            BRBeginning.IsEnabled = false;
            DialogContainer.Visibility = Visibility.Visible;
            BasicRenovationContainer.Visibility = Visibility.Visible;
            Room r = (Room)RoomsDG.SelectedItems[0];
            SelectedId = r.Id;
            SelectedNametag = r.Nametag;
            BRNametag.Text = r.Nametag;
            BRType.Text = r.Type;
            this.Beginnings = _renovationController.GetBegginigns(SelectedId);
            if(Beginnings.Count == 0)
            {
                Feedback = "*there are no free days for renovation for this room in next 14 days!";
            }
            else
            {
                BRBeginning.ItemsSource = Beginnings;
                BRBeginning.Items.Refresh();
                BRBeginning.IsEnabled = true;
            }
            
            
        }
        private void FindPotentialEndings(string beginning)
        {
            if (beginning.Equals(""))
            {
                return;
            }
            this.Endings = _renovationController.GetEndings(beginning, SelectedId);
            BREnding.ItemsSource = Endings;
            BREnding.Items.Refresh();
            BREnding.IsEnabled = true;
            
        }

        private void BRConfirm_Click(object sender, RoutedEventArgs e)
        {
            if(BRBeginning.Text.Equals("") || BREnding.Text.Equals("") || BRDescription.Text.Equals(""))
            {
                Feedback = "*you must fill all fields!";
                return;
            }
            if (BRDescription.Text.Contains(";"))
            {
                Feedback = "*you can't put semicolon (;) in description!";
                return;
            }
            _renovationController.Create(new Renovation(0, SelectedId, BRDescription.Text, DateTime.Parse(BRBeginning.Text), DateTime.Parse(BREnding.Text)));

            DialogContainer.Visibility = Visibility.Collapsed;
            BasicRenovationContainer.Visibility = Visibility.Collapsed;
            ResetFieldsBR();
        }

        private void XBRButton_Click(object sender, RoutedEventArgs e)
        {
            DialogContainer.Visibility = Visibility.Collapsed;
            BasicRenovationContainer.Visibility = Visibility.Collapsed;
            ResetFieldsBR();
        }
        private void ResetFieldsBR()
        {
            Feedback = "";
            BRNametag.Text = "";
            BRType.Text = "";
            BRDescription.Text = "";
            BREnding.Text = "";
            BRBeginning.Text = "";
        }
        //--------------------------------------------------------------------------------------------------------
        //          Advanced Room Renovating code:
        //--------------------------------------------------------------------------------------------------------
        private void AdvancedRenovationButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void WrongSelectionOK_Click(object sender, RoutedEventArgs e)
        {
            WrongSelectionContainer.Visibility = Visibility.Collapsed;
        }


    }
}
