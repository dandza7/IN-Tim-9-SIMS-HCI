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
        private string ALLERGY_FILE = _projectPath + "\\Resources\\Data\\allergy.csv";
        private string DOCTORS_REPORT_FILE = _projectPath + "\\Resources\\Data\\doctors_report.csv";
        private string SURVEY_FILE = _projectPath + "\\Resources\\Data\\survey.csv";
        private string DYNAMIC_EQ_REQUEST_FILE = _projectPath + "\\Resources\\Data\\dynamic_equipment_requests.csv";
        private string REQUEST_FILE = _projectPath + "\\Resources\\Data\\doctors_requests.csv";
        private string NOTE_FILE = _projectPath + "\\Resources\\Data\\note.csv";
        private string MEETING_FILE = _projectPath + "\\Resources\\Data\\meeting.csv";

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
        public AllergyController AllergyController { get; set; }
        public DoctorsReportController DoctorsReportController { get; set; }
        public SurveyController SurveyController { get; set; }
        public DynamicEquipmentRequestController DynamicEquipmentReqeustController { get; set; }
        public RequestController RequestController { get; set; }
        public NoteController NoteController { get; set; }
        public MeetingController MeetingController { get; set; }

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjUxNjk3QDMyMzAyZTMxMmUzMG10TS85NWoyeTI4VEd5alEvcjFFSU5kV1BZWnRhVUxRQVpsSTZndkFGbm89");
            var notificationRepository = new NotificationRepository(NOTIFICATION_FILE, CSV_DELIMITER, DATETIME_FORMAT);
            var therapyRepository = new TherapyRepository(THERAPY_FILE, CSV_DELIMITER);
            var roomRepository = new RoomRepository(ROOM_FILE, CSV_DELIMITER);
            var patientRepository = new PatientRepository(PATIENT_FILE, CSV_DELIMITER, DATETIME_FORMAT);
            var doctorRepository = new DoctorRepository(DOCTOR_FILE, CSV_DELIMITER);
            var appointmentRepository = new AppointmentRepository(APPOINTMENT_FILE, CSV_DELIMITER, DATETIME_FORMAT);
            var drugRepository = new DrugRepository(DRUG_FILE, CSV_DELIMITER);
            var renovationRepository = new RenovationRepository(RENOVATION_FILE, CSV_DELIMITER);
            var inventoryRepository = new InventoryRepository(INVENTORY_FILE, CSV_DELIMITER);
            var inventoryMovingRepository = new InventoryMovingRepository(INVENTORY_MOVING_FILE, CSV_DELIMITER);
            var userRepository = new UserRepository(USER_FILE, CSV_DELIMITER);
            var medicalRecordRepository = new MedicalRecordRepository(MEDICAL_RECORD_FILE, CSV_DELIMITER);
            var allergyRepository = new AllergyRepository(ALLERGY_FILE, CSV_DELIMITER);
            var doctorsReportRepository = new DoctorsReportRepository(DOCTORS_REPORT_FILE, CSV_DELIMITER);
            var surveyRepository = new SurveyRepository(SURVEY_FILE, CSV_DELIMITER);
            var dynamicEqRequestRepository = new DynamicEquipmentRequestRepository(DYNAMIC_EQ_REQUEST_FILE, CSV_DELIMITER);
            var requestRepository = new RequestRepository(REQUEST_FILE, CSV_DELIMITER, DATETIME_FORMAT);
            var noteRepository = new NoteRepository(NOTE_FILE, CSV_DELIMITER, DATETIME_FORMAT);
            var meetingRepository = new MeetingRepository(MEETING_FILE, CSV_DELIMITER);

            var notificationService = new NotificationService(notificationRepository, drugRepository, medicalRecordRepository, therapyRepository, noteRepository);
            NotificationController = new NotificationController(notificationService);

            var patientService = new PatientService(userRepository, patientRepository, medicalRecordRepository, appointmentRepository);
            PatientController = new PatientController(patientService);

            var doctorService = new DoctorService(userRepository, doctorRepository);
            DoctorController = new DoctorController(doctorService);

            var appointmentService = new AppointmentService(appointmentRepository, doctorRepository, patientRepository, 
                                                            roomRepository, userRepository, renovationRepository, notificationRepository);
            AppointmentController = new AppointmentController(appointmentService);

            var drugService = new DrugService(drugRepository);
            DrugController = new DrugController(drugService);

            var renovationService = new RenovationService(renovationRepository, appointmentRepository, roomRepository);
            RenovationController = new RenovationController(renovationService);

            var therapyService = new TherapyService(therapyRepository, medicalRecordRepository, drugRepository);
            TherapyController = new TherapyController(therapyService);

            var inventoryMovingService = new InventoryMovingService(inventoryMovingRepository, inventoryRepository);
            InventoryMovingController = new InventoryMovingController(inventoryMovingService);

            var userService = new UserService(userRepository);
            UserController = new UserController(userService);

            var medicalRecordService = new MedicalRecordService(medicalRecordRepository);
            MedicalRecordController = new MedicalRecordController(medicalRecordService);

            var allergyService = new AllergyService(allergyRepository);
            AllergyController = new AllergyController(allergyService);

            var doctorsReportService = new DoctorsReportService(doctorsReportRepository,appointmentRepository);
            DoctorsReportController = new DoctorsReportController(doctorsReportService);

            var surveyService = new SurveyService(surveyRepository, appointmentRepository, doctorRepository);
            SurveyController = new SurveyController(surveyService);

            var dynamicEquipmentRequestService = new DynamicEquipmentRequestService(dynamicEqRequestRepository, inventoryRepository);
            DynamicEquipmentReqeustController = new DynamicEquipmentRequestController(dynamicEquipmentRequestService);

            var requestService = new RequestService(requestRepository,doctorRepository);
            RequestController = new RequestController(requestService);

            var noteService = new NoteService(noteRepository);
            NoteController = new NoteController(noteService);

            var meetingService = new MeetingService(meetingRepository, appointmentRepository, roomRepository, renovationRepository);
            MeetingController = new MeetingController(meetingService);

            var inventoryService = new InventoryService(inventoryRepository, roomRepository, inventoryMovingRepository);

            var roomService = new RoomService(roomRepository, doctorService, inventoryMovingService, inventoryService, renovationService, appointmentService);
            RoomController = new RoomController(roomService);

            InventoryController = new InventoryController(inventoryService, roomService);
        }
    }
}
