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
using WpfApp1.View.Dialog;
using WpfApp1.View.Model.Patient;

namespace WpfApp1.View.Model.Secretary
{
    /// <summary>
    /// Interaction logic for SecretaryAppointmentsView.xaml
    /// </summary>
    public partial class SecretaryAppointmentsView : Page, INotifyPropertyChanged
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
        public ObservableCollection<SecretaryAppointmentView> Appointments { get; set; }
        public SecretaryAppointmentsView()
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            Add_New_Appointment.Focus();
            Appointments = new ObservableCollection<SecretaryAppointmentView>(_appointmentController.GetSecretaryAppointmentViews().ToList());
            app.Properties["SecretaryAppointmentsDataGrid"] = SecretaryAppointmentsDataGrid;
        }
       
        private void ViewAppointmentDetails(object sender, RoutedEventArgs e)
        {
            int appointmentId = ((SecretaryAppointmentView)SecretaryAppointmentsDataGrid.SelectedItem).Id;

            var s = new SecretaryViewAppointmentsDialog(appointmentId);
            s.Show();
        }
        private void Add_New_Appointment_Click(object sender, RoutedEventArgs e)
        {
 
            var s = new SecretaryAddNewAppointmentDialog();
            s.Show();
        }
        private void DeleteAppointment(object sender, RoutedEventArgs e)
        {
            int appointmentId = ((SecretaryAppointmentView)SecretaryAppointmentsDataGrid.SelectedItem).Id;
            var app = Application.Current as App;

            _appointmentController = app.AppointmentController;
            _appointmentController.Delete(appointmentId);

            SecretaryAppointmentsDataGrid.ItemsSource = null;
            SecretaryAppointmentsDataGrid.ItemsSource = _appointmentController.GetSecretaryAppointmentViews();
        }

        private void Add_New_Urgent_Appointment_Click(object sender, RoutedEventArgs e)
        {
            var s = new SecretaryAddNewUrgentAppointmentDialog();
            s.Show();
        }

        private void Show_Report_Click(object sender, RoutedEventArgs e)
        {
            var s = new SecretaryReportDialog();
            s.Show();
        }
    }
}
