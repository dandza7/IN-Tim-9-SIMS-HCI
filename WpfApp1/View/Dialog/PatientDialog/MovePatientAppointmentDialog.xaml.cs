﻿using System;
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
using System.Windows.Shapes;
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.View.Model.Patient;
using static WpfApp1.Model.Appointment;

namespace WpfApp1.View.Dialog.PatientDialog
{
    /// <summary>
    /// Interaction logic for MovePatientAppointmentDialog.xaml
    /// </summary>
    public partial class MovePatientAppointmentDialog : Page
    {
        private AppointmentController _appointmentController;
        private DoctorController _doctorController;
        private UserController _userController;
        public ObservableCollection<User> Doctors { get; set; }
        public Appointment OldAppointment { get; set; }
        public User OldDoctor { get; set; }

        public MovePatientAppointmentDialog()
        {
            InitializeComponent();
            DataContext = this;

            var app = Application.Current as App;
            _doctorController = app.DoctorController;
            _userController = app.UserController;
            _appointmentController = app.AppointmentController;

            Doctors = new ObservableCollection<User>();
            List<Doctor> generalPracticioners = _doctorController.GetAllGeneralPracticioners().ToList();
            generalPracticioners.ForEach(doctor =>
            {
                Doctors.Add(_userController.GetById(doctor.Id));
            });

            int appointmentId = (int)app.Properties["appointmentId"];
            OldAppointment = _appointmentController.GetById(appointmentId);
            OldDoctor = _userController.GetById(OldAppointment.DoctorId);

            for(int i = 0; i < Doctors.Count; i++)
            {
                if(Doctors[i].Id == OldDoctor.Id)
                {
                    DoctorComboBox.SelectedIndex = i; break; 
                }
            }
            PriorityComboBox.SelectedIndex = 0;
        }

        private void MoveAppointment_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            _appointmentController = app.AppointmentController;
            _doctorController = app.DoctorController;

            int appointmentId = (int)app.Properties["appointmentId"];
            Appointment oldAppointment = _appointmentController.GetById(appointmentId);

            if (PriorityComboBox.SelectedValue == null) return;
            if (DoctorComboBox.SelectedValue == null) return;
            if (BeginningDTP.Text == null || EndingDTP.Text == null) return;

            if (DateTime.Parse(BeginningDTP.Text).AddHours(1) > DateTime.Parse(EndingDTP.Text))
            {
                Console.WriteLine("Morate imati bar jedan sat vremenskog intervala");
                return;
            }

            if (DateTime.Parse(BeginningDTP.Text) > DateTime.Parse(EndingDTP.Text))
            {
                Console.WriteLine("Vremenski interval mora počinjati prije svoga kraja");
                return;
            }
            
            if (DateTime.Now.AddDays(1) > oldAppointment.Beginning)
            {
                Console.WriteLine("Ostalo manje od 1 dana");
                return;
            }
            
            if (oldAppointment.Ending.AddDays(4) < DateTime.Parse(BeginningDTP.Text))
            {
                Console.WriteLine("Pomjera se više od 4 dana u budućnost");
                return;
            }

            if (DateTime.Parse(EndingDTP.Text) < DateTime.Now)
            {
                Console.WriteLine("Pomjeranje u prošlost");
                return;
            }

            if (oldAppointment.Beginning.AddDays(-4) > DateTime.Parse(EndingDTP.Text))
            {
                Console.WriteLine("Pomjera se više u 4 dana");
                return;
            }

            Doctor doctor = _doctorController.GetByUsername(((User)DoctorComboBox.SelectedValue).Username);

            app.Properties["priority"] = PriorityComboBox.SelectedValue.ToString().TrimStart("System.Windows.Controls.ComboBoxItem: ".ToCharArray());
            app.Properties["doctorId"] = doctor.Id;
            app.Properties["startOfInterval"] = DateTime.Parse(BeginningDTP.Text);
            app.Properties["endOfInterval"] = DateTime.Parse(EndingDTP.Text);
            app.Properties["oldAppointmentId"] = oldAppointment.Id;

            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new ListAvailableAppointments();
        }

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;

            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientAppointmentsView();
        }
    }
}
