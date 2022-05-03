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
using WpfApp1.Service;
using WpfApp1.View.Model;
using WpfApp1.View.Model.Secretary;

namespace WpfApp1.View.Util
{
    /// <summary>
    /// Interaction logic for SecretaryMenuBar.xaml
    /// </summary>
    public partial class SecretaryMenuBar : UserControl
    {
        public SecretaryMenuBar()
        {
            InitializeComponent();
        }

        private void SecretaryDashboard_Click(object sender, RoutedEventArgs e)
        {
            PageHeader.Text = "Dashboard";
        }
        private void SecretaryPatients_Click(object sender, RoutedEventArgs e)
        {
            PageHeader.Text = "Patient List";
            PatientsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFD8117");
            AppointmentsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF101010");
            Main.Content = new SecretaryPatientsView();
        }
        private void SecretaryAppointments_Click(object sender, RoutedEventArgs e)
        {
            PageHeader.Text = "Appointment List";
            AppointmentsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFD8117");
            PatientsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF101010");
            Main.Content = new SecretaryAppointmentsView();
        }
        private void CloseAllWindows()
        {
            for (int intCounter = App.Current.Windows.Count - 1; intCounter > -1; intCounter--)
            {
                if (intCounter == 0)
                {
                    var s = new MainWindow();
                    s.Show();

                }
            }
            App.Current.Windows[0].Close();
        }
        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            app.Properties["userId"] = 0;
            app.Properties["userRole"] = "loggedOut";
            CloseAllWindows();
        }
    }
}
