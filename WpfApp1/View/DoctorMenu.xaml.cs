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
using System.Windows.Shapes;
using WpfApp1.Controller;
using WpfApp1.Model;
using static WpfApp1.Model.Appointment;

namespace WpfApp1.Service
{
    /// <summary>
    /// Interaction logic for DoctorMenu.xaml
    /// </summary>
    public partial class DoctorMenu : Window
    {
        private AppointmentController _appointmentController;
        private DoctorController _doctorController;
        public IEnumerable<Appointment> appointments { get; set; }
        public List<int> Ids { get; set; }
        public List<String> Priorities = new List<String>();

        public DoctorMenu()
        {
            InitializeComponent();
            Priorities.Add("Doctor");
            Priorities.Add("Appointment time");
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            this.appointments = _appointmentController.GetAll();
            this.DataContext = this;
        }
        private void ListButton_Click(object sender, RoutedEventArgs e)
        {
            EditContainer.Visibility = Visibility.Collapsed;
            AddContainer.Visibility = Visibility.Collapsed;
            ListContainer.Visibility = Visibility.Visible;


            this.appointments = _appointmentController.GetAll();

        }

        private void XListButton_Click(object sender, RoutedEventArgs e)
        {
            ListContainer.Visibility = Visibility.Collapsed;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            EditContainer.Visibility = Visibility.Collapsed;
            ListContainer.Visibility = Visibility.Collapsed;
            AddContainer.Visibility = Visibility.Visible;


        }

        private void XAddButton_Click(object sender, RoutedEventArgs e)
        {
            AddContainer.Visibility = Visibility.Collapsed;

        }

        private void AddConfirm_Click(object sender, RoutedEventArgs e)
        {
            Appointment appointment = new Appointment(Convert.ToDateTime(StartDP.Text), Convert.ToDateTime(EndDP.Text), AppointmentType.regular, false, 1, 3, 1);
            _appointmentController.Create(appointment);
            AddContainer.Visibility = Visibility.Collapsed;

        }

        private void XEditButton_Click(object sender, RoutedEventArgs e)
        {
            EditContainer.Visibility = Visibility.Collapsed;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditContainer.Visibility = Visibility.Visible;
            ListContainer.Visibility = Visibility.Visible;
            AddContainer.Visibility = Visibility.Collapsed;
            Appointment a = ((Appointment)AppointmentGrid.SelectedItems[0]);
            IdLabel.Content = a.Id;
            EditStartDP.Text = a.Beginning.ToString();
            EditEndDP.Text = a.Ending.ToString();
        }

        private void EditConfirm_Click(object sender, RoutedEventArgs e)
        {
            Appointment appointment = new Appointment(Convert.ToInt32(IdLabel.Content), Convert.ToDateTime(EditStartDP.Text), Convert.ToDateTime(EditEndDP.Text), AppointmentType.regular, false, 1, 3, 1);
            _appointmentController.Update(appointment);
            EditContainer.Visibility = Visibility.Collapsed;
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            _appointmentController.Delete(((Appointment)AppointmentGrid.SelectedItems[0]).Id);

        }


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeleteButton.IsEnabled = true;
            EditButton.IsEnabled = true;
        }

        private void Priority_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
