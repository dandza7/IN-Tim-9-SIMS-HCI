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
using WpfApp1.View;
using WpfApp1.View.Model;
using WpfApp1.Service;
using WpfApp1.Controller;
using WpfApp1.Model;

namespace WpfApp1.Service
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserController _userController;
        public string BadLoginText { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            var app = Application.Current as App;
            _userController = app.UserController;
            BadLoginText = "There was a problem with logging in, check if you typed your username and password correctly";
            this.DataContext = this;
        }

        private void ShowPatientOptions(object sender, RoutedEventArgs e)
        {
            var s = new PatientMenu();
            s.Show();
        }

        private void ShowExecutiveOptions(object sender, RoutedEventArgs e)
        {
            var s = new ExecutiveMenu();
            s.Show();
        }
        private void ShowDoctorOptions(object sender, RoutedEventArgs e)
        {
            var s = new DoctorMenu();
            s.Show();
        }
        private void ShowSecretaryOptions(object sender, RoutedEventArgs e)
        {
            var s = new SecretaryMenu();
            s.Show();
        }

        //---------------------------------------------------------------------------
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
            LogInConfirm.Visibility = Visibility.Collapsed;
            FakeLogInButton1.Visibility = Visibility.Visible;
        }

        private void XLogInButton_Click(object sender, RoutedEventArgs e)
        {
            LogInForm.Visibility = Visibility.Collapsed;
        }

        private void LogInConfirm_Click(object sender, RoutedEventArgs e)
        {
            string user = UsernameField.Text;
            string pw = PasswordField.Password.ToString();
            User logged = this._userController.CheckLogIn(user, pw);
            if(logged == null)
            {
                ErrorContainer.Visibility = Visibility.Visible;
                UsernameField.Text = "";
                PasswordField.Password = "";
                return;
            }
            var app = Application.Current as App;
            app.Properties["userId"] = logged.Id;
            app.Properties["userRole"] = logged.Role.ToString();
            if (logged.Role.ToString().Equals("executive"))
            {
                var s = new ExecutiveMenu();
                s.Show();
                Close();
            } 
            else if (logged.Role.ToString().Equals("secretary"))
            {
                var s = new SecretaryMenu();
                s.Show();
                Close();
            }
            else if (logged.Role.ToString().Equals("patient"))
            {
                var s = new PatientMenu();
                s.Show();
                Close();
            }
            else if (logged.Role.ToString().Equals("doctor"))
            {
                var s = new DoctorMenu();
                s.Show();
                Close();
            }
            Console.WriteLine("Logged in user with ID: {0} and ROLE: {1}", app.Properties["userId"], app.Properties["userRole"]);
        }

        private void OkToError_Click(object sender, RoutedEventArgs e)
        {
            ErrorContainer.Visibility = Visibility.Collapsed;
        }


        // JOKES ON YOU
        private void FakeLogInButton1_MouseEnter(object sender, MouseEventArgs e)
        {
            FakeLogInButton1.Visibility = Visibility.Collapsed;
            FakeLogInButton2.Visibility = Visibility.Visible;
        }
        private void FakeLogInButton2_MouseEnter(object sender, MouseEventArgs e)
        {
            FakeLogInButton2.Visibility = Visibility.Collapsed;
            FakeLogInButton3.Visibility = Visibility.Visible;
        }
        private void FakeLogInButton3_MouseEnter(object sender, MouseEventArgs e)
        {
            FakeLogInButton3.Visibility = Visibility.Collapsed;
            FakeLogInButton4.Visibility = Visibility.Visible;
        }
        private void FakeLogInButton4_MouseEnter(object sender, MouseEventArgs e)
        {
            FakeLogInButton4.Visibility = Visibility.Collapsed;
            FakeLogInButton5.Visibility = Visibility.Visible;
        }
        private void FakeLogInButton5_MouseEnter(object sender, MouseEventArgs e)
        {
            FakeLogInButton5.Visibility = Visibility.Collapsed;
            FakeLogInButton6.Visibility = Visibility.Visible;
        }
        private void FakeLogInButton6_MouseEnter(object sender, MouseEventArgs e)
        {
            FakeLogInButton6.Visibility = Visibility.Collapsed;
            FakeLogInButton7.Visibility = Visibility.Visible;
        }
        private void FakeLogInButton7_MouseEnter(object sender, MouseEventArgs e)
        {
            FakeLogInButton7.Visibility = Visibility.Collapsed;
            FakeLogInButton8.Visibility = Visibility.Visible;
        }
        private void FakeLogInButton8_MouseEnter(object sender, MouseEventArgs e)
        {
            FakeLogInButton8.Visibility = Visibility.Collapsed;
            FakeLogInButton9.Visibility = Visibility.Visible;
        }
        private void FakeLogInButton9_MouseEnter(object sender, MouseEventArgs e)
        {
            FakeLogInButton9.Visibility = Visibility.Collapsed;
            FakeLogInButton10.Visibility = Visibility.Visible;
        }
        private void FakeLogInButton10_MouseEnter(object sender, MouseEventArgs e)
        {
            FakeLogInButton10.Visibility = Visibility.Collapsed;
            LogInConfirm.Visibility = Visibility.Visible;
        }

    }
}
