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
        public Storyboard LogOut { get; set; }
        private bool isOpen;
        public ExecutiveMenu()
        {
            InitializeComponent();
            ExecutiveMainFrame.Content = new ExecutiveMainPage();
            this.DataContext = this;
            this.Open = FindResource("Open") as Storyboard;
            this.Close = FindResource("Close") as Storyboard;
            this.LogOut = FindResource("SlowLogout") as Storyboard;
            this.isOpen = false;

        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            LogOut.Begin();
        }

        private void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Open_Completed(object sender, EventArgs e)
        {
        }

        private void Close_Completed(object sender, EventArgs e)
        {
        }

        private void SlowLogout_Completed(object sender, EventArgs e)
        {
            var app = Application.Current as App;
            app.Properties["userId"] = 0;
            app.Properties["userRole"] = "loggedOut";
            var s = new MainWindow();
            s.Show();
            Close();
        }

        private void ButtonContainer_MouseEnter(object sender, MouseEventArgs e)
        {
            Open.Begin();
        }

        private void ButtonContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            Close.Begin();
        }
    }
}
