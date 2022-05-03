﻿using System;
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

    }
}
