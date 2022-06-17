using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Controller;
using WpfApp1.Model;

namespace WpfApp1.ViewModel.Secretary
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private ObservableCollection<Patient> _patients;
        private string _Month;
        private ObservableCollection<Appointment> _appointments;
        private ObservableCollection<Room> _rooms;
        private ObservableCollection<Doctor> _doctors;
        private ObservableCollection<SpecializationCountViewModel> _specializationList;
        private ObservableCollection<RoomCountViewModel> _roomList;
        private ObservableCollection<AppointmentsMonthViewModel> _monthList;
        private Dictionary<string, int> _spec;
        private int _patientCount;
        private int _doctorCount;
        private int _roomCount;
        private int _appointmentCount;
        private PatientController _patientController;
        private AppointmentController _appointmentController;
        private RoomController _roomController;
        private DoctorController _doctorController;
        public ObservableCollection<Patient> Patients
        {
            get
            {
                return _patients;
            }
            set
            {
                if (value != _patients)
                {
                    _patients = value;
                    OnPropertyChanged("Patients");
                }
            }
        }

        public ObservableCollection<SpecializationCountViewModel> SpecializationList
        {
            get
            {
                return _specializationList;
            }
            set
            {
                if (value != _specializationList)
                {
                    _specializationList = value;
                    OnPropertyChanged("SpecializationList");
                }
            }
        }
        public ObservableCollection<AppointmentsMonthViewModel> MonthList
        {
            get
            {
                return _monthList;
            }
            set
            {
                if (value != _monthList)
                {
                    _monthList = value;
                    OnPropertyChanged("MonthList");
                }
            }
        }
        public ObservableCollection<RoomCountViewModel> RoomList
        {
            get
            {
                return _roomList;
            }
            set
            {
                if (value != _roomList)
                {
                    _roomList = value;
                    OnPropertyChanged("RoomList");
                }
            }
        }
        public ObservableCollection<Doctor> Doctors
        {
            get
            {
                return _doctors;
            }
            set
            {
                if (value != _doctors)
                {
                    _doctors = value;
                    OnPropertyChanged("Doctors");
                }
            }
        }
        public ObservableCollection<Appointment> Appointments
        {
            get
            {
                return _appointments;
            }
            set
            {
                if (value != _appointments)
                {
                    _appointments = value;
                    OnPropertyChanged("PatiAppointmentsents");
                }
            }
        }
        public ObservableCollection<Room> Rooms
        {
            get
            {
                return _rooms;
            }
            set
            {
                if (value != _rooms)
                {
                    _rooms = value;
                    OnPropertyChanged("Rooms");
                }
            }
        }

        public Dictionary<string, int> Specialization {
            get
            {
                return _spec;
            }
            set
            {
                if (value != _spec)
                {
                    _spec = value;
                    OnPropertyChanged("Specialization");
                }
            }
        }
            
    
        public int PatientCount
        {
            get
            {
                return _patientCount;
            }
            set
            {
                if (value != _patientCount)
                {
                    _patientCount = value;
                    OnPropertyChanged("PatientCount");
                }
            }
        }
        public int AppointmentCount
        {
            get
            {
                return _appointmentCount;
            }
            set
            {
                if (value != _appointmentCount)
                {
                    _appointmentCount = value;
                    OnPropertyChanged("AppointmentCount");
                }
            }
        }
        public int RoomCount
        {
            get
            {
                return _roomCount;
            }
            set
            {
                if (value != _roomCount)
                {
                    _roomCount = value;
                    OnPropertyChanged("RoomCount");
                }
            }
        }
        public int DoctorCount
        {
            get
            {
                return _doctorCount;
            }
            set
            {
                if (value != _doctorCount)
                {
                    _doctorCount = value;
                    OnPropertyChanged("DoctorCount");
                }
            }
        }



        public DashboardViewModel()
        {
            LoadPatients();


        }
        private void LoadPatients()
        {
            var app = Application.Current as App;
            _patientController = app.PatientController;
            _doctorController = app.DoctorController;
            _roomController = app.RoomController;
            _appointmentController = app.AppointmentController;
            Patients = new ObservableCollection<Patient>(_patientController.GetAll());
            PatientCount = Patients.Count();
            Doctors = new ObservableCollection<Doctor>(_doctorController.GetAll());
            DoctorCount = Doctors.Count();
            Rooms = new ObservableCollection<Room>(_roomController.GetAll());
            RoomCount = Rooms.Count();
            Appointments = new ObservableCollection<Appointment>(_appointmentController.GetAll());
            AppointmentCount = Appointments.Count();

            Dictionary<string, int> specialization =
            new Dictionary<string, int>();
            SpecializationList = new ObservableCollection<SpecializationCountViewModel>();
            MonthList = new ObservableCollection<AppointmentsMonthViewModel>();
            RoomList = new ObservableCollection<RoomCountViewModel>();
            List<string> specializations = new List<string>();
            specializations.Add("radiologist");
            specializations.Add("generalPracticioner");
            specializations.Add("anesthesiologist");
            specializations.Add("cardiologist");

            foreach (string spec in specializations)
                {
                    int num = _appointmentController.GetAllBySpecialization(spec).Count();
                string specc="";
                if (spec == "radiologist") { specc = "Radiology"; }
                if (spec == "cardiologist") { specc = "Cardiology"; }
                if (spec == "anesthesiologist") { specc = "Anesthesiology"; }
                if (spec == "generalPracticioner") { specc = "General Practice"; }
                SpecializationCountViewModel s = new SpecializationCountViewModel(specc, num);
                    Console.WriteLine(s.Number);
                    SpecializationList.Add(s);
                }

            for (int i = 0; i <= 12; i++)
            {
                int num = _appointmentController.GetAllByMonth(i).Count();
                if (i == 1) { _Month = "January"; }
                if (i == 2) { _Month = "February"; }
                if (i == 3) { _Month = "March"; }
                if (i == 4) { _Month = "April"; }
                if (i == 5) { _Month = "May"; }
                if (i == 6) { _Month = "June"; }
                if (i == 7) { _Month = "July"; }
                if (i == 8) { _Month = "August"; }
                if (i == 9) { _Month = "September"; }
                if (i == 10) { _Month = "October"; }
                if (i == 11) { _Month = "November"; }
                if (i == 12) { _Month = "December"; }
                AppointmentsMonthViewModel s = new AppointmentsMonthViewModel(_Month, num);
                Console.WriteLine(s.Number);
                MonthList.Add(s);
            }
            List<string> roomTypes = new List<string>();
            roomTypes.Add("Meeting");
            roomTypes.Add("Office");
            roomTypes.Add("Operating");
            roomTypes.Add("Storage");
            foreach (string type in roomTypes)
            {
                int num = _roomController.GetAllByType(type).Count();
                RoomCountViewModel s = new RoomCountViewModel(type, num);
                Console.WriteLine(s.Number);
                RoomList.Add(s);
            }

        }
    }
}
