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
        private string USER_FILE = _projectPath + "\\Resources\\Data\\user.csv";
        private string DOCTOR_FILE = _projectPath + "\\Resources\\Data\\doctors.csv";
        private string DRUG_FILE = _projectPath + "\\Resources\\Data\\drugs.csv";
        private string NOTIFICATION_FILE = _projectPath + "\\Resources\\Data\\notification.csv";
        private string RENOVATION_FILE = _projectPath + "\\Resources\\Data\\renovation.csv";
        private string THERAPY_FILE = _projectPath + "\\Resources\\Data\\therapy.csv";
        private string INVENTORY_FILE = _projectPath + "\\Resources\\Data\\inventory.csv";
        private string INVENTORY_MOVING_FILE = _projectPath + "\\Resources\\Data\\inventoryMoving.csv";
        private string MEDICAL_RECORD_FILE = _projectPath + "\\Resources\\Data\\medical_record.csv";
        private const string CSV_DELIMITER = ";";
        private const string DATETIME_FORMAT = "dd.MM.yyyy. HH:mm:ss";

        public AppointmentController AppointmentController { get; set; }
        public RoomController RoomController { get; set; }
        public DoctorController DoctorController { get; set; }
        public PatientController PatientController { get; set; }
        public DrugController DrugController { get; set; }
        public NotificationController NotificationController { get; set; }
        public RenovationController RenovationController { get; set; }
        public TherapyController TherapyController { get; set; }
        public InventoryController InventoryController { get; set; }
        public InventoryMovingController InventoryMovingController { get; set; }
        public UserController UserController { get; set; } 
        public MedicalRecordController MedicalRecordController { get; set; }

        public App()
        {
            var notificationRepository = new NotificationRepository(NOTIFICATION_FILE, CSV_DELIMITER, DATETIME_FORMAT);
            var therapyRepository = new TherapyRepository(THERAPY_FILE, CSV_DELIMITER);
            var roomRepository = new RoomRepository(ROOM_FILE, CSV_DELIMITER);
            var patientRepository = new PatientRepository(PATIENT_FILE, CSV_DELIMITER);
            var doctorRepository = new DoctorRepository(DOCTOR_FILE, CSV_DELIMITER);
            var appointmentRepository = new AppointmentRepository(APPOINTMENT_FILE, CSV_DELIMITER, DATETIME_FORMAT);
            var drugRepository = new DrugRepository(DRUG_FILE, CSV_DELIMITER);
            var renovationRepository = new RenovationRepository(RENOVATION_FILE, CSV_DELIMITER);
            var inventoryRepository = new InventoryRepository(INVENTORY_FILE, CSV_DELIMITER);
            var inventoryMovingRepository = new InventoryMovingRepository(INVENTORY_MOVING_FILE, CSV_DELIMITER);
            var userRepository = new UserRepository(USER_FILE, CSV_DELIMITER);
            var medicalRecordRepository = new MedicalRecordRepository(MEDICAL_RECORD_FILE, CSV_DELIMITER);

            var notificationService = new NotificationService(notificationRepository, drugRepository, patientRepository, medicalRecordRepository, therapyRepository);
            NotificationController = new NotificationController(notificationService);

            var roomService = new RoomService(roomRepository);
            RoomController = new RoomController(roomService);

            var patientService = new PatientService(userRepository, patientRepository, therapyRepository, medicalRecordRepository, drugRepository);
            PatientController = new PatientController(patientService);

            var doctorService = new DoctorService(userRepository, doctorRepository);
            DoctorController = new DoctorController(doctorService);

            var appointmentService = new AppointmentService(appointmentRepository, doctorRepository, patientRepository, roomRepository, userRepository);
            AppointmentController = new AppointmentController(appointmentService);

            var drugService = new DrugService(drugRepository);
            DrugController = new DrugController(drugService);

            var renovationService = new RenovationService(renovationRepository, appointmentRepository);
            RenovationController = new RenovationController(renovationService);

            var therapyService = new TherapyService(therapyRepository);
            TherapyController = new TherapyController(therapyService);

            var inventoryService = new InventoryService(inventoryRepository, roomRepository, inventoryMovingRepository);
            InventoryController = new InventoryController(inventoryService);

            var inventoryMovingService = new InventoryMovingService(inventoryMovingRepository, inventoryRepository);
            InventoryMovingController = new InventoryMovingController(inventoryMovingService);

            var userService = new UserService(userRepository, notificationRepository);
            UserController = new UserController(userService);

            var medicalRecordService = new MedicalRecordService(medicalRecordRepository);
            MedicalRecordController = new MedicalRecordController(medicalRecordService);
        }
    }
}
