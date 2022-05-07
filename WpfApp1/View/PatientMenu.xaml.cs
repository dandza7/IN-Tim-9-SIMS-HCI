using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using WpfApp1.View.Dialog;
using WpfApp1.View.Model;

namespace WpfApp1.Service
{
    /// <summary>
    /// Interaction logic for PatientMenu.xaml
    /// </summary>
    public partial class PatientMenu : Window
    {
        public PatientMenu()
        {
            InitializeComponent();
            var app = Application.Current as App;
            app.Properties["PatientMenu"] = this;
            app.Properties["PatientFrame"] = PatientFrame;
            app.Properties["PatientOverlay"] = Overlay;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            app.Properties["userId"] = 0;
            app.Properties["userRole"] = "loggedOut";

            var s = new MainWindow();
            s.Show();
            Close();
        }
    }
}
