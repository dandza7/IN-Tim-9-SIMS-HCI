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
                NotificationContainer.Visibility = Visibility.Visible;
            } else
            {
                LogInContainer.Visibility = Visibility.Collapsed;
            }
        }

        private void OKNotification_Click(object sender, RoutedEventArgs e)
        {
            NotificationContainer.Visibility=Visibility.Collapsed;
        }
    }
}
