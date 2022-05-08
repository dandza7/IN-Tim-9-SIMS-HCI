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
using System.Windows.Media.Animation;
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

        public Storyboard Open { get; set; }
        public Storyboard Close { get; set; }
        private bool isOpen;
        public ExecutiveMenu()
        {
            InitializeComponent();
            ExecutiveMainFrame.Content = new ExecutiveMainPage();
            this.DataContext = this;
            this.Open = FindResource("Open") as Storyboard;
            this.Close = FindResource("Close") as Storyboard;
            this.isOpen = false;
            ManipulationButton.Content = "<";
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

        private void ManipulationButton_Click(object sender, RoutedEventArgs e)
        {
            if (isOpen)
            {
                Close.Begin();
                isOpen = false;
            }
            else
            {
                Open.Begin();
                isOpen = true;
            }
            
        }

        private void Open_Completed(object sender, EventArgs e)
        {
            ManipulationButton.Content = ">";
        }

        private void Close_Completed(object sender, EventArgs e)
        {
            ManipulationButton.Content = "<";
        }
    }
}
