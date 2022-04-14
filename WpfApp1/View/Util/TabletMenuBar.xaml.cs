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
using WpfApp1.View.Model;

namespace WpfApp1.View.Util
{
    /// <summary>
    /// Interaction logic for TabletMenuBar.xaml
    /// </summary>
    public partial class TabletMenuBar : UserControl
    {
        public TabletMenuBar()
        {
            InitializeComponent();
        }

        private void ShowAppointments_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientAppointmentsView();
        }
    }
}
