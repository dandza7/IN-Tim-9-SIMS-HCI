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
using WpfApp1.Controller;
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
        private UserController _userController;
        public SecretaryMenuBar()
        {
            InitializeComponent();
            var app = Application.Current as App;
            _userController = app.UserController;
            int loggedId = (int)app.Properties["userId"];
            Name.Text = _userController.GetById(loggedId).Name + " " + _userController.GetById(loggedId).Surname;
            Main.Content = new SecretaryDashboard();
            PageHeader.Text = "Dashboard";
        }

        private void SecretaryDashboard_Click(object sender, RoutedEventArgs e)
        {
            PageHeader.Text = "Dashboard";
            DashboardColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF7153C7");
            PatientsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            AppointmentsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            DynamicEquipmentColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            MeetingsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            LeaveRequestsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            NotificationsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            Main.Content = new SecretaryDashboard();
        }


        private void SecretaryPatients_Click(object sender, RoutedEventArgs e)
        {
            PageHeader.Text = "Patient List";
            DashboardColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            PatientsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF7153C7");
            AppointmentsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            DynamicEquipmentColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            MeetingsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            LeaveRequestsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            NotificationsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            Main.Content = new SecretaryPatientsView();
        }
        private void SecretaryAppointments_Click(object sender, RoutedEventArgs e)
        {
            PageHeader.Text = "Appointment List";
            DashboardColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            PatientsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            AppointmentsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF7153C7");
            DynamicEquipmentColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            MeetingsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            LeaveRequestsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            NotificationsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            Main.Content = new SecretaryAppointmentsView();
        }
        private void SecretaryDynamicEquipemnt_Click(object sender, RoutedEventArgs e)
        {
            PageHeader.Text = "Dynamic Equipment List";
            DashboardColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            PatientsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            AppointmentsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            DynamicEquipmentColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF7153C7");
            MeetingsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            LeaveRequestsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            NotificationsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            Main.Content = new SecretaryDynamicEquipmentView();
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

        private void SecretaryLeaveRequests_Click(object sender, RoutedEventArgs e)
        {
            PageHeader.Text = "Leave Request List";
            DashboardColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            PatientsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            AppointmentsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            DynamicEquipmentColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            MeetingsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            LeaveRequestsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF7153C7");
            NotificationsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            Main.Content = new SecretaryLeaveRequestsView();
        }

        private void SecretaryMeetingsClick(object sender, RoutedEventArgs e)
        {
            PageHeader.Text = "Meeting List";
            DashboardColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            PatientsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            AppointmentsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            DynamicEquipmentColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            MeetingsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF7153C7");
            LeaveRequestsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            NotificationsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            Main.Content = new SecretaryMeetingsView();
        }

        private void SecretaryNotifications_Click(object sender, RoutedEventArgs e)
        {
            PageHeader.Text = "Notification List";
            DashboardColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            PatientsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            AppointmentsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            DynamicEquipmentColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            MeetingsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            LeaveRequestsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF130A38");
            NotificationsColorMark.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF7153C7");
            Main.Content = new NotificationsView();
        }
    }
}
