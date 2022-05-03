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
using WpfApp1.View.Model.Patient;

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
            PatientAppointments.Background = (Brush)(new BrushConverter().ConvertFrom("#0082F0"));
            MyProfile.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            PatientNotes.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            PatientReports.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            var app = Application.Current as App;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientAppointmentsView();
            
        }

        private void MyProfile_Click(object sender, RoutedEventArgs e)
        {
            PatientAppointments.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            MyProfile.Background = (Brush)(new BrushConverter().ConvertFrom("#0082F0"));
            PatientNotes.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            PatientReports.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            var app = Application.Current as App;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientProfileView();
        }

        private void PatientNotes_Click(object sender, RoutedEventArgs e)
        {
            PatientAppointments.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            MyProfile.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            PatientNotes.Background = (Brush)(new BrushConverter().ConvertFrom("#0082F0"));
            PatientReports.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            var app = Application.Current as App;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientNotesView();
        }

        private void PatientReports_Click(object sender, RoutedEventArgs e)
        {
            PatientAppointments.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            MyProfile.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            PatientNotes.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            PatientReports.Background = (Brush)(new BrushConverter().ConvertFrom("#0082F0"));
            var app = Application.Current as App;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientReportsView();
        }
    }
}
