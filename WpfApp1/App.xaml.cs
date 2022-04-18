using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Controller;
using WpfApp1.Repository;
using WpfApp1.Service;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : Application
    {
        private static string _projectPath = System.Reflection.Assembly.GetExecutingAssembly().Location
            .Split(new string[] { "bin" }, StringSplitOptions.None)[0];
        private string APPOINTMENT_FILE = _projectPath + "\\Resources\\Data\\appointments.csv";
        private string ROOM_FILE = _projectPath + "\\Resources\\Data\\rooms.csv";
        private string PATIENT_FILE = _projectPath + "\\Resources\\Data\\patients.csv";
        private string DOCTOR_FILE = _projectPath + "\\Resources\\Data\\doctors.csv";
        private string DRUG_FILE = _projectPath + "\\Resources\\Data\\drugs.csv";
        private string NOTIFICATION_FILE = _projectPath + "\\Resources\\Data\\notification.csv";
        private string RENOVATION_FILE = _projectPath + "\\Resources\\Data\\renovation.csv";
        private const string CSV_DELIMITER = ";";
        private const string DATETIME_FORMAT = "dd.MM.yyyy. HH:mm:ss";

        public AppointmentController AppointmentController { get; set; }
        public RoomController RoomController { get; set; }
        public DoctorController DoctorController { get; set; }
        public PatientController PatientController { get; set; }
        public DrugController DrugController { get; set; }
        public NotificationController NotificationController { get; set; }
        public RenovationController RenovationController { get; set; }

        public App()
        {
            var roomRepository = new RoomRepository(ROOM_FILE, CSV_DELIMITER);

            var roomService = new RoomService(roomRepository);

            RoomController = new RoomController(roomService);

            var patientRepository = new PatientRepository(PATIENT_FILE, CSV_DELIMITER);

            var patientService = new PatientService(patientRepository);

            PatientController = new PatientController(patientService);

            var doctorRepository = new DoctorRepository(DOCTOR_FILE, CSV_DELIMITER);

            var doctorService = new DoctorService(doctorRepository);

            DoctorController = new DoctorController(doctorService);

            var appointmentRepository = new AppointmentRepository(APPOINTMENT_FILE, CSV_DELIMITER, DATETIME_FORMAT);

            var appointmentService = new AppointmentService(appointmentRepository, doctorRepository, patientRepository);

            AppointmentController = new AppointmentController(appointmentService);

            var drugRepository = new DrugRepository(DRUG_FILE, CSV_DELIMITER);

            var drugService = new DrugService(drugRepository);

            DrugController = new DrugController(drugService);

            var notificationRepository = new NotificationRepository(NOTIFICATION_FILE, CSV_DELIMITER, DATETIME_FORMAT);

            var notificationService = new NotificationService(notificationRepository);

            NotificationController = new NotificationController(notificationService);

            var renovationRepository = new RenovationRepository(RENOVATION_FILE, CSV_DELIMITER);

            var renovationService = new RenovationService(renovationRepository, appointmentRepository);

            RenovationController = new RenovationController(renovationService);
        }
    }
}
