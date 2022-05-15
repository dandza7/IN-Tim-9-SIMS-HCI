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
using WpfApp1.Model;

namespace WpfApp1.View.Model.Executive.ExecutiveRoomDialogs
{
    /// <summary>
    /// Interaction logic for AdvancedRenovation.xaml
    /// </summary>
    public partial class AdvancedRenovation : Page, INotifyPropertyChanged
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
        private List<string> _endings;
        public List<string> Endings
        {
            get
            {
                return _endings;
            }
            set
            {
                if (value != _endings)
                {
                    _endings = value;
                    OnPropertyChanged("Endings");
                }
            }
        }
        private ObservableCollection<Room> _tRooms;
        public ObservableCollection<Room> TRooms
        {
            get
            {
                return _tRooms;
            }
            set
            {
                if (value != _tRooms)
                {
                    _tRooms = value;
                    OnPropertyChanged("TRooms");
                }
            }
        }
        private ObservableCollection<Room> _cRooms;
        public ObservableCollection<Room> CRooms
        {
            get
            {
                return _cRooms;
            }
            set
            {
                if (value != _cRooms)
                {
                    _cRooms = value;
                    OnPropertyChanged("CRooms");
                }
            }
        }
        private ObservableCollection<string> _rooms;
        public ObservableCollection<string> Rooms
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

        public ExecutiveRoomPages ParentPage;
        public List<string> Beginnings { get; set; }
        public List<string> NewNametags { get; set; }
        public List<string> Types { get; set; }


        public AdvancedRenovation(ExecutiveRoomPages parent)
        {
            InitializeComponent();
            this.ParentPage = parent;
            this.DataContext = this;
            Beginnings = ParentPage.Beginnings;
            TRooms = new ObservableCollection<Room>();
            CRooms = new ObservableCollection<Room>();
            Rooms = new ObservableCollection<string>(ParentPage.RoomController.GetEditableNametags());
            NewNametags = new List<string>();
            Types = new List<string>() { "Operating", "Meeting", "Office", "Storage"};
            Rooms.Remove(ParentPage.SelectedNametag);
            this.TRooms.Add(ParentPage.RoomController.GetById(ParentPage.SelectedId));
        }

        private void FindPotentialEndings(string beginning)
        {
            if (beginning.Equals(""))
            {
                return;
            }
            this.Endings = ParentPage.RenovationController.GetEndings(beginning, ParentPage.SelectedId);
            Ending.IsEnabled = true;

        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ParentPage.CloseFrame.Begin();
            
        }

        private void AddRoom_Click(object sender, RoutedEventArgs e)
        {
            TRooms.Add(ParentPage.RoomController.GetByNametag(OldRooms.Text));
            Rooms.Remove(OldRooms.Text);
            Feedback = "";
        }
        
        private void RemoveFromTRoomsButton_Click(object sender, RoutedEventArgs e)
        {
            Room r = (Room)TargetedRooms.SelectedItems[0];
            TRooms.Remove(r);
            Rooms.Add(r.Nametag);
            if (NewNametags.Contains(r.Nametag))
            {
                Room dr = null;
                foreach (Room room in CRooms)
                {
                    if (room.Nametag.Equals(r.Nametag))
                    {
                        dr = room;
                    }
                }
                if (dr != null)
                {
                    CRooms.Remove(dr);
                    Feedback = "Room is removed from New Room list because its nametag is used in Old Room you just removed from renovation!";
                }
                else
                {
                    Feedback = "";
                }
            }
            else
            {
                Feedback = "";
            }
        }
        private void RemoveFromCRoomsButton_Click(object sender, RoutedEventArgs e)
        {
            Room r = (Room)CreatedRooms.SelectedItems[0];
            CRooms.Remove(r);
            NewNametags.Remove(r.Nametag);
            Feedback = "";
        }

        private void AddNewRoom_Click(object sender, RoutedEventArgs e)
        {
            if (Rooms.Contains(NewNametag.Text) || NewNametags.Contains(NewNametag.Text))
            {
                Feedback = "Nametag of new room is already in use!";
                return;
            }
            Room nr = new Room(0, NewNametag.Text, NewType.Text, false);
            CRooms.Add(nr);
            NewNametags.Add(nr.Nametag);
            Feedback = "";
        }
    }
}
