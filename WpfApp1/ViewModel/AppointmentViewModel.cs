using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.Service;
using WpfApp1.View.Dialog.PatientDialog;
using WpfApp1.View.Model.Patient;
using WpfApp1.ViewModel.Commands.Patient;

namespace WpfApp1.ViewModel
{
    public class AppointmentViewModel : INotifyPropertyChanged
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
        private AppointmentView _appointment;

        public OpenAddAppointmentDialog AddAppointmentDialog { get; set; }
        public OpenMoveAppointmentDialog MoveAppointmentDialog { get; set; }
        public DeleteAppointment Delete { get; set; }
        public DiscardAppointment Discard { get; set; }

        public AppointmentViewModel()
        {
            LoadPatientsAppointments();
            AddAppointmentDialog = new OpenAddAppointmentDialog(this);
            MoveAppointmentDialog = new OpenMoveAppointmentDialog(this);
            Delete = new DeleteAppointment(this);
            Discard = new DiscardAppointment(this);
        }

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

        public AppointmentView Appointment
        {
            get { return _appointment; }
            set
            {
                if (value != _appointment)
                {
                    _appointment = value;
                    OnPropertyChanged("Appointment");
                }
            }
        }

        private void LoadPatientsAppointments()
        {
            var app = Application.Current as App;
            int patientId = (int)app.Properties["userId"];

            _appointmentController = app.AppointmentController;

            Appointments = new ObservableCollection<AppointmentView>(_appointmentController.GetPatientsAppointmentsView(patientId).ToList());
        }

        public void OpenAddAppointmentDialog()
        {
            var app = Application.Current as App;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new AddPatientAppointmentDialog();
        }

        public void OpenMoveAppointmentDialog()
        {
            int appointmentId = Appointment.Id;
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

        public void DeleteAppointment()
        {
            int appointmentId = Appointment.Id;
            var app = Application.Current as App;
            int patientId = (int)app.Properties["userId"];

            _appointmentController = app.AppointmentController;
            _patientController = app.PatientController;

            _appointmentController.AppointmentCancellationByPatient(patientId, appointmentId);
            LoadPatientsAppointments();
            var patient = _patientController.GetById(patientId);
            AppointmentCancellationFeedback(patient.NumberOfCancellations);
        }

        public void DiscardAppointment()
        {
            var app = Application.Current as App;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientAppointmentsView();
        }

        private void AppointmentCancellationFeedback(int numberOfcancellations)
        {
            var app = Application.Current as App;
            _patientController = app.PatientController;
            int patientId = (int)app.Properties["userId"];

            if ((4 - numberOfcancellations) > 0)
            {
                PatientErrorMessageBox.Show("You have " + (4 - numberOfcancellations) + " cancellations left in this month");
            }
            else if (4 - numberOfcancellations == 0)
            {
                PatientErrorMessageBox.Show("WARNING: If you cancel one more appointment in this month you will get banned.");
            }
            else
            {
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
