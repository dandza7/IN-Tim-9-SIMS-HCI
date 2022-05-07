using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfApp1.Service;
using WpfApp1.View.Converter;
using WpfApp1.View.Dialog.PatientDialog;

namespace WpfApp1.View.Model.Patient
{
    /// <summary>
    /// Interaction logic for PatientAppointmentsView.xaml
    /// </summary>
    public partial class PatientAppointmentsView : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private AppointmentController _appointmentController;
        private PatientController _patientController;
        private ObservableCollection<AppointmentView> _appointments;

        public ObservableCollection<AppointmentView> Appointments
        {
            get { return _appointments; }
            set
            {
                if (value != _appointments)
                {
                    _appointments = value;
                    OnPropertyChanged("Appointments");
                }
            }
        }

        public PatientAppointmentsView()
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;
            int patientId = (int)app.Properties["userId"];
            
            _appointmentController = app.AppointmentController;

            Appointments = new ObservableCollection<AppointmentView>(_appointmentController.GetPatientsAppointmentsView(patientId).ToList());
        }

        private void AddAppointment_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new AddPatientAppointmentDialog();
        }

        private void OpenMoveAppointmentDialog_Click(object sender, RoutedEventArgs e)
        {
            int appointmentId = ((AppointmentView)PatientAppointmentsDataGrid.SelectedItem).Id;
            var app = Application.Current as App;

            _appointmentController = app.AppointmentController;

            Appointment oldAppointment = _appointmentController.GetById(appointmentId);
            if (DateTime.Now.AddDays(1) > oldAppointment.Beginning)
            {
                PatientErrorMessageBox.Show("ERROR: You cannot move the appointment if it's beginning in less than one day!");
                return;
            }

            app.Properties["appointmentId"] = appointmentId;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new MovePatientAppointmentDialog();
        }

        private void RemoveAppointment_Click(object sender, RoutedEventArgs e)
        {
            int appointmentId = ((AppointmentView)PatientAppointmentsDataGrid.SelectedItem).Id;
            var app = Application.Current as App;
            int patientId = (int)app.Properties["userId"];

            _appointmentController = app.AppointmentController;
            _patientController = app.PatientController;

            _appointmentController.PatientsAppointmentDelete(patientId, appointmentId);

            PatientAppointmentsDataGrid.ItemsSource = null;
            PatientAppointmentsDataGrid.ItemsSource = _appointmentController.GetPatientsAppointmentsView(patientId);
            
            var patient = _patientController.GetById(patientId);

            if((4 - patient.NumberOfCancellations) > 0)
            {
                PatientErrorMessageBox.Show("You have " + (4 - patient.NumberOfCancellations) + " cancellations left in this month");
            } else if(4 - patient.NumberOfCancellations == 0) {
                PatientErrorMessageBox.Show("WARNING: If you cancel one more appointment in this month you will get banned.");
            } else {
                Window patientMenu = (Window)app.Properties["PatientMenu"];
                var s = new MainWindow();

                PatientErrorMessageBox.Show("You have been banned because you've cancelled too many appointments in this month!");

                _patientController.Delete(patientId);

                patientMenu.Close();
                s.Show();
            }
        }
    }
}
