using System;
using System.Collections.Generic;
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
    public partial class ExecutiveRoomPages : Page
    {
        private RoomController _roomController;
        public List<Room> Rooms { get; set; }
        public List<String> RoomTypes { get; set; }
        public String Feedback { get; set; }
        public int SelectedId { get; set; }
        public String SelectedNametag { get; set; }


        public ExecutiveRoomPages()
        {
            InitializeComponent();
            var app = Application.Current as App;
            _roomController = app.RoomController;
            this.Rooms = _roomController.GetAll();
            this.RoomTypes = new List<String>() { "Storage", "Operating", "Office", "Meeting"};
            this.DataContext = this;
            Feedback = "";
            SelectedNametag = "";
            SelectedId = 0;

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
            foreach(Room room in Rooms)
            {
                if(room.Nametag == AddNametag.Text)
                {
                    Feedback = "*Selected Nametag is already in use!";
                    return;
                }
            }

            Feedback = "";
            AddNametag.Text = "";
            AddType.Text = "";
            _roomController.Create(new Room(0, AddNametag.Text, AddType.Text));
            DialogContainer.Visibility = Visibility.Collapsed;
            AddContainer.Visibility = Visibility.Collapsed;


        }

        //--------------------------------------------------------------------------------------------------------
        //          Room Editing code:
        //--------------------------------------------------------------------------------------------------------

        private void EditRoomButton_Click(object sender, RoutedEventArgs e)
        {
            if(RoomsDG.SelectedItems.Count == 0)
            {
                Console.WriteLine("Nije selektovano!");
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
        }
    }
}
