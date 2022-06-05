using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace WpfApp1.View.Model.Doctor
{
    /// <summary>
    /// Interaction logic for Doctor_MyAppointments.xaml
    /// </summary>
    public partial class Doctor_MyAppointments : Page
    {
        AppointmentController _appointmentController;
        DoctorController _doctorController;
        PatientController _patientController;
        public ObservableCollection<DoctorAppointmentView> futureAppointments = new ObservableCollection<DoctorAppointmentView>();
        public ObservableCollection<DoctorAppointmentView> pastAppointments = new ObservableCollection<DoctorAppointmentView>();
        public List<int> PatientIds = new List<int>();
        public int userId = -1;
        public Doctor_MyAppointments()
        {
            InitializeComponent();
            this.DataContext = this;
            var app = Application.Current as App;
            userId = int.Parse(app.Properties["userId"].ToString());

            _appointmentController = app.AppointmentController;
            _doctorController = app.DoctorController;
            _patientController = app.PatientController;
            foreach (User p in _patientController.GetAll()) PatientIds.Add(p.Id);
            foreach (Appointment a in _appointmentController.GetAllByDoctorId(userId))
            {


                if (a.Beginning >= DateTime.Now)
                    this.futureAppointments.Add(
                            AppointmentConverter.ConvertAppointmentToDoctorAppointmentView(
                                a,
                                _doctorController.GetById(a.DoctorId),
                                _patientController.GetById(a.PatientId)
                                )
                        );
                else this.pastAppointments.Add(
                            AppointmentConverter.ConvertAppointmentToDoctorAppointmentView(
                                a,
                                _doctorController.GetById(a.DoctorId),
                                _patientController.GetById(a.PatientId)
                                )
                        );
            }
            PastAppointmentsGrid.ItemsSource = pastAppointments;
            FutureAppointmentsGrid.ItemsSource = futureAppointments;
            PatientCB.ItemsSource = PatientIds;
            ClearAppointmentForm();

        }

        private void ConfirmBT_Click(object sender, RoutedEventArgs e)
        {
            
            Enum.TryParse(TypeCB.SelectedItem.ToString(), true, out Appointment.AppointmentType type);
            if((string)FormGB.Header == "Create Appointment")
            {
                _appointmentController.Create(new Appointment(
                Convert.ToDateTime(BeginningDTP.Text),
                Convert.ToDateTime(EndingDTP.Text),
                type,
                (bool)UrgentCB.IsChecked,
                userId,
                int.Parse(PatientCB.SelectedItem.ToString()),
                _doctorController.GetById(userId).RoomId
                    )
                    );
            }
            else
            {
                _appointmentController.Update(new Appointment(
                (DateTime)BeginningDTP.Value,
                (DateTime)EndingDTP.Value,
                (Appointment.AppointmentType)TypeCB.SelectedItem,
                (bool)UrgentCB.IsChecked,
                userId,
                int.Parse(PatientCB.SelectedItem.ToString()),
                _doctorController.GetById(userId).RoomId
                    )
                    );
            }

            ClearAppointmentForm();
        }

        private void Discard_Click(object sender, RoutedEventArgs e)
        {

            ClearAppointmentForm(); 
        }
        public void ClearAppointmentForm()
        {
            UrgentCB.IsChecked = false;
            EndingDTP.Text = BeginningDTP.Text = "";
            TypeCB.SelectedItem = PatientCB.SelectedIndex = -1;
            FormGB.Header = "Create Appointment";
        }
        private void FutureAppointmentsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Appointment a = _appointmentController.GetById(((DoctorAppointmentView)FutureAppointmentsGrid.SelectedItems[0]).Id);
            BeginningDTP.Text = a.Beginning.ToString();
            EndingDTP.Text = a.Ending.ToString();
            PatientCB.SelectedIndex = a.PatientId;
            UrgentCB.IsChecked = a.IsUrgent;
            FormGB.Header = "Update Appointment";
        }
    }
}
