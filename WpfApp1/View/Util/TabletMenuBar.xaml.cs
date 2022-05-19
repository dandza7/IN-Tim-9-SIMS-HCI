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
using WpfApp1.View.Dialog.PatientDialog;
using WpfApp1.View.Model.Patient;

namespace WpfApp1.View.Util
{
    /// <summary>
    /// Interaction logic for TabletMenuBar.xaml
    /// </summary>
    public partial class TabletMenuBar : UserControl
    {
        private SurveyController _surveyController;
        public TabletMenuBar()
        {
            InitializeComponent();
        }

        private void ShowAppointments_Click(object sender, RoutedEventArgs e)
        {
            PatientAppointments.Background = (Brush)(new BrushConverter().ConvertFrom("#0082F0"));
            MyProfile.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            PatientNotes.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            HospitalSurvey.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            var app = Application.Current as App;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientAppointmentsView();
            
        }

        private void MyProfile_Click(object sender, RoutedEventArgs e)
        {
            PatientAppointments.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            MyProfile.Background = (Brush)(new BrushConverter().ConvertFrom("#0082F0"));
            PatientNotes.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            HospitalSurvey.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            var app = Application.Current as App;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientProfileView();
        }

        private void PatientNotes_Click(object sender, RoutedEventArgs e)
        {
            PatientAppointments.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            MyProfile.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            PatientNotes.Background = (Brush)(new BrushConverter().ConvertFrom("#0082F0"));
            HospitalSurvey.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            var app = Application.Current as App;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientNotesView();
        }

        private void HospitalSurvey_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _surveyController = app.SurveyController;

            app.Properties["appointmentId"] = -1;
            int patientId = (int)app.Properties["userId"];

            if(_surveyController.IsAlreadyGraded(patientId, -1))
            {
                PatientErrorMessageBox.Show("You have already graded our hospital!");
                return;
            }

            PatientAppointments.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            MyProfile.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            PatientNotes.Background = (Brush)(new BrushConverter().ConvertFrom("#199EF3"));
            HospitalSurvey.Background = (Brush)(new BrushConverter().ConvertFrom("#0082F0"));
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new HospitalSurveyDialog();
        }
    }
}
