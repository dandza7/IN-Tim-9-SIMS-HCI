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

namespace WpfApp1.View.Model.Executive.ExecutiveInventoryDialogs
{
    /// <summary>
    /// Interaction logic for MoveInventory.xaml
    /// </summary>
    public partial class MoveInventory : Page, INotifyPropertyChanged
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
        public ExecutiveInventoryPages ParentPage { get; set; }
        public List<string> SOPRooms { get; set; }
        public MoveInventory(ExecutiveInventoryPages parent)
        {
            InitializeComponent();
            this.DataContext = this;
            this.ParentPage = parent;
            this.SOPRooms = parent.InventoryController.GetSOPRooms();
            InventoryName.Text = ParentPage.SelectedInventoryName;
            OldRoom.Text = ParentPage.SelectedRoomName;
            Feedback = "";
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ParentPage.CloseFrame.Begin();
            InventoryName.Text = "";
            OldRoom.Text = "";
            NewRoom.Text = "";
            Feedback = "";
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewRoom.Text == "" || MoveDate.Text == "")
            {
                Feedback = "*you must fill all fields!";
                return;
            }
            if (DateTime.Compare(DateTime.Parse(MoveDate.Text), DateTime.Today) < 0)
            {
                Feedback = "*you must select date that is either today or in future!";
                return;
            }
            ParentPage.InventoryMovingController.NewMoving(new InventoryMoving(0, ParentPage.SelectedId, ParentPage.RoomController.GetIdByNametag(NewRoom.Text), DateTime.Parse(MoveDate.Text)));
            ParentPage.CloseFrame.Begin();
            InventoryName.Text = "";
            OldRoom.Text = "";
            NewRoom.Text = "";
            Feedback = "";
            ParentPage.Inventory = ParentPage.InventoryController.GetPreviews();
        }
    }
}
