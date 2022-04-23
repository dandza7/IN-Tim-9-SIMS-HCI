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


        public ExecutiveMenu()
        {
            InitializeComponent();
            ExecutiveMainFrame.Content = new ExecutiveMainPage();
            this.DataContext = this;

        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            app.Properties["userId"] = 0;
            app.Properties["userRole"] = "loggedOut";
            var s = new MainWindow();
            s.Show();
            Close();
        }

        private void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
