using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository
{
    public class MeetingRepository
    {
        private string _path;
        private string _delimiter;

        public MeetingRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
        }
        private int GetMaxId(IEnumerable<Meeting> meetings)
        {
            return meetings.Count() == 0 ? 0 : meetings.Max(meeting => meeting.Id);
        }

        public IEnumerable<Meeting> GetAll()
        {
            List<string> lines = File.ReadAllLines(_path).ToList();
            List<Meeting> meetings = new List<Meeting>();
            foreach (string line in lines)
            {
                if (line == "") continue;
                meetings.Add(ConvertCSVFormatToMeeting(line));
            }
            return meetings;
        }


        public Meeting Create(Meeting meeting)
        {
            int maxId = GetMaxId(GetAll());
            meeting.Id = ++maxId;
            AppendLineToFile(_path, ConvertMeetingToCSVFormat(meeting));
            return meeting;
        }

        private Meeting ConvertCSVFormatToMeeting(string meetingCSVFormat)
        {
            var tokens = meetingCSVFormat.Split(_delimiter.ToCharArray());
            List<string> result = tokens[4]?.Split(',').ToList();
            return new Meeting(int.Parse(tokens[0]),
                DateTime.Parse(tokens[1]),
                DateTime.Parse(tokens[2]),
                int.Parse(tokens[3]),
                result
                );
        }

        private string ConvertMeetingToCSVFormat(Meeting meeting)
        {
            string a = "";
            int count = meeting.Users.Count();
            int i = 0;
            foreach (string b in meeting.Users)
            {
                if(i<count-1)
                a = a + b + ",";
                else { a = a + b; }
                i++;
            }

            return string.Join(_delimiter,
                meeting.Id,
                meeting.Beginning,
                meeting.Ending,
                meeting.RoomId,
                a);
        }


        private void AppendLineToFile(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }
    }
}