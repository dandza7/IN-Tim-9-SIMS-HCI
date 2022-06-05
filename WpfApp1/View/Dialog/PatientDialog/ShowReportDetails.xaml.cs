using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WpfApp1.Model;
using WpfApp1.View.Converter;
using WpfApp1.View.Model.Patient;

namespace WpfApp1.View.Dialog.PatientDialog
{
    /// <summary>
    /// Interaction logic for ShowReportDetails.xaml
    /// </summary>
    public partial class ShowReportDetails : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private ReportDetailsView _reportDetails;
        private DoctorsReportController _doctorsReportController;
        private DoctorController _doctorController;
        private AppointmentController _appointmentController;
        private UserController _userController;
        private RoomController _roomController;
        private SurveyController _surveyController;

        public ShowReportDetails()
        {
            InitializeComponent();
            DataContext = this;

            var app = Application.Current as App;
            _doctorsReportController = app.DoctorsReportController;
            _appointmentController = app.AppointmentController;
            _doctorController = app.DoctorController;
            _userController = app.UserController;
            _roomController = app.RoomController;

            int appointmentId = (int)app.Properties["appointmentId"];
            Appointment appointment = _appointmentController.GetById(appointmentId);
            Doctor doctor = _doctorController.GetById(appointment.DoctorId);
            User doctorUser = _userController.GetById(appointment.DoctorId);
            Room room = _roomController.GetById(doctor.RoomId);

            DoctorsReport doctorsReport = _doctorsReportController.GetByAppointmentId(appointmentId);
            ReportDetails = ReportConverter.ConvertAppointmentViewAndReportToReportDetailsView(appointment.Beginning, appointment.Ending, doctorUser.Username, room.Nametag, doctorsReport.Description);
        }

        public ReportDetailsView ReportDetails
        {
            get { return _reportDetails; }
            set
            {
                if (value != _reportDetails)
                {
                    _reportDetails = value;
                    OnPropertyChanged("ReportDetails");
                }
            }
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Grade_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;

            _surveyController = app.SurveyController;

            int appointmentId = (int)app.Properties["appointmentId"];
            int patientId = (int)app.Properties["userId"];

            if(_surveyController.IsAlreadyGraded(patientId, appointmentId))
            {
                PatientErrorMessageBox.Show("ERROR: You have already graded this appointment");
                return;
            }

            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new DoctorSurveyDialog();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            const string REPORT_DETAILS_HELP = "Here you can see the detailed review of the appointment." +
                "In doctors report you can find his diagnosis for the appointment. If you are willing to give us feedback about your " +
                "experience you are welcomed to do so by clicking on the button grade.";
            PatientHelp.Show(REPORT_DETAILS_HELP);
        }
    }
}
