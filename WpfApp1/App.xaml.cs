﻿using System;
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
        private const string CSV_DELIMITER = ";";
        private const string APPOINTMENT_DATETIME_FORMAT = "dd.MM.yyyy. HH:mm:ss";

        public AppointmentController AppointmentController { get; set; }
        public RoomController RoomController { get; set; }

        public App()
        {
            var roomRepository = new RoomRepository(ROOM_FILE, CSV_DELIMITER);

            var roomService = new RoomService(roomRepository);

            RoomController = new RoomController(roomService);

            var appointmentRepository = new AppointmentRepository(APPOINTMENT_FILE, CSV_DELIMITER, APPOINTMENT_DATETIME_FORMAT);

            var appointmentService = new AppointmentService(appointmentRepository);

            AppointmentController = new AppointmentController(appointmentService);
        }
    }
}
