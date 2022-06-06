using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class MeetingService
    {
        private readonly MeetingRepository _meetingRepo;

        public MeetingService(MeetingRepository meetingRepo)
        {
            _meetingRepo = meetingRepo;
        }
        public IEnumerable<Meeting> GetAll()
        {
            return _meetingRepo.GetAll();
        }

        public Meeting Create(Meeting meeting)
        {
            return _meetingRepo.Create(meeting);
        }
    }
}
