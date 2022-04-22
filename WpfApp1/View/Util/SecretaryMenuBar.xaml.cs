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
            PatientsColorMark.Background = Brushes.LimeGreen;
            AppointmentsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF0C382E");
            Main.Content = new SecretaryPatientsView();
        }
        private void SecretaryAppointments_Click(object sender, RoutedEventArgs e)
        {
            PageHeader.Text = "Appointment List";
            AppointmentsColorMark.Background = Brushes.LimeGreen;
            PatientsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF0C382E");
            Main.Content = new SecretaryAppointmentsView();
        }
    }
}
