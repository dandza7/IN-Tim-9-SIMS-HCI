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
        private RenovationController _renovationController;
        public List<Room> Rooms { get; set; }
        public List<String> RoomTypes { get; set; }
        public List<String> Beginnings { get; set; }
        public List<String> Endings { get; set; }
        public int SelectedId { get; set; }
        public String SelectedNametag { get; set; }

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
            SelectedId = 0;

        }

        //--------------------------------------------------------------------------------------------------------
        //          Global methods code:
        //--------------------------------------------------------------------------------------------------------

        public void RefreshSource()
        {
            this.Rooms = this._roomController.GetAll();
            RoomsDG.ItemsSource = Rooms;
            RoomsDG.Items.Refresh();
        }

        private void NotSelectedOK_Click(object sender, RoutedEventArgs e)
        {
            NotSelectedContainer.Visibility = Visibility.Collapsed;
            DialogContainer.Visibility = Visibility.Collapsed;
        }

        //--------------------------------------------------------------------------------------------------------
        //          Room Adding code:
        //--------------------------------------------------------------------------------------------------------

        private void AddRoomButton_Click(object sender, RoutedEventArgs e)
        {
            DialogContainer.Visibility = Visibility.Visible;
            AddContainer.Visibility = Visibility.Visible;
            Feedback = "";
            AddNametag.Text = "";
            AddType.Text = "";
        }
        private void XAddButton_Click(object sender, RoutedEventArgs e)
        {
            DialogContainer.Visibility = Visibility.Collapsed;
            AddContainer.Visibility = Visibility.Collapsed;
            Feedback = "";
            AddNametag.Text = "";
            AddType.Text = "";
        }
        private void AddConfirm_Click(object sender, RoutedEventArgs e)
        {
            if(AddNametag.Text == "" || AddType.Text == "")
            {
                
                Feedback = "*You have to fill all fields!";
                return;
            }
            if(AddNametag.Text.Contains(";"))
            {
                Feedback = "*You can't use semicolon (;) in Nametag!";
                return;
            }
            foreach(Room room in Rooms)
            {
                if(room.Nametag == AddNametag.Text)
                {
                    Feedback = "*Selected Nametag is already in use!";
                    return;
                }
            }


            _roomController.Create(new Room(0, AddNametag.Text, AddType.Text));
            DialogContainer.Visibility = Visibility.Collapsed;
            AddContainer.Visibility = Visibility.Collapsed;
            RefreshSource();
            Feedback = "";
            AddNametag.Text = "";
            AddType.Text = "";


        }

        //--------------------------------------------------------------------------------------------------------
        //          Room Editing code:
        //--------------------------------------------------------------------------------------------------------

        private void EditRoomButton_Click(object sender, RoutedEventArgs e)
        {
            if(RoomsDG.SelectedItems.Count == 0)
            {
                NotSelectedContainer.Visibility = Visibility.Visible;
                DialogContainer.Visibility = Visibility.Visible;
                return;
            }
            Feedback = "";
            EditType.Text = "";
            DialogContainer.Visibility = Visibility.Visible;
            EditContainer.Visibility = Visibility.Visible;
            Room r = (Room)RoomsDG.SelectedItems[0];
            SelectedId = r.Id;
            SelectedNametag = r.Nametag;
            EditNametag.Text = r.Nametag;
        }

        private void XEditButton_Click(object sender, RoutedEventArgs e)
        {
            Feedback = "";
            SelectedNametag = "";
            EditType.Text = "";
            DialogContainer.Visibility = Visibility.Collapsed;
            EditContainer.Visibility = Visibility.Collapsed;
        }

        private void EditConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (EditType.Text == "")
            {
                Feedback = "*You have to select new type of room!";
                return;
            }
            _roomController.Update(new Room(SelectedId, SelectedNametag, EditType.Text));
            Feedback = "";
            SelectedNametag = "";
            EditType.Text = "";
            DialogContainer.Visibility = Visibility.Collapsed;
            EditContainer.Visibility = Visibility.Collapsed;
            RefreshSource();
        }





        //--------------------------------------------------------------------------------------------------------
        //          Room Deleting code:
        //--------------------------------------------------------------------------------------------------------

        private void DeleteRoomButton_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsDG.SelectedItems.Count == 0)
            {
                NotSelectedContainer.Visibility = Visibility.Visible;
                DialogContainer.Visibility = Visibility.Visible;
                return;
            }
            Room r = (Room)RoomsDG.SelectedItems[0];
            SelectedId = r.Id;
            DeleteNametag.Text = r.Nametag;
            DialogContainer.Visibility = Visibility.Visible;
            DeleteContainer.Visibility = Visibility.Visible;
            
        }
        private void XDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DialogContainer.Visibility = Visibility.Collapsed;
            DeleteContainer.Visibility = Visibility.Collapsed;
        }

        private void DeleteConfirm_Click(object sender, RoutedEventArgs e)
        {
            this._roomController.Delete(SelectedId);
            RefreshSource();
            DialogContainer.Visibility = Visibility.Collapsed;
            DeleteContainer.Visibility = Visibility.Collapsed;
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
                NotSelectedContainer.Visibility = Visibility.Visible;
                DialogContainer.Visibility = Visibility.Visible;
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


    }
}
