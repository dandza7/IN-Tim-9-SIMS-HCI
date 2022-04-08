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
        private RoomController _roomController;

        public event PropertyChangedEventHandler PropertyChanged;
        public List<Room> Rooms {get; set;}

        public ExecutiveMenu()
        {
            InitializeComponent();
            var app = Application.Current as App;
            _roomController = app.RoomController;
            this.Rooms = _roomController.GetAll();
        }
        //----------------------------------------------------------------------------------------------------------------
        //              LISTANJE PROSTORIJA
        //----------------------------------------------------------------------------------------------------------------

        private void ListButton_Click(object sender, RoutedEventArgs e)
        {
            EditContainer.Visibility = Visibility.Collapsed;
            AddContainer.Visibility = Visibility.Collapsed;
            ListContainer.Visibility = Visibility.Visible;
            DeleteContainer.Visibility = Visibility.Collapsed;

            ListButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#0082F0");
            AddButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
            EditButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
            DeleteButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");

            this.Rooms = _roomController.GetAll();
            this.DataContext = this;
        }

        private void XListButton_Click(object sender, RoutedEventArgs e)
        {
            ListContainer.Visibility = Visibility.Collapsed;
            ListButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
        }


        //----------------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------------------
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
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

        }

        private void XEditButton_Click(object sender, RoutedEventArgs e)
        {
            EditContainer.Visibility = Visibility.Collapsed;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditContainer.Visibility = Visibility.Visible;
            ListContainer.Visibility = Visibility.Collapsed;
            AddContainer.Visibility = Visibility.Collapsed;
            DeleteContainer.Visibility = Visibility.Collapsed;

            ListButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
            AddButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
            EditButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#0082F0");
            DeleteButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
        }

        private void XDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteContainer.Visibility = Visibility.Collapsed;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            EditContainer.Visibility = Visibility.Collapsed;
            ListContainer.Visibility = Visibility.Collapsed;
            AddContainer.Visibility = Visibility.Collapsed;
            DeleteContainer.Visibility = Visibility.Visible;

            ListButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
            AddButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
            EditButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#88C6FC");
            DeleteButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#0082F0");
        }
    }
}
