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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.View.Model.Doctor;
using WpfApp1.Service;

namespace WpfApp1.View
{
    /// <summary>
    /// Interaction logic for DoctorMenu.xaml
    /// </summary>
    public partial class DoctorMenu : Window
    {
        public DoctorMenu()
        {
            InitializeComponent();
            DoctorDisplayFrame.Content = new DoctorProfilePage();
            this.DataContext = this;
        }

        private void ProfileBT_Click(object sender, RoutedEventArgs e)
        {
            DoctorDisplayFrame.Content = new DoctorProfilePage();
        }

        private void AppointmentsBT_Click(object sender, RoutedEventArgs e)
        {
            DoctorDisplayFrame.Content = new DoctorAppointmentsPage();
        }

        private void MedicalRecordsBT_Click(object sender, RoutedEventArgs e)
        {

            DoctorDisplayFrame.Content = new DoctorMedicalRecordsPage();
        }

        private void RequestsBT_Click(object sender, RoutedEventArgs e)
        {

            DoctorDisplayFrame.Content = new DoctorRequestsPage();
        }

        private void NotificationsBT_Click(object sender, RoutedEventArgs e)
        {

            DoctorDisplayFrame.Content = new DoctorNotificationsPage();
        }

        private void DrugValidationBT_Click(object sender, RoutedEventArgs e)
        {

            DoctorDisplayFrame.Content = new DoctorDrugValidationPage();
        }

        private void LogOutBT_Click(object sender, RoutedEventArgs e)
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
