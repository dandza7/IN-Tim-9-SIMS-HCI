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

namespace WpfApp1.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
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

    }
}
