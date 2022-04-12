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
using System.Windows.Shapes;
using WpfApp1.Controller;
using WpfApp1.Model;

namespace WpfApp1.Service
{
    /// <summary>
    /// Interaction logic for ExecutiveMenu.xaml
    /// </summary>
    public partial class ExecutiveMenu : Window, INotifyPropertyChanged
    {
        //----------------------------------------------------------------------------------------------------------------
        //              INotifyPropertyChanged Definions and Variables
        //----------------------------------------------------------------------------------------------------------------
        #region NotifyProperties
        private string _notification;
        public string Notification
        {
            get
            {
                return _notification;
            }
            set
            {
                if (value != _notification)
                {
                    _notification = value;
                    OnPropertyChanged("Notification");
                }
            }
        }

        #endregion
        #region PropertyChangedNotifier
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------------------
        //              Beginning of Room logic
        //----------------------------------------------------------------------------------------------------------------

        private RoomController _roomController;
        public List<Room> Rooms { get; set; }
        public List<String> Nametags { get; set; }
        public List<RoomType> TypesList { get; set;}


        public ExecutiveMenu()
        {
            InitializeComponent();
            var app = Application.Current as App;
            _roomController = app.RoomController;
            this.Rooms = _roomController.GetAll();
            var roomTypes = Enum.GetValues(typeof(RoomType));
            TypesList = roomTypes.OfType<RoomType>().ToList();
            this.Nametags = new List<String>();
            Notification = "InitalMessageThatIs KindaVeryLooooong";
            foreach (Room room in Rooms)
            {
                Nametags.Add(room.Nametag);
            }
            this.DataContext = this;

        }

        public void RefreshSource()
        {
            this.Rooms = _roomController.GetAll();
            DGList.ItemsSource = Rooms;
            DGList.Items.Refresh();
            
            this.Nametags = new List<String>();
            foreach (Room room in Rooms)
            {
                Nametags.Add(room.Nametag);
            }
            EditNametagComboBox.ItemsSource = Nametags;
            //EditNametagComboBox.Items.Refresh();
            DeleteNametagComboBox.ItemsSource = Nametags;
            //DeleteNametagComboBox.Items.Refresh();
        }
        //----------------------------------------------------------------------------------------------------------------
        //              Room Listing
        //----------------------------------------------------------------------------------------------------------------

        private void ListButton_Click(object sender, RoutedEventArgs e)
        {
            this.RefreshSource();

            EditContainer.Visibility = Visibility.Collapsed;
            AddContainer.Visibility = Visibility.Collapsed;
            ListContainer.Visibility = Visibility.Visible;
            DeleteContainer.Visibility = Visibility.Collapsed;

            ListButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#0082F0");
            AddButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
            EditButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
            DeleteButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");



        }

        private void XListButton_Click(object sender, RoutedEventArgs e)
        {
            ListContainer.Visibility = Visibility.Collapsed;
            ListButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
        }


        //----------------------------------------------------------------------------------------------------------------
        //              Room Adding
        //----------------------------------------------------------------------------------------------------------------
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            this.RefreshSource();
            AddNametagTextBox.Text = "";
            AddRoomTypeComboBox.Text = "";
            EditContainer.Visibility = Visibility.Collapsed;
            ListContainer.Visibility = Visibility.Collapsed;
            AddContainer.Visibility = Visibility.Visible;
            DeleteContainer.Visibility = Visibility.Collapsed;

            ListButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
            AddButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#0082F0");
            EditButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
            DeleteButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");


        }

        private void XAddButton_Click(object sender, RoutedEventArgs e)
        {
            AddContainer.Visibility = Visibility.Collapsed;
            AddButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");

        }

        private void AddConfirm_Click(object sender, RoutedEventArgs e)
        {
            if(AddNametagTextBox.Text.Equals("") || AddRoomTypeComboBox.Text.Equals("")){
                Notification = "All fields must be filled!";
                FeedbackContainer.Visibility = Visibility.Visible;
                return;
            }
            if (Nametags.Contains(AddNametagTextBox.Text))
            {
                Notification = "Room nametag you chose is already in use. \n Please pick another one...";
                FeedbackContainer.Visibility = Visibility.Visible;
                return;
            }
            Room room = new Room(0, AddNametagTextBox.Text, (RoomType)Enum.Parse(typeof(RoomType), AddRoomTypeComboBox.Text, true));
            _roomController.Create(room);
            AddContainer.Visibility = Visibility.Collapsed;
            AddButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
        }

        //----------------------------------------------------------------------------------------------------------------
        //              Room Editing
        //----------------------------------------------------------------------------------------------------------------

        private void XEditButton_Click(object sender, RoutedEventArgs e)
        {
            EditContainer.Visibility = Visibility.Collapsed;
            EditButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditNametagComboBox.Text = "";
            EditRoomTypeComboBox.Text = "";

            this.RefreshSource();
            EditContainer.Visibility = Visibility.Visible;
            ListContainer.Visibility = Visibility.Collapsed;
            AddContainer.Visibility = Visibility.Collapsed;
            DeleteContainer.Visibility = Visibility.Collapsed;

            ListButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
            AddButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
            EditButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#0082F0");
            DeleteButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
        }

        private void EditConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (EditNametagComboBox.Text.Equals("") || EditRoomTypeComboBox.Text.Equals(""))
            {
                Notification = "All fields must be filled!";
                FeedbackContainer.Visibility = Visibility.Visible;
                return;
            }
            int id = -1;
            foreach (Room r in Rooms)
            {
                if (r.Nametag.Equals(EditNametagComboBox.Text))
                {
                    id = r.Id;
                }
            }
            if (id == -1)
            {
                Notification = "Selected room doesn't exist anymore!\n Try refreshing the application.";
                FeedbackContainer.Visibility = Visibility.Visible;
                Console.WriteLine("Error: Selected room doesn't exist anymore!");
                return;
            }
            Room room = new Room(id, EditNametagComboBox.Text, (RoomType)Enum.Parse(typeof(RoomType), EditRoomTypeComboBox.Text, true));
            _roomController.Update(room);
            EditContainer.Visibility = Visibility.Collapsed;
            EditButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
        }

        //----------------------------------------------------------------------------------------------------------------
        //              Room Deleting
        //----------------------------------------------------------------------------------------------------------------

        private void XDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteContainer.Visibility = Visibility.Collapsed;
            DeleteButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteNametagComboBox.Text = "";
            this.RefreshSource();
            EditContainer.Visibility = Visibility.Collapsed;
            ListContainer.Visibility = Visibility.Collapsed;
            AddContainer.Visibility = Visibility.Collapsed;
            DeleteContainer.Visibility = Visibility.Visible;

            ListButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
            AddButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
            EditButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
            DeleteButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#0082F0");
        }

        private void DeleteConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (DeleteNametagComboBox.Text.Equals(""))
            {
                Notification = "You must select the room you want to delete!";
                FeedbackContainer.Visibility = Visibility.Visible;
                return;
            }
            int id = -1;
            foreach (Room r in Rooms)
            {
                if (r.Nametag.Equals(DeleteNametagComboBox.Text))
                {
                    id = r.Id;
                }
            }
            if (id == -1)
            {
                Notification = "Selected room doesn't exist anymore!\n Try refreshing the application.";
                FeedbackContainer.Visibility = Visibility.Visible;
                Console.WriteLine("Error: Selected room doesn't exist anymore!");
                return;
            }
            _roomController.Delete(id);
            DeleteContainer.Visibility = Visibility.Collapsed;
            DeleteButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
        }


        //----------------------------------------------------------------------------------------------------------------
        //              Feedback message
        //----------------------------------------------------------------------------------------------------------------
        private void OkFeedback_Click(object sender, RoutedEventArgs e)
        {
            FeedbackContainer.Visibility = Visibility.Collapsed;
        }
    }
}
