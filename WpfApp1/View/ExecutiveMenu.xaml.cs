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
using System.Windows.Shapes;

namespace WpfApp1.View
{
    /// <summary>
    /// Interaction logic for ExecutiveMenu.xaml
    /// </summary>
    public partial class ExecutiveMenu : Window
    {
        public ExecutiveMenu()
        {
            InitializeComponent();
        }

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
        }

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

        private void XListButton_Click(object sender, RoutedEventArgs e)
        {
            ListContainer.Visibility = Visibility.Collapsed;
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
