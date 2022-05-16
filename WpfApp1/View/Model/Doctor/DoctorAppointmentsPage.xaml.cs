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

namespace WpfApp1.View.Model.Doctor
{
    /// <summary>
    /// Interaction logic for DoctorAppointmentsPage.xaml
    /// </summary>
    public partial class DoctorAppointmentsPage : Page
    {
        public DoctorAppointmentsPage()
        {
            InitializeComponent();
            AppointmentFrame.Content = new Doctor_MyAppointments();
            MyAppointmentsBT.Background = MyAppointmentsBT.Foreground = this.Resources["MedicalGreen"] as Brush;
            RefferalsBT.BorderBrush = RefferalsBT.Foreground = this.Resources["White"] as Brush;
        }

        private void MyAppointmentsBT_Click(object sender, RoutedEventArgs e)
        {

            AppointmentFrame.Content = new Doctor_MyAppointments();
            MyAppointmentsBT.Background = MyAppointmentsBT.Foreground = this.Resources["MedicalGreen"] as Brush;
            RefferalsBT.BorderBrush = RefferalsBT.Foreground = this.Resources["White"] as Brush;
        }

        private void RefferalsBT_Click(object sender, RoutedEventArgs e)
        {

            AppointmentFrame.Content = new Doctor_Refferals();
            MyAppointmentsBT.Background = MyAppointmentsBT.Foreground = this.Resources["White"] as Brush;
            RefferalsBT.BorderBrush = RefferalsBT.Foreground = this.Resources["MedicalGreen"] as Brush;
        }

    }
}
