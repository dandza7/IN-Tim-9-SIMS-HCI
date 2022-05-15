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


        public AdvancedRenovation(ExecutiveRoomPages parent)
        {
            InitializeComponent();
            this.ParentPage = parent;
            this.DataContext = this;
            Beginnings = ParentPage.Beginnings;
            TRooms = new ObservableCollection<Room>();
            Rooms = new ObservableCollection<string>(ParentPage.RoomController.GetEditableNametags());
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
        }
        
        private void RemoveFromTRoomsButton_Click(object sender, RoutedEventArgs e)
        {
            Room r = (Room)TargetedRooms.SelectedItems[0];
            TRooms.Remove(r);
            Rooms.Add(r.Nametag);
        }
    }
}
