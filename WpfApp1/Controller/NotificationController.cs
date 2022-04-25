﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class NotificationController
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public IEnumerable<Notification> GetAll()
        {
            return _notificationService.GetAll();
        }

        public IEnumerable<Notification> GetUsersNotDeletedNotifications(int userId)
        {
            return _notificationService.GetUsersNotDeletedNotifications(userId);
        }

        public IEnumerable<Notification> GetUsersNotifications(int userId)
        {
            return _notificationService.GetUsersNotifications(userId);
        }

        public Notification Create(Notification notification)
        {
            return _notificationService.Create(notification);
        }

        public Notification Update(Notification notification)
        {
            return _notificationService.Update(notification);
        }

        public bool Delete(int id)
        {
            return _notificationService.Delete(id);
        }

        public bool DeleteLogically(int id)
        {
            return _notificationService.DeleteLogically(id);
        }

        public void DeleteOldUsersNotifications(int id)
        {
            _notificationService.DeleteOldUsersNotifications(id);
        }

        public void GetScheduledPatientsNotifications(int patientId)
        {
            _notificationService.GetScheduledPatientsNotifications(patientId);
        }
    }
}
