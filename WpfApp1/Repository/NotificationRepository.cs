﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository
{
    public class NotificationRepository
    {
        private string _path;
        private string _delimiter;
        private readonly string _datetimeFormat;

        public NotificationRepository(string path, string delimiter, string datetimeFormat)
        {
            _path = path;
            _delimiter = delimiter;
            _datetimeFormat = datetimeFormat;
        }

        private int GetMaxId(IEnumerable<Notification> notifications)
        {
            return notifications.Count() == 0 ? 0 : notifications.Max(notification => notification.Id);
        }

        public IEnumerable<Notification> GetAll()
        {
            List<string> lines = File.ReadAllLines(_path).ToList();
            List<Notification> notifications = new List<Notification>();
            foreach (string line in lines)
            {
                if (line == "") continue;
                notifications.Add(ConvertCSVFormatToNotification(line));
            }
            return notifications;
        }
        public Notification Create(Notification notification)
        {
            int maxId = GetMaxId(GetAll());
            notification.Id = ++maxId;
            AppendLineToFile(_path, ConvertNotificationToCSVFormat(notification));
            return notification;
        }

        public Notification Update(Notification notification)
        {
            List<Notification> notifications = GetAll().ToList();
            List<string> newFile = new List<string>();
            foreach (Notification n in notifications)
            {
                if (n.Id == notification.Id)
                {
                    n.Date = notification.Date;
                    n.Content = notification.Content;
                    n.Title = notification.Title;
                }
                newFile.Add(ConvertNotificationToCSVFormat(n));
            }
            File.WriteAllLines(_path, newFile);
            return notification;
        }

        public bool Delete(int id)
        {
            List<Notification> notifications = GetAll().ToList();
            List<string> newFile = new List<string>();
            bool isDeleted = false;
            foreach (Notification n in notifications)
            {
                if (n.Id != id)
                {
                    newFile.Add(ConvertNotificationToCSVFormat(n));
                    isDeleted = true;
                }
            }
            File.WriteAllLines(_path, newFile);
            return isDeleted;
        }

        private Notification ConvertCSVFormatToNotification(string notificationCSVFormat)
        {
            var tokens = notificationCSVFormat.Split(_delimiter.ToCharArray());
            return new Notification(int.Parse(tokens[0]),
                DateTime.Parse(tokens[1]),
                tokens[2],
                tokens[3]);
        }
        private string ConvertNotificationToCSVFormat(Notification notification)
        {
            return string.Join(_delimiter,
                notification.Id,
                notification.Date.ToString(_datetimeFormat),
                notification.Content,
                notification.Title);
        }

        private void AppendLineToFile(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }
    }
}
