using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class MeetingController
    {
        private readonly MeetingService _meetingService;

        public MeetingController(MeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        public IEnumerable<Meeting> GetAll()
        {
            return _meetingService.GetAll();
        }

        public Meeting Create(Meeting meeting)
        {
            return _meetingService.Create(meeting);
        }

    }
}
