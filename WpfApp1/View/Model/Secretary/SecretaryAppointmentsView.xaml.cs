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
using WpfApp1.View.Dialog;
using WpfApp1.View.Model.Patient;

namespace WpfApp1.View.Model.Secretary
{
    /// <summary>
    /// Interaction logic for SecretaryAppointmentsView.xaml
    /// </summary>
    public partial class SecretaryAppointmentsView : Page
    {
        private AppointmentController _appointmentController;
        public ObservableCollection<SecretaryAppointmentView> Appointments { get; set; }
        public SecretaryAppointmentsView()
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            Appointments = new ObservableCollection<SecretaryAppointmentView>(_appointmentController.GetSecretaryAppointmentViews().ToList());
        }
       
        private void ViewAppointmentDetails(object sender, RoutedEventArgs e)
        {
            int appointmentId = ((SecretaryAppointmentView)SecretaryAppointmentsDataGrid.SelectedItem).Id;

            var s = new SecretaryViewAppointmentsDialog(appointmentId);
            s.Show();
        }
    }
}
