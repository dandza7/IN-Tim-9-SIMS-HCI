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
using WpfApp1.View.Model.Executive;

namespace WpfApp1.Service
{
    /// <summary>
    /// Interaction logic for ExecutiveMenu.xaml
    /// </summary>
    public partial class ExecutiveMenu : Window
    {

        public string BadLoginText { get; set; }

        public ExecutiveMenu()
        {
            InitializeComponent();
            BadLoginText = "There was a problem with logging in, check if you typed your username and password correctly";
            this.DataContext = this;

        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            HelpLogIn.Visibility = Visibility.Visible;
        }
        private void XHelpButton_Click(object sender, RoutedEventArgs e)
        {
            HelpLogIn.Visibility = Visibility.Collapsed;
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            HelpLogIn.Visibility = Visibility.Collapsed;
            LogInForm.Visibility = Visibility.Visible;
        }

        private void XLogInButton_Click(object sender, RoutedEventArgs e)
        {
            LogInForm.Visibility = Visibility.Collapsed;
        }

        private void LogInConfirm_Click(object sender, RoutedEventArgs e)
        {
            string user = UsernameField.Text;
            string pw = PasswordField.Password.ToString();
            if (user != "dandza" || pw != "0904")
            {
                ErrorContainer.Visibility = Visibility.Visible;
            } else
            {
                UsernameField.Text = "";
                PasswordField.Clear();
                LogInContainer.Visibility = Visibility.Collapsed;
                ExecutiveMainFrame.Content = new ExecutiveMainPage();
                ExecutiveMainFrame.Visibility = Visibility.Visible;
                LogOutButton.Visibility = Visibility.Visible;
                NotificationsButton.Visibility = Visibility.Visible;
            }
        }

        private void OkToError_Click(object sender, RoutedEventArgs e)
        {
            ErrorContainer.Visibility=Visibility.Collapsed;
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            LogInContainer.Visibility = Visibility.Visible;
            ExecutiveMainFrame.Visibility = Visibility.Collapsed;
            LogOutButton.Visibility = Visibility.Collapsed;
            NotificationsButton.Visibility = Visibility.Collapsed;
        }

        private void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
