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

namespace WpfApp1.View.Model.Executive.ExecutiveRoomDialogs
{
    /// <summary>
    /// Interaction logic for EditRoom.xaml
    /// </summary>
    public partial class EditRoom : Page, INotifyPropertyChanged
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
        public ExecutiveRoomPages ParentPage { get; set; }
        public List<string> RoomTypes { get; set; }
        public EditRoom(ExecutiveRoomPages parent)
        {
            InitializeComponent();
            this.ParentPage = parent;
            this.RoomTypes = new List<string>() { "Storage", "Operating", "Office", "Meeting" };
            this.DataContext = this;
            RoomType.SelectedValue = ParentPage.SelectedType;
            RoomNametag.Text = ParentPage.SelectedNametag;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (RoomNametag.Text == "" || RoomType.Text == "")
            {

                Feedback = "*You have to fill all fields!";
                return;
            }
            if (RoomNametag.Text.Contains(";"))
            {
                Feedback = "*You can't use semicolon (;) in Nametag!";
                return;
            }
            List<Room> Rooms = ParentPage.Rooms;
            foreach (Room room in Rooms)
            {
                if (room.Nametag == RoomNametag.Text && RoomNametag.Text != ParentPage.SelectedNametag)
                {
                    Feedback = "*Selected Nametag is already in use!";
                    return;
                }
            }


            ParentPage.RoomController.Update(new Room(ParentPage.SelectedId, RoomNametag.Text, RoomType.Text, true));
            ParentPage.Rooms = ParentPage.RoomController.GetAll();
            ParentPage.CloseFrame.Begin();
            RoomType.Text = "";
            RoomNametag.Text = "";
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ParentPage.CloseFrame.Begin();
            RoomType.Text = "";
            RoomNametag.Text = "";
        }
    }
}
